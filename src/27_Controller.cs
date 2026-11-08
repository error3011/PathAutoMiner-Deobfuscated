// ========================== CONTROLLER MODE ==========================
// Controller PB: lists PAM ships in antenna range and sends them commands (ported from PAM 1.3.1)

void ControllerMainLoop(string argument)
{
    if (firstRun) connectedShips = new List<ShipInfo>();

    String senderId = "";
    String extra = "";

    if (!IsInternalMessage(argument)) ProcessControllerArgument(argument);

    // Process every queued ship broadcast (one per run fell behind with many ships), sort once.
    bool shipsChanged = false;
    int msgBudget = 50;
    while (broadcastListener != null && broadcastListener.HasPendingMessage && msgBudget-- > 0)
    {
        MyIGCMessage msg = broadcastListener.AcceptMessage();
        String data = msg.Data as String;
        if (data == null) continue;
        if (ParseBroadcastMessage(ref data, out senderId, out extra) && senderId != "" && senderId != BROADCAST_SELF_ID)
        {
            ShipInfo ship = FindShip(senderId, "");
            if (ship == null) { ship = new ShipInfo(senderId); connectedShips.Add(ship); }
            ship.ProcessCommand(extra, false, false, true);
            UpdateShipInfo(ship, data);
            shipsChanged = true;
        }
    }
    if (shipsChanged)
        connectedShips.Sort(delegate (ShipInfo a, ShipInfo b)
        {
            if (a.name == null && b.name == null) return 0;
            else if (a.name == null) return -1;
            else if (b.name == null) return 1;
            else return a.name.CompareTo(b.name);
        });

    if (isSlowTick || firstRun)
    {
        if (scanCountdown <= 0 || firstRun)
        {
            statusMessage = "";
            firstRun = false;
            ScanControllerBlocks();
            InitBroadcast();
            lastScanTime = TimeNow();
        }
    }

    if (isTick) UpdateDockControl(); // station dock control (clearance for one ship at a time)
    if (isTick) UpdateJobCoordination(); // co-op jobs: hand out holes
    if (isTick) UpdateTrafficControl();  // all ships at once: who gives way
    if (isTick) UpdateStagedHome();      // "Move home" for all: one ship after the other
    if (isTick) UpdateScreens();         // recorded paths for the map
    if (isTick && tcSendTick == 0) UpdateDebugDraw(); // 3D debug drawing (DebugAPI mod), twice a second
    UpdateControllerDisplays();
}

void UpdateShipInfo(ShipInfo ship, String data)
{
    ship.lastMessageTime = TimeNow();
    String field = "";
    ExtractField(ref data, out ship.version, BROADCAST_SEP);
    ExtractField(ref data, out ship.name, BROADCAST_SEP);
    if (ship.version != VERSION) return;
    String modeStr = "";
    String[] posStr = new string[3];
    ExtractField(ref data, out modeStr, BROADCAST_SEP);
    ExtractField(ref data, out posStr[0], BROADCAST_SEP);
    ExtractField(ref data, out posStr[1], BROADCAST_SEP);
    ExtractField(ref data, out posStr[2], BROADCAST_SEP);
    ExtractField(ref data, out ship.stateText, BROADCAST_SEP);
    ExtractField(ref data, out ship.statusMessage, BROADCAST_SEP);
    ExtractField(ref data, out field, BROADCAST_SEP);
    ExtractField(ref data, out ship.menuText, BROADCAST_SEP);
    String volStr = "", maxVolStr = "";
    ExtractField(ref data, out volStr, BROADCAST_SEP);
    ExtractField(ref data, out maxVolStr, BROADCAST_SEP);

    ship.items.Clear();
    while (true)
    {
        String itemStr;
        if (!ExtractField(ref data, out itemStr, BROADCAST_SEP)) break;
        String[] itemParts = itemStr.Split('/');
        if (itemParts.Count() < 3) continue;
        int amount = 0;
        if (!int.TryParse(itemParts[2], out amount)) continue;
        ship.items.Add(new ItemInfo(itemParts[0], itemParts[1], amount, ItemLocation.InCargo));
    }

    int modeInt = 0;
    if (!int.TryParse(field, out ship.menuIndex)) ship.menuIndex = 0;
    if (!int.TryParse(modeStr, out modeInt)) ship.shipMode = ShipMode.Unknown;
    if (!float.TryParse(posStr[0], out ship.position.X)) ship.position.X = 0;
    if (!float.TryParse(posStr[1], out ship.position.Y)) ship.position.Y = 0;
    if (!float.TryParse(posStr[2], out ship.position.Z)) ship.position.Z = 0;
    if (!float.TryParse(volStr, out ship.currentVolume)) ship.currentVolume = 0;
    if (!float.TryParse(maxVolStr, out ship.maxVolume)) ship.maxVolume = 0;
    ship.shipMode = (ShipMode)modeInt;
    ship.distance = (int)Math.Round(Vector3.Distance(Me.GetPosition(), ship.position));
    ship.totalItems = 0;
    for (int i = 0; i < ship.items.Count(); i++) ship.totalItems += ship.items[i].amount;
}

ShipInfo FindShip(String id, String name)
{
    id = id.ToUpper(); name = name.ToUpper();
    for (int i = 0; i < connectedShips.Count; i++)
    {
        if (id != "" && connectedShips[i].id.ToUpper() == id) return connectedShips[i];
        if (name != "" && connectedShips[i].name != null && connectedShips[i].name.ToUpper() == name) return connectedShips[i];
    }
    return null;
}

// last command sent from the menus: its result is shown only on the page it was sent from, for a while
String cmdSent = "";
ShipInfo cmdTarget = null;
DateTime cmdTime = DateTime.MinValue;
List<ShipInfo> cmdShips = new List<ShipInfo>();
const float CMD_RESULT_TIME = 20f; //[s]

bool ShowCmdResult(ShipInfo target, String cmd)
{
    return cmd == cmdSent && target == cmdTarget && ((TimeNow() - cmdTime).TotalSeconds < CMD_RESULT_TIME ||
        (cmd == "HOMEPOS" && (homeQueue.Count > 0 || homeCurrent != null))); // staged "Move home" still running
}

// one line per ship the last command went to: received / pending / no answer
String GetCmdResultLines(ShipInfo target)
{
    if (cmdSent == "" || !ShowCmdResult(target, cmdSent)) return "";
    String s = "";
    int ok = 0, shown = 0;
    for (int i = 0; i < cmdShips.Count; i++)
    {
        CommandState st = cmdShips[i].ProcessCommand(cmdSent, false, false, false);
        if (st == CommandState.Received) ok++;
        bool queued = homeQueue.Contains(cmdShips[i]);
        if (shown++ < 8) s += "  " + cmdShips[i].name + ": " + (queued ? "waiting to leave (" + (homeQueue.IndexOf(cmdShips[i]) + 1) + ")" : st == CommandState.Received ? "received" :
            st == CommandState.Pending ? "pending..." : st == CommandState.NoAnswer ? "no answer!" : "-") + "\n";
    }
    if (shown > 8) s += "  +" + (shown - 8) + " more\n";
    return "--------------------------------------------\n" + cmdSent + ": " + ok + "/" + cmdShips.Count + " received\n" + s;
}

void SendCommandToShip(ShipInfo ship, String cmd)
{
    cmdSent = cmd; cmdTarget = ship; cmdTime = TimeNow();
    cmdShips.Clear();
    if (ship == null) cmdShips.AddRange(connectedShips); else cmdShips.Add(ship);
    // a new command replaces a staged "Move home" (for that ship, or for all)
    if (ship == null) { homeQueue.Clear(); homeCurrent = null; } else homeQueue.Remove(ship);
    if (ship == null && cmd == "HOMEPOS") { StartStagedHome(); return; } // one at a time, see UpdateStagedHome
    if (ship == null)
    {
        for (int i = 0; i < connectedShips.Count; i++)
            connectedShips[i].ProcessCommand(cmd, true, false, false);
        SendBroadcastTo(cmd, "*", cmd);
    }
    else
    {
        ship.ProcessCommand(cmd, true, false, false);
        SendBroadcastTo(cmd, ship.id, cmd);
    }
}

void ProcessControllerArgument(string argument)
{
    if (argument == "") return;
    var parts = argument.ToUpper().Split(' ');
    parts.DefaultIfEmpty("");
    var cmd = parts.ElementAtOrDefault(0);
    var arg1 = parts.ElementAtOrDefault(1);
    String invalidMsg = "Invalid argument: " + argument;

    if (currentMenu == MenuPage.ShipList)
    {
        if (selectedShip != null)
        {
            switch (cmd)
            {
                case "UP": { if (menuIndex < 2) break; if (selectedShip.menuIndex == 0) { menuIndex = 1; return; } SendCommandToShip(selectedShip, "UP"); return; }
                case "DOWN": { if (menuIndex < 1) break; if (menuIndex == 1) { SendCommandToShip(selectedShip, "MRES"); break; } SendCommandToShip(selectedShip, "DOWN"); break; }
                case "APPLY": { if (menuIndex < 2) break; SendCommandToShip(selectedShip, "APPLY"); return; }
            }
        }
    }

    switch (cmd)
    {
        case "UP": this.MenuUp(false); return;
        case "DOWN": this.MenuDown(false); return;
        case "APPLY": this.BuildControllerScreen(true); return;
    }

    switch (cmd)
    {
        case "CLEAR": ClearShipList(); return;
        case "SENDALL":
            if (String.IsNullOrEmpty(arg1)) { statusMessage = "Usage: SENDALL <cmd>"; return; }
            SendCommandToShip(null, arg1); return;
        case "SEND":
            if (argument.Length <= "SEND".Length + 1) { statusMessage = "Usage: SEND <ship>:<cmd>"; return; }
            SendToNamedShip(argument.Remove(0, "SEND".Length + 1)); return;
    }
    statusMessage = invalidMsg;
}

void SendToNamedShip(String arg)
{
    if (arg == "") return;
    var parts = arg.Split(':');
    if (parts.Length != 2) { statusMessage = "Missing separator \":\""; return; }
    parts.DefaultIfEmpty("");
    String shipName = parts.ElementAtOrDefault(0).Trim();
    ShipInfo ship = FindShip("", shipName);
    if (ship != null) SendCommandToShip(ship, parts.ElementAtOrDefault(1).Trim());
    else statusMessage = "Unknown ship: " + shipName;
}

void ClearShipList() { connectedShips.Clear(); selectedShip = null; NavigateToMenu(MenuPage.Main); menuIndex = 0; }

void ScanControllerBlocks()
{
    List<IMyTerminalBlock> taggedBlocks = new List<IMyTerminalBlock>();
    gridTerminalSystem.GetBlocksOfType(lcdPanels, IsOnSameGrid);
    gridTerminalSystem.SearchBlocksOfName(pamTag.Substring(0, pamTag.Length - 1) + ":", taggedBlocks,
        q => q.CubeGrid == Me.CubeGrid && q is IMyTextSurfaceProvider);
    lcdPanels = FilterByTag(lcdPanels, pamTag, true);
    SetupTaggedSurfaces(taggedBlocks);
    SetupDebugPanels();
    ScanDockArea();
    SetLCDFormat(lcdPanels.FindAll(q => ModeOf(q) < LCDMode.Fleet), setLCDFontAndSize, 1.15f, false); // the new screens keep their own format
    SetLCDFormat(textSurfaces, setLCDFontAndSize, 1.15f, true);

    // Write the LCD mode template into empty Custom Data; flag outdated Custom Data.
    String helpText = GetLCDModeHelp(false);
    String obsoleteNote = "//Custom Data is obsolete, please delete it.\n\n";
    foreach (IMyTerminalBlock panel in lcdPanels)
    {
        if (panel.CustomData == "") { panel.CustomData = GetLCDModeHelp(true); continue; }
        if (!panel.CustomData.Contains(helpText))
            if (!panel.CustomData.Contains(obsoleteNote)) panel.CustomData = obsoleteNote + panel.CustomData;
    }
}

void UpdateControllerDisplays()
{
    String echoText = "[PAM]-Controller\n\n" + "Run-arguments: (Type without:[ ])\n" + "———————————————\n" +
        "[UP] Menu navigation up\n" + "[DOWN] Menu navigation down\n" + "[APPLY] Apply menu point\n" +
        "[CLEAR] Clear miner list\n" + "[SEND ship:cmd] Send to a ship*\n" + "[SENDALL cmd] Send to all ships*\n" +
        "———————————————\n\n" + "*[SEND] = Cmd to one ship:\n" + " e.g.: \"SEND Miner 1:homepos\"\n\n" +
        "*[SENDALL] = Cmd to all ships:\n" + " e.g.: \"SENDALL homepos\"\n\n";

    for (int i = 0; i < textSurfaces.Count; i++)
        textSurfaces[i].WriteText(BuildControllerScreenForMode(LCDMode.MainCompact, "0", false));
    bool draw = ++screenTick % 6 == 0; // drawn screens about once a second
    for (int i = 0; i < lcdPanels.Count; i++)
    {
        LCDMode mode = LCDMode.Unknown;
        String param = "";
        IMyTextPanel p = lcdPanels[i];
        ParseLCDMode(p, out mode, out param);
        if (SpriteMode(mode)) { if (draw) DrawSpriteScreen(p, mode, param); continue; }
        if (p.ContentType != ContentType.TEXT_AND_IMAGE) p.ContentType = ContentType.TEXT_AND_IMAGE;
        if (mode == LCDMode.Fleet && setLCDFontAndSize && p.Font != "Monospace") p.Font = "Monospace"; // table columns
        p.WriteText(BuildControllerScreenForMode(mode, param, i == 0));
    }
    Echo(echoText + GetDebugDrawLegend());
    for (int i = 0; i < debugPanels.Count; i++)
    {
        IMyTextPanel panel = debugPanels[i];
        String data = panel.CustomData.ToUpper();
        if (data == "DEBUG") panel.WriteText("" + "\n" + "");
        if (data == "INSTRUCTIONS") panel.WriteText(GetInstructionInfo());
    }
}

String BuildControllerScreen(bool applyAction)
{
    int dummy = 0;
    return BuildControllerScreen(applyAction, 0, ref dummy, false, 1);
}

// Controller menu. pageOffset/scrollState/compact/pageSize are used by the paged LCD modes.
String BuildControllerScreen(bool applyAction, int pageOffset, ref int scrollState, bool compact, int pageSize)
{
    String text = "";
    String separator = "——————————————————\n";
    String thinSeparator = "--------------------------------------------\n";

    if (selectedShip == null || currentMenu == MenuPage.Main || currentMenu == MenuPage.SendCommandAll)
        text += "[PAM]-Controller | " + connectedShips.Count + " Connected ships" + "\n" + GetDockAreaLine() + GetJobLines() + GetTrafficControlLine();
    else text += GetShipStatusLine(selectedShip) + "\n";
    text += separator;
    int headerLines = text.Split('\n').Length - 1; // title, docking area, jobs, traffic, separator: the list gets the rest

    int idx = 0;
    int sel = menuIndex;
    menuItemCount = 0;

    if (statusMessage != "")
    {
        AddMenuItem(ref text, sel, idx++, applyAction, statusMessage);
    }
    else if (currentMenu == MenuPage.Main)
    {
        if (selectedShip == null && connectedShips.Count >= 1) selectedShip = connectedShips[0];
        String list = "";
        if (AddMenuItem(ref list, sel, idx++, applyAction, " Send command to all")) NavigateToMenu(MenuPage.SendCommandAll);
        int selectedLine = 0;
        for (int s = 0; s < connectedShips.Count; s++)
        {
            if (compact) list += "\n";
            ShipInfo ship = connectedShips[s];
            if (sel == idx || sel == idx + 1) selectedShip = ship;
            if (idx == sel) selectedLine = list.Split('\n').Length - 1;
            if (AddMenuItem(ref list, sel, idx++, applyAction, " " + GetShipStatusLine(ship)))
            {
                selectedShip = ship;
                NavigateToMenu(MenuPage.ShipList);
                selectedShip.ProcessCommand("", false, true, false);
            }
            if (idx == sel) selectedLine = list.Split('\n').Length - 1;
            if (AddMenuItem(ref list, sel, idx++, applyAction, " " + (compact ? "" : "  ") + GetShipCargoLine(ship)))
            {
                selectedShip = ship;
                NavigateToMenu(MenuPage.ShipInventory);
            }
        }
        int visibleLines = Math.Max(3, pageSize - headerLines);
        visibleLines += Math.Max(0, (pageOffset - 1)) * pageSize;
        text += ScrollText(visibleLines, list, selectedLine, ref scrollState);
    }
    else if (currentMenu == MenuPage.SendCommand || currentMenu == MenuPage.SendCommandAll)
    {
        MenuPage backPage = currentMenu == MenuPage.SendCommand ? MenuPage.ShipList : MenuPage.Main;
        ShipInfo target = null; // null = all ships
        if (currentMenu == MenuPage.SendCommand) target = selectedShip;
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) { NavigateToMenu(backPage); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Stop] " + GetCommandStatusText(target, "STOP"))) { SendCommandToShip(target, "STOP"); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Continue] " + GetCommandStatusText(target, "CONT"))) { SendCommandToShip(target, "CONT"); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Move home] " + GetCommandStatusText(target, "HOMEPOS"))) { SendCommandToShip(target, "HOMEPOS"); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Move to job] " + GetCommandStatusText(target, "JOBPOS"))) { SendCommandToShip(target, "JOBPOS"); }
        if (AddMenuItem(ref text, sel, idx++, applyAction, " [Full simulation] " + GetCommandStatusText(target, "FULL"))) { SendCommandToShip(target, "FULL"); }
        if (target != null && target.shipMode == ShipMode.Shuttle)
            if (AddMenuItem(ref text, sel, idx++, applyAction, " [Undock] " + GetCommandStatusText(target, "UNDOCK"))) { SendCommandToShip(target, "UNDOCK"); }
        text += GetCmdResultLines(target);
    }
    else if (currentMenu == MenuPage.ShipList)
    {
        // Ship detail: mirrors the remote ship's own menu, navigated via UP/DOWN/APPLY forwarding.
        String backLine = "";
        if (AddMenuItem(ref backLine, sel, idx++, applyAction, " Back")) { NavigateToMenu(MenuPage.Main); }
        text += backLine.Substring(0, backLine.Length - 1).PadRight(36, ' ');
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Send cmd")) { NavigateToMenu(MenuPage.SendCommand); }
        if (!selectedShip.IsTimedOut() && !selectedShip.IsCommandTimedOut()) menuItemCount++;
        if (selectedShip.IsCommandTimedOut()) text += "No answer received...";
        else text += SignalOrText(selectedShip, selectedShip.menuText);
    }
    else if (currentMenu == MenuPage.ShipInventory)
    {
        if (AddMenuItem(ref text, sel, idx++, applyAction, " Back")) { NavigateToMenu(MenuPage.Main); }
        text += thinSeparator;
        text += SignalOrText(selectedShip, GetShipInventoryText(selectedShip));
    }

    if (!menuJustNavigated) menuIndex = Math.Min(menuItemCount - 1, menuIndex);
    return text;
}

String GetShipStatusLine(ShipInfo ship)
{
    if (ship.version != VERSION) return ship.name + ": Different version!";
    return ship.name + ": " + SignalOrText(ship, ship.stateText) + " " + ship.distance + "m";
}

String GetShipCargoLine(ShipInfo ship)
{
    String s = ProgressBar("", SafeDiv(ship.currentVolume, ship.maxVolume) * 100f, 100f, 8, 0, 0) + "% ";
    for (int i = 0; i < ship.items.Count; i++)
    {
        if (i >= 5) break;
        ItemInfo item = ship.items[i];
        s += Truncate(Capitalize(GetItemShortName(item)), 3) + " " + FormatAmount(item.amount) + " ";
    }
    return s;
}

String GetItemShortName(ItemInfo item)
{
    if (item.type.ToUpper() == "ORE" || item.type.ToUpper() == "INGOT")
    {
        String code = GetElementCode(item.name.ToUpper());
        if (code != "") return code;
    }
    return item.name;
}

// Replaces the text with "No signal..." once the ship hasn't reported for a while.
String SignalOrText(ShipInfo ship, String text)
{
    if (ship.IsTimedOut()) return "No signal...(" + FormatTime((int)(TimeNow() - ship.lastMessageTime).TotalSeconds) + ")";
    return text;
}

String GetLCDModeHelp(bool withDefault)
{
    String text = "";
    if (withDefault) text += "mode=main:1\n\n";
    text += "//Available modes:\n" + "//main:<Page>\n" + "//mainX:<Page>  (no empty lines)\n" + "//menu\n" +
        "//inventory\n" + "//menu:<shipname>\n" + "//inventory:<shipname>";
    // (the check for outdated Custom Data looks for the text above: new modes only in the template)
    if (withDefault) text += "\n//fleet\n//ship:<shipname>\n//log\n//job:<number>\n//map  (wide LCD)";
    return text;
}

LCDMode ModeOf(IMyTerminalBlock panel) { LCDMode m; String s; ParseLCDMode(panel, out m, out s); return m; }

void ParseLCDMode(IMyTerminalBlock panel, out LCDMode mode, out String param)
{
    bool ok = true;
    String modeStr = ParseCustomDataValue(FindCustomDataLine(panel.CustomData.Split('\n'), "mode", ref ok)).ToUpper();
    String[] parts = modeStr.Split(':');
    String modeKey = parts.Length > 0 ? parts[0].Trim() : "";
    param = parts.Length > 1 ? parts[1].Trim() : "";
    mode = LCDMode.Unknown;
    if (modeKey == "MAIN") mode = LCDMode.MainPaged;
    else if (modeKey == "MAINX") mode = LCDMode.MainPagedCompact;
    else if (modeKey == "MENU") mode = LCDMode.Menu;
    else if (modeKey == "INVENTORY") mode = LCDMode.Inventory;
    else if (modeKey == "DEBUG") mode = LCDMode.Debug;
    else if (modeKey == "FLEET") mode = LCDMode.Fleet;
    else if (modeKey == "SHIP") mode = LCDMode.Ship;
    else if (modeKey == "LOG") mode = LCDMode.Log;
    else if (modeKey == "JOB") mode = LCDMode.Job;
    else if (modeKey == "MAP") mode = LCDMode.Map;
    if (mode == LCDMode.Unknown) param = modeStr;
}

// Renders one LCD. "main:<page>" pages share one scrolling list; firstPanel resets the page bookkeeping.
String BuildControllerScreenForMode(LCDMode mode, String param, bool firstPanel)
{
    int linesPerPage = 15;
    if (firstPanel)
    {
        controllerPageIdx1 = controllerPageCount1;
        controllerPageCount1 = 0;
        controllerPageIdx2 = controllerPageCount2;
        controllerPageCount2 = 0;
    }
    String text = "";
    if (mode == LCDMode.Fleet) return BuildFleetScreen();
    if (mode == LCDMode.Ship) return BuildShipScreen(param);
    if (mode == LCDMode.Log) return BuildLogScreen();
    if (mode == LCDMode.MainPaged)
    {
        int page = 0;
        text = BuildControllerScreen(false, controllerPageIdx1, ref controllerScrollState1, true, linesPerPage);
        if (!int.TryParse(param, out page)) return text;
        controllerPageCount1 = Math.Max(page, controllerPageCount1);
        page--;
        return PageText(page, linesPerPage, text);
    }
    else if (mode == LCDMode.MainPagedCompact)
    {
        int page = 0;
        text = BuildControllerScreen(false, controllerPageIdx2, ref controllerScrollState2, false, linesPerPage);
        if (!int.TryParse(param, out page)) return text;
        controllerPageCount2 = Math.Max(page, controllerPageCount2);
        page--;
        return PageText(page, linesPerPage, text);
    }
    if (mode == LCDMode.MainCompact)
    {
        return BuildControllerScreen(false, 0, ref controllerScrollState3, true, linesPerPage);
    }
    else if (mode == LCDMode.Inventory || mode == LCDMode.Menu || mode == LCDMode.Debug)
    {
        ShipInfo ship = selectedShip;
        if (param != "")
        {
            ship = FindShip("", param);
            if (ship == null) return "Unknown ship: " + param;
        }
        else
        {
            if (ship == null) return "No ship on main screen selected.";
            text = "Selected: ";
        }
        String separator = "—————————————————————\n";
        String title = "";
        String body = "";
        if (mode == LCDMode.Menu) { title = "Menu"; body = SignalOrText(ship, ship.menuText); }
        else if (mode == LCDMode.Inventory) { title = "Inventory"; body = SignalOrText(ship, GetShipInventoryText(ship)); }
        else if (mode == LCDMode.Debug) { return SignalOrText(ship, GetShipDebugText(ship)); }
        text += ship.name + " - " + ship.distance + "m | " + Capitalize("" + title) + "\n" + separator;
        text += body;
        return text;
    }
    return "Unknown command: " + param;
}

string GetShipDebugText(ShipInfo ship)
{
    String s = "";
    s += ship.version + "\n";
    s += ship.name + "\n";
    s += ship.position.X + "\n";
    s += ship.position.Y + "\n";
    s += ship.position.Z + "\n";
    s += ship.stateText + "\n";
    s += ship.statusMessage + "\n";
    s += ship.menuIndex + "\n";
    s += ship.menuText.Length + "\n";
    s += ship.currentVolume + "\n";
    s += ship.maxVolume + "\n";
    s += ship.items.Count() + "\n";
    return s;
}

string GetShipInventoryText(ShipInfo ship)
{
    String s = "";
    s += ProgressBar("All", SafeDiv(ship.currentVolume, ship.maxVolume) * 100f, 100, 30, 8, 12) + "%\n";
    s += "\n";
    for (int i = 0; i < ship.items.Count(); i++)
    {
        ItemInfo item = ship.items[i];
        s += ProgressBar(item.name, item.amount, ship.totalItems, 30, 8, 10) + "\n";
    }
    return s;
}

// Combined command state for one ship, or for all ships when ship == null.
CommandState GetCommandState(ShipInfo ship, bool clear, string cmd)
{
    CommandState result = CommandState.None;
    if (ship == null)
    {
        for (int i = 0; i < connectedShips.Count; i++)
        {
            CommandState state = connectedShips[i].ProcessCommand(cmd, false, clear, false);
            if (state == CommandState.Received) result = state;
            if (state == CommandState.NoAnswer) return state;
            if (state == CommandState.Pending) return state;
        }
        return result;
    }
    else return ship.ProcessCommand(cmd, false, clear, false);
}

String GetCommandStatusText(ShipInfo ship, String cmd)
{
    if (!ShowCmdResult(ship, cmd)) return ""; // sent from another page, to another ship, or long ago
    CommandState state = GetCommandState(ship, false, cmd);
    if (state == CommandState.Received) return "received!";
    if (state == CommandState.Pending) return "pending...";
    if (state == CommandState.NoAnswer) return "no answer!";
    return "";
}
