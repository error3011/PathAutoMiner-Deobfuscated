// ========================== CONTROLLER SCREENS ==========================
// Extra LCD modes (Custom Data "mode=..."):
//   fleet          one line per ship: state, cargo, energy, hole, flags; docking area and jobs
//   ship:<name>    one ship in detail
//   log            coordination events (docking, holes, giving way, emergencies, contact), newest first,
//                  coloured by kind (drawn; text size = the LCD's font size)
//   job:<n>        hole matrix of job n (drawn, as seen behind a ship drilling it)
//   map[:side|top|dock|all]  side view / top view / docking area / all three (drawn, see the maps below)
//   docks          the station's connectors: free / cleared / docking / docked / BLOCKED lane / damaged
// The drawn ones redraw about once a second.

List<String> logLines = new List<String>();
List<DateTime> logTimes = new List<DateTime>();
Dictionary<long, String> shipPaths = new Dictionary<long, String>(); // grid id -> recorded path "x|y|z:..."
IMyBroadcastListener pathListener = null;
Vector3 planetCenter = new Vector3();
bool hasPlanet = false;
int screenTick = 0;

void AddLog(String msg)
{
    // the same event again within 30 s (a conflict that comes and goes): once is enough
    for (int i = 0; i < logLines.Count && i < 10; i++) if (logLines[i].EndsWith(msg) && (TimeNow() - logTimes[i]).TotalSeconds < 30) return;
    logLines.Insert(0, DateTime.Now.ToString("HH:mm:ss ") + msg);
    logTimes.Insert(0, TimeNow());
    if (logLines.Count > 40) { logLines.RemoveAt(40); logTimes.RemoveAt(40); }
}

String ShipName(long gridId)
{
    TCShip t = tcShips.Find(q => q.id == gridId);
    if (t != null) return TCName(t);
    String id = gridId.ToString();
    return "Ship " + id.Substring(Math.Max(0, id.Length - 4));
}

void UpdateScreens()
{
    if (pathListener == null) pathListener = IGC.RegisterBroadcastListener("[PAMPTH]");
    int budget = 10;
    while (pathListener.HasPendingMessage && budget-- > 0)
    {
        String data = pathListener.AcceptMessage().Data as String;
        if (data == null) continue;
        String[] f = data.Split(';');
        long id;
        if (f.Length < 2 || !long.TryParse(f[0], out id)) continue;
        shipPaths[id] = f[1];
        if (f.Length >= 3 && f[2] != "") { planetCenter = StringToVector(f[2]); hasPlanet = true; } // id;path;planet centre
    }
}

bool SpriteMode(LCDMode m) { return m == LCDMode.Map || m == LCDMode.Job || m == LCDMode.Log; }

String ShortState(String s)
{
    return s.Replace("Flying to ", "To ").Replace(" position", " pos").Replace("Returning to dock", "Going home").Replace("Waiting for ", "Wait ")
        .Replace("Approaching dock", "Approach").Replace("Charging batteries", "Charging");
}

String Pad(String s, int n) { return s.Length >= n ? s.Substring(0, n) : s + new String(' ', n - s.Length); }

String Pct(int v) { return v < 0 ? "  - " : (v + "%").PadLeft(4); }

// flags for the fleet list / ship page
String ShipFlags(TCShip t)
{
    if (t == null) return "no traffic";
    String f = "";
    if (t.emg) f += "EMG ";
    if (t.flag == 2) f += "PARKED ";
    else if (t.flag == 1) f += "QUEUE ";
    if (t.prio < 0) f += "WAIT ";
    if (t.limit >= 0) f += "GIVE ";
    if (clearedShip == t.id || dockingShips.Contains(t.id)) f += "CLEARED ";
    return f.Trim();
}

// this PB's run time (server performance)
String ExecLine() { return "Run time: avg " + Math.Round(execAvg, 3) + " ms, max " + Math.Round(execPeak, 2) + " ms (10 s)\n"; }

String BuildFleetScreen()
{
    String text = "PAM fleet | " + connectedShips.Count + " ships | controller " + Math.Round(execAvg, 2) + " ms (max " + Math.Round(execPeak, 2) + ")\n" + GetDockAreaLine() + GetJobLines() + GetTrafficControlLine();
    int nw = 6; // name column: as wide as the longest name (max 20 characters)
    for (int i = 0; i < connectedShips.Count; i++) nw = Math.Min(21, Math.Max(nw, connectedShips[i].name.Length + 1));
    text += Pad("Ship", nw) + Pad("State", 15) + " Crg  Bat  H2  Hole   ms  Flags\n";
    for (int i = 0; i < connectedShips.Count; i++)
    {
        ShipInfo s = connectedShips[i];
        TCShip t = FindTCShip(s);
        String hole = "  -";
        if (t != null && t.hole >= 0)
        {
            JobRec job = jobs.Find(q => q.key == t.job);
            hole = ("" + (job != null ? job.order.IndexOf(t.hole) + 1 : t.hole + 1)).PadLeft(3);
        }
        text += Pad(s.name, nw - 1) + " " + Pad(s.IsTimedOut() ? "NO CONTACT" : ShortState(t != null && t.nav >= 0 ? GetNavStateName((NavState)t.nav) : s.stateText), 14) + " " +
            Pct((int)(SafeDiv(s.currentVolume, s.maxVolume) * 100)) + " " + Pct(t != null ? t.bat : -1) + " " + Pct(t != null ? t.h2 : -1) + " " + hole + " " + (t != null && t.ms >= 0 ? t.ms.ToString("0.00") : "   -").PadLeft(5) + "  " + ShipFlags(t) + "\n";
    }
    return text;
}

String BuildShipScreen(String name)
{
    ShipInfo s = FindShip("", name);
    if (s == null) return "Unknown ship: " + name;
    TCShip t = FindTCShip(s);
    String text = s.name + " | " + s.distance + " m from here" + (s.IsTimedOut() ? " | NO CONTACT" : "") + "\n" + "—————————————————————\n";
    text += "Job:    " + s.stateText + "\n" + "Status: " + s.statusMessage + "\n";
    text += ProgressBar("Cargo", SafeDiv(s.currentVolume, s.maxVolume) * 100f, 100, 20, 8, 7) + "%\n";
    if (t == null) return text + "(no traffic data: traffic off or out of range)\n";
    if (t.nav >= 0) text += "Doing:  " + GetNavStateName((NavState)t.nav) + "\n";
    text += "Battery " + Pct(t.bat).Trim() + "  H2 " + Pct(t.h2).Trim() + "  U " + t.ur + " kg\n";
    text += "Speed " + Math.Round(t.vel.Length(), 1) + " m/s" + (t.ms >= 0 ? "  Script " + t.ms.ToString("0.00") + " ms" : "") + "\n";
    JobRec job = jobs.Find(q => q.key == t.job);
    if (job != null)
    {
        int n = 0;
        for (int i = 0; i < job.done.Length; i++) if (job.done[i] == '1') n++;
        text += "Job " + (jobs.IndexOf(job) + 1) + ": " + n + "/" + job.done.Length + " done" + (t.hole >= 0 ? ", hole " + (job.order.IndexOf(t.hole) + 1) : "") + "\n";
    }
    text += "Right of way: " + Pick("waiting|normal|between holes|heading home|docking / emergency", t.prio + 1) + (t.metric >= 0 ? ", " + t.metric + " m to go" : "") + "\n";
    text += "Docking: " + Pick("-|to the docking area|last leg|docked / undocking", t.phase) +
        (clearedShip == t.id ? ", cleared" : dockingShips.Contains(t.id) ? ", cleared (on its last leg)" : "") + "\n";
    String f = ShipFlags(t);
    text += "Flags: " + (f == "" ? "-" : f) + "\n";
    text += "Last contact: " + Math.Round((TimeNow() - t.time).TotalSeconds, 1) + " s\n";
    return text;
}

// ---- text screens drawn in colour (main, menus, inventory, fleet, ship) ----
Dictionary<IMyTextSurface, String> screenCache = new Dictionary<IMyTextSurface, String>();

// connectors of the station (pool + ships' homes): state, ship, lane
String BuildDocksScreen()
{
    String text = "Connectors" + (dockAreaStatus != "" ? " | " + dockAreaStatus : "") + "\n";
    List<IMyShipConnector> list = DockList();
    if (list.Count == 0) return text + "(no [PAM-Dock] connectors, no ship asked yet)\n";
    foreach (IMyShipConnector c in list)
    {
        bool cam = laneCameras.Exists(q => Vector3D.Distance(q.GetPosition(), c.GetPosition()) <= 10);
        text += c.CustomName + (homeConns.Contains(c.EntityId) ? " (home)" : "") + ": " + DockSt(c) + (cam ? "" : " | no lane camera") + "\n";
    }
    return text;
}

String GridName(IMyCubeGrid g)
{
    TCShip t = tcShips.Find(q => q.id == g.EntityId);
    return t != null ? TCName(t) : g.CustomName;
}

// colour of a text line by what it says (default: the LCD's font colour)
Color LineColor(String l, LCDMode mode, Color def)
{
    String t = l.Trim();
    if (mode == LCDMode.Docks)
    {
        if (l.Contains(": BLOCKED")) return Color.Red;
        if (l.Contains(": DAMAGED") || l.Contains(": OFF")) return Color.Coral;
        if (l.Contains(": docked")) return Color.Gray;
        if (l.Contains(": cleared") || l.Contains(": docking") || l.Contains(": ready")) return Color.SkyBlue;
        if (l.Contains(": free")) return Color.LimeGreen;
    }
    if (t.StartsWith("—") || t.StartsWith("---")) return Color.Gray;
    if (mode == LCDMode.Fleet && t.StartsWith("Ship ")) return Color.Gray; // table header
    if (l.Contains("EMERGENCY") || l.Contains(" EMG") || l.Contains("no answer") || l.Contains("NO CONTACT") || l.Contains("No signal") || l.Contains("Different version")) return Color.Red;
    if (t.StartsWith("Battery"))
    {
        int b = -1;
        int.TryParse(new String(t.Substring(7).Trim().TakeWhile(char.IsDigit).ToArray()), out b);
        return b < 0 ? def : b >= 50 ? Color.LimeGreen : b >= 20 ? Color.Yellow : Color.Red;
    }
    if (l.Contains(" GIVE") || l.Contains("giving way") || l.Contains("pending") || t.StartsWith("Traffic:")) return Color.Yellow;
    if (l.Contains(" WAIT") || l.Contains("waiting to leave")) return Color.Violet;
    if (l.Contains("PARKED")) return Color.Gray;
    if (l.Contains("CLEARED") || l.Contains("received") || l.Contains("Job active")) return Color.LimeGreen;
    if (l.Contains("QUEUE") || t.StartsWith("Dock")) return Color.SkyBlue;
    if (t.StartsWith("Job")) return Color.Orange;
    if (t.StartsWith("[") || t.Contains("[|")) return new Color(200, 160, 80); // cargo / inventory bars
    return def;
}

// a text screen as coloured lines; the LCD's font, size, padding and colours; the selected menu line
// highlighted; redrawn only when something changed
void DrawTextScreen(IMyTextSurface p, String text, LCDMode mode, int lines)
{
    String font = mode == LCDMode.Fleet && !lcdFontSet ? "Monospace" : p.Font; // table columns (unless font= is set)
    String key = text + p.FontSize + font + p.FontColor + p.BackgroundColor + p.TextPadding, old;
    if (screenCache.TryGetValue(p, out old) && old == key && p.ContentType == ContentType.SCRIPT) return;
    screenCache[p] = key;
    if (p.ContentType != ContentType.SCRIPT) { p.ContentType = ContentType.SCRIPT; p.Script = ""; }
    p.ScriptBackgroundColor = p.BackgroundColor;
    Vector2 size = p.SurfaceSize, at = (p.TextureSize - size) / 2;
    float sc = Math.Max(0.1f, p.FontSize), pad = size.Y * p.TextPadding / 100f;
    float lh = Math.Min(p.MeasureStringInPixels(new StringBuilder("Ag"), font, sc).Y, (size.Y - 2 * pad) / Math.Max(1, lines));
    drawFrame = p.DrawFrame();
    String[] ls = text.Split('\n');
    float y = at.Y + pad;
    for (int i = 0; i < ls.Length && y + lh <= at.Y + size.Y - pad + 1; i++, y += lh)
    {
        if (ls[i].StartsWith(">")) DBox(new Vector2(at.X + size.X / 2, y + lh / 2), new Vector2(size.X, lh), new Color(25, 45, 80), "SquareSimple");
        DText(ls[i], new Vector2(at.X + pad, y), LineColor(ls[i], mode, p.FontColor), sc, TextAlignment.LEFT, font);
    }
    drawFrame.Dispose();
}

// colour of a log event by its kind
Color LogColor(String msg)
{
    if (msg.StartsWith("EMERGENCY")) return Color.Red;
    if (msg.Contains("couldn't lock") || msg.Contains("too slow")) return Color.Coral;
    if (msg.StartsWith("Dock:")) return Color.SkyBlue;
    if (msg.StartsWith("Job:")) return Color.Orange;
    if (msg.StartsWith("Traffic:")) return Color.Yellow;
    if (msg.StartsWith("Move home:")) return Color.Violet;
    if (msg.StartsWith("Contact lost")) return Color.IndianRed;
    if (msg.StartsWith("Contact")) return Color.LimeGreen;
    return Color.White;
}

// log: time in grey, the event in its colour, long events wrapped; text size = the LCD's font size
void DrawLog(IMyTextPanel p, Vector2 at, Vector2 size)
{
    float sc = Math.Max(0.3f, p.FontSize), pad = 6, bottom = at.Y + size.Y;
    StringBuilder sb = new StringBuilder();
    String fn = p.Font;
    Func<String, float> W = s => p.MeasureStringInPixels(sb.Clear().Append(s), fn, sc).X;
    float lh = p.MeasureStringInPixels(sb.Clear().Append("Ag"), fn, sc).Y, y = at.Y + pad;
    DText("Coordination log (newest first)", new Vector2(at.X + pad, y), Color.White, sc, TextAlignment.LEFT, fn);
    y += lh * 1.3f;
    if (logLines.Count == 0) DText("(nothing yet)", new Vector2(at.X + pad, y), Color.Gray, sc, TextAlignment.LEFT, fn);
    for (int i = 0; i < logLines.Count && y + lh <= bottom; i++)
    {
        String time = logLines[i].Substring(0, 9), msg = logLines[i].Substring(9), line = "";
        Color c = LogColor(msg);
        DText(time, new Vector2(at.X + pad, y), Color.Gray, sc, TextAlignment.LEFT, fn);
        float x = at.X + pad + W(time), width = at.X + size.X - pad - x;
        foreach (String w in msg.Split(' '))
        {
            String t = line == "" ? w : line + " " + w;
            if (line == "" || W(t) <= width) { line = t; continue; }
            DText(line, new Vector2(x, y), c, sc, TextAlignment.LEFT, fn);
            y += lh;
            line = w;
            if (y + lh > bottom) { line = ""; break; }
        }
        if (line != "") DText(line, new Vector2(x, y), c, sc, TextAlignment.LEFT, fn);
        y += lh;
    }
}

// ---- drawing ----
MySpriteDrawFrame drawFrame;

void DLine(Vector2 a, Vector2 b, Color c, float w)
{
    Vector2 d = b - a;
    float len = d.Length();
    if (len < 0.5f) return;
    drawFrame.Add(new MySprite(SpriteType.TEXTURE, "SquareSimple", (a + b) / 2, new Vector2(len, w), c, null, TextAlignment.CENTER, (float)Math.Atan2(d.Y, d.X)));
}

void DBox(Vector2 center, Vector2 size, Color c, String shape) { drawFrame.Add(new MySprite(SpriteType.TEXTURE, shape, center, size, c)); }

// text sprite; without a font given (map, job): the LCD's font, sizes scaled by its font size
float drawScale = 1;
String drawFont = "Debug";
void DText(String s, Vector2 at, Color c, float scale, TextAlignment align, String font = null)
{
    MySprite t = MySprite.CreateText(s, font ?? drawFont, c, font == null ? scale * drawScale : scale, align);
    t.Position = at;
    drawFrame.Add(t);
}

void DrawSpriteScreen(IMyTextPanel p, LCDMode mode, String param)
{
    // log and job: only when something changed (the drawing stays on the LCD until the next one)
    String key = null;
    if (mode == LCDMode.Log) key = "L" + logLines.Count + (logLines.Count > 0 ? logLines[0] : "");
    if (mode == LCDMode.Job)
    {
        int n = 1;
        int.TryParse(param, out n);
        JobRec jr = n >= 1 && n <= jobs.Count ? jobs[n - 1] : null;
        key = "J" + param + (jr == null ? "" : jr.done + jr.ships.Count + String.Join(",", jr.ships.Select(q => q.cell)));
    }
    if (key != null)
    {
        key += p.FontSize + p.Font + p.BackgroundColor + p.SurfaceSize;
        String old;
        if (screenCache.TryGetValue(p, out old) && old == key && p.ContentType == ContentType.SCRIPT) return;
        screenCache[p] = key;
    }
    if (p.ContentType != ContentType.SCRIPT) { p.ContentType = ContentType.SCRIPT; p.Script = ""; }
    p.ScriptBackgroundColor = new Color(0, 0, 0);
    Vector2 size = p.SurfaceSize, at = (p.TextureSize - size) / 2;
    drawScale = p.FontSize; drawFont = p.Font;
    drawFrame = p.DrawFrame();
    if (mode == LCDMode.Job) DrawJobMatrix(param, at, size);
    else if (mode == LCDMode.Log) DrawLog(p, at, size);
    else { drawFrameSize = size; DrawMap(param, at, size); }
    drawFrame.Dispose();
}

// hole matrix: columns left to right, rows top to bottom, as seen from behind a ship drilling it
void DrawJobMatrix(String param, Vector2 at, Vector2 size)
{
    int n = 1;
    int.TryParse(param, out n);
    if (n < 1 || n > jobs.Count) { DText(jobs.Count == 0 ? "No job" : "Job " + param + ": no such job (" + jobs.Count + ")", at + size / 2, Color.White, 1, TextAlignment.CENTER); return; }
    JobRec job = jobs[n - 1];
    int done = 0;
    for (int i = 0; i < job.done.Length; i++) if (job.done[i] == '1') done++;
    DText("Job " + n + ": " + done + "/" + job.done.Length + " holes done", at + new Vector2(size.X / 2, 4), Color.White, 0.9f, TextAlignment.CENTER);
    int keyLines = (job.ships.Count + 1) / 2; // key under the grid: 2 ships per line
    float top = 40, bottom = 30 + 20 * keyLines;
    float cell = Math.Min(size.X / job.w, (size.Y - top - bottom) / job.h);
    Vector2 origin = at + new Vector2((size.X - cell * job.w) / 2, top + (size.Y - top - bottom - cell * job.h) / 2);
    for (int c = 0; c < job.done.Length; c++)
    {
        Vector2 ctr = origin + new Vector2((c % job.w + 0.5f) * cell, (c / job.w + 0.5f) * cell);
        JobShip owner = HoleOwner(job, c, null);
        Color col = job.done[c] == '1' ? new Color(90, 90, 90) : owner != null ? Color.Orange : new Color(30, 60, 90);
        DBox(ctr, new Vector2(cell * 0.9f, cell * 0.9f), col, "SquareSimple");
        if (owner == null) continue;
        DText("" + (job.ships.IndexOf(owner) + 1), ctr - new Vector2(0, cell * 0.3f), Color.Black, Math.Min(1.2f, cell / 40), TextAlignment.CENTER);
    }
    // key: the number in a square = the ship working it
    for (int i = 0; i < job.ships.Count; i += 2)
        DText((i + 1) + " " + ShipName(job.ships[i].id) + (i + 1 < job.ships.Count ? "    " + (i + 2) + " " + ShipName(job.ships[i + 1].id) : ""),
            at + new Vector2(size.X / 2, size.Y - 26 - 20 * (keyLines - i / 2)), Color.Orange, 0.55f, TextAlignment.CENTER);
    DText("grey done | orange: ship working (number) | blue free", at + new Vector2(size.X / 2, size.Y - 24), Color.Gray, 0.6f, TextAlignment.CENTER);
}

// ---- maps (mode=map:<view>) ----
//   map / map:side  side view station -> job: real heights, labelled in metres (stretched at most 5x)
//   map:top         top view, the way to the first job pointing up, scale bar; heights as labels
//   map:dock        the docking area zoomed in: connectors (numbered, coloured by state), their lanes,
//                   [PAM-Zone] boxes, blocked lanes, ships near the docks, a line from a cleared ship to its connector
//   map:all         all three on one screen, for a big LCD (3x3, 5x5)
// Markers and text scale with the view (its smaller side / 512 px) times the LCD's font size.
Vector3 mO, mUp, mAlong, mRight;
Vector2 mAt, mSize;
float mK = 1, mFont = 1;
List<List<Vector3>> mPaths = new List<List<Vector3>>();
String mPathKey = "";

void DrawMap(String view, Vector2 at, Vector2 size)
{
    mFont = drawScale;
    // frame of all views: station, up (planet: away from its centre), the way to the first job
    mO = Me.GetPosition();
    mUp = new Vector3();
    if (hasPlanet) mUp = mO - planetCenter;
    else foreach (TCShip t in tcShips) mUp += t.up;
    if (mUp.LengthSquared() < 0.01f) mUp = Me.WorldMatrix.Up;
    mUp.Normalize();
    // every recorded path once (ships sharing a path: drawn once), parsed again only when one changed
    List<String> ps = shipPaths.Values.Distinct().ToList();
    String key = String.Join(";", ps);
    if (key != mPathKey)
    {
        mPathKey = key; mPaths.Clear();
        foreach (String p in ps) { List<Vector3> l = new List<Vector3>(); foreach (String w in p.Split(':')) if (w != "") l.Add(StringToVector(w)); mPaths.Add(l); }
    }
    // towards the first job (else the path end farthest from the station, else the first ship)
    Vector3 far = jobs.Count > 0 ? JobPos(jobs[0]) : mO;
    if (far == mO) foreach (List<Vector3> l in mPaths) foreach (Vector3 v in l) if (Vector3.Distance(v, mO) > Vector3.Distance(far, mO)) far = v;
    if (far == mO && tcShips.Count > 0) far = tcShips[0].pos;
    mAlong = far - mO - mUp * Vector3.Dot(far - mO, mUp);
    if (mAlong.LengthSquared() < 1) mAlong = Vector3.Cross(mUp, Me.WorldMatrix.Right);
    mAlong.Normalize();
    mRight = Vector3.Cross(mAlong, mUp);

    if (view == "ALL")
    {
        // wide: top view left, docks and side view right; square (3x3, 5x5): top view and docks above, side view below
        float w = size.X, h = size.Y * 0.93f;
        bool wide = w > h * 1.4f;
        Vector2 a = wide ? new Vector2(w / 2, h) : new Vector2(w * 0.62f, h * 0.66f);
        MapView("TOP", at, a);
        MapView("DOCK", at + new Vector2(a.X, 0), wide ? new Vector2(w / 2, h / 2) : new Vector2(w - a.X, a.Y));
        MapView("SIDE", at + (wide ? new Vector2(w / 2, h / 2) : new Vector2(0, a.Y)), wide ? new Vector2(w / 2, h / 2) : new Vector2(w, h - a.Y));
        mK = Math.Min(size.X, size.Y) / 1024; drawScale = mFont * mK;
        MapKey(at + new Vector2(size.X / 2, h + 22 * mK), true);
    }
    else MapView(view == "TOP" || view == "DOCK" ? view : "SIDE", at, size);
}

void MapView(String view, Vector2 at, Vector2 size)
{
    mAt = at; mSize = size;
    mK = Math.Min(size.X, size.Y) / 512; drawScale = mFont * mK;
    bool all = size != drawFrameSize;
    if (all) // frame around each view of map:all
    {
        Vector2 b = at + size;
        Color f = new Color(60, 60, 60);
        DLine(at, new Vector2(b.X, at.Y), f, 2); DLine(new Vector2(b.X, at.Y), b, f, 2); DLine(b, new Vector2(at.X, b.Y), f, 2); DLine(new Vector2(at.X, b.Y), at, f, 2);
    }
    if (view == "DOCK") DrawDockMap(at, size);
    else if (view == "TOP") DrawTopMap(at, size);
    else DrawSideMap(at, size);
    if (!all) MapKey(at + new Vector2(size.X / 2, size.Y - 22 * mK), view == "DOCK");
}

Vector2 drawFrameSize;

// colour key at the bottom of a view
void MapKey(Vector2 at, bool docks)
{
    if (docks) at.Y -= 18 * mK;
    DText("green moving | yellow giving way | violet waiting | grey parked | red emergency", at, Color.Gray, 0.5f, TextAlignment.CENTER);
    if (docks) DText("docks: green free | grey docked | sky blue cleared, docking | red lane blocked | coral damaged", at + new Vector2(0, 18 * mK), Color.Gray, 0.5f, TextAlignment.CENTER);
}

float Ht(Vector3 v) { return hasPlanet ? Vector3.Distance(v, planetCenter) - Vector3.Distance(mO, planetCenter) : Vector3.Dot(v - mO, mUp); }
String HtS(Vector3 v) { float h = Ht(v); return (h >= 0 ? "+" : "") + Math.Round(h) + " m"; }

Color ShipCol(TCShip t) { return t.emg ? Color.Red : t.limit >= 0 ? Color.Yellow : t.prio < 0 ? Color.Violet : TCParked(t) ? Color.Gray : Color.Lime; }

// the connector a ship is cleared to / docking at (null: none)
IMyShipConnector ShipDock(long id)
{
    int d = dockingShips.IndexOf(id);
    return StationConnector(id == clearedShip && clearedKind == 1 ? clearedConn : d >= 0 ? dockingConns[d] : 0);
}

// a ship: arrow the way it flies (or wants to), a line to its connector when cleared (else where it is
// in 5 s), its name and height
void DShip(TCShip t, Func<Vector3, Vector2> P)
{
    Vector2 q = P(t.pos), d = P(t.pos + (t.vel.LengthSquared() > 0.25f ? t.vel : t.heading)) - q;
    if (q.X < mAt.X || q.Y < mAt.Y || q.X > mAt.X + mSize.X || q.Y > mAt.Y + mSize.Y) return;
    Color c = ShipCol(t);
    IMyShipConnector sc = ShipDock(t.id);
    if (sc != null) DLine(q, P(sc.GetPosition()), Color.SkyBlue, 2 * mK);
    else if (t.vel.LengthSquared() > 0.25f) DLine(q, P(t.pos + t.vel * 5), c, 2 * mK);
    if (d.LengthSquared() > 0.01f) drawFrame.Add(new MySprite(SpriteType.TEXTURE, "Triangle", q, new Vector2(14, 16) * mK, c, null, TextAlignment.CENTER, (float)Math.Atan2(d.X, -d.Y)));
    else DBox(q, new Vector2(12, 12) * mK, c, "Circle");
    DText(TCName(t) + " " + HtS(t.pos), q - new Vector2(0, 28 * mK), c, 0.55f, TextAlignment.CENTER);
}

// recorded paths (fewer segments when they'd be shorter than ~3 px)
void DPaths(Func<Vector3, Vector2> P)
{
    foreach (List<Vector3> l in mPaths)
    {
        Vector2 last = new Vector2();
        for (int i = 0; i < l.Count; i++)
        {
            Vector2 q = P(l[i]);
            if (i > 0 && (q - last).Length() < 3 && i < l.Count - 1) continue;
            if (i > 0) DLine(last, q, Color.SkyBlue, 2 * mK);
            last = q;
        }
    }
}

// the station's connectors (pool + ships' homes)
List<IMyShipConnector> DockList()
{
    List<IMyShipConnector> list = new List<IMyShipConnector>(stationDocks);
    foreach (long h in homeConns) { IMyShipConnector hc = StationConnector(h); if (hc != null && !list.Contains(hc)) list.Add(hc); }
    return list;
}

String DockSt(IMyShipConnector c)
{
    LaneBlock b = LaneBlockOf(c.EntityId);
    int d = dockingConns.IndexOf(c.EntityId);
    return b != null ? "BLOCKED: " + b.what + " in its lane" : !c.IsFunctional ? "DAMAGED" : !c.Enabled ? "OFF" :
        c.Status == MyShipConnectorStatus.Connected ? "docked: " + GridName(c.OtherConnector.CubeGrid) :
        c.Status == MyShipConnectorStatus.Connectable ? "ready to lock" :
        clearedShip != 0 && clearedKind == 1 && clearedConn == c.EntityId ? "cleared: " + ShipName(clearedShip) :
        d >= 0 ? "docking: " + ShipName(dockingShips[d]) : "free";
}

Color DockCol(IMyShipConnector c) { return LineColor(": " + DockSt(c), LCDMode.Docks, Color.White); }

// grid lines / scale bar step: 1, 2, 5 x 10^n m, about n of them over range metres
float MapStep(float range, int n) { float s = 1; while (range / s > n) s *= s.ToString().StartsWith("2") ? 2.5f : 2; return s; }

// side view: horizontal = distance from the station towards the job, vertical = real height above the
// station (on a planet: distance from its centre, so the curve of the planet doesn't tilt it)
void DrawSideMap(Vector2 at, Vector2 size)
{
    float x0 = 0, x1 = 20, y0 = 0, y1 = 10;
    List<Vector3> all = new List<Vector3>();
    foreach (List<Vector3> l in mPaths) all.AddRange(l);
    foreach (JobRec j in jobs) all.Add(JobPos(j));
    foreach (TCShip t in tcShips) all.Add(t.pos);
    foreach (Vector3 v in all)
    {
        float x = Vector3.Dot(v - mO, mAlong), y = Ht(v);
        x0 = Math.Min(x0, x); x1 = Math.Max(x1, x); y0 = Math.Min(y0, y); y1 = Math.Max(y1, y);
    }
    float mx = (x1 - x0) * 0.06f + 5, my = (y1 - y0) * 0.15f + 5;
    x0 -= mx; x1 += mx; y0 -= my; y1 += my;
    float top = 30 * mK, bottom = 40 * mK, left = 52 * mK;
    Vector2 area = new Vector2(size.X - left, size.Y - top - bottom);
    float sx = area.X / (x1 - x0), sy = Math.Min(area.Y / (y1 - y0), sx * 5), yMid = (y0 + y1) / 2;
    Func<float, float, Vector2> P2 = (x, y) => at + new Vector2(left + (x - x0) * sx, top + area.Y / 2 - (y - yMid) * sy);
    Func<Vector3, Vector2> P = v => P2(Vector3.Dot(v - mO, mAlong), Ht(v));

    DText("Side view: station -> job | heights x" + Math.Round(sy / sx, 1), at + new Vector2(size.X / 2, 4 * mK), Color.White, 0.7f, TextAlignment.CENTER);
    // height lines every 1/2/5 x 10^n m, labelled (height above the station)
    float range = area.Y / sy, step = MapStep(range, 6);
    for (float h = (float)Math.Ceiling((yMid - range / 2) / step) * step; h <= yMid + range / 2; h += step)
    {
        Vector2 a = P2(x0, h);
        if (a.Y < at.Y + top || a.Y > at.Y + size.Y - bottom) continue;
        DLine(a, P2(x1, h), h == 0 ? new Color(70, 70, 70) : new Color(35, 35, 35), mK);
        DText((h > 0 ? "+" : "") + h + " m", new Vector2(at.X + 2, a.Y - 9 * mK), Color.Gray, 0.5f, TextAlignment.LEFT);
    }
    DPaths(P);
    // station: its connectors coloured by state (above each other at their heights)
    DText("Station", P(mO) + new Vector2(0, 10 * mK), Color.White, 0.6f, TextAlignment.CENTER);
    List<IMyShipConnector> docks = DockList();
    if (docks.Count == 0) DBox(P(mO), new Vector2(14, 14) * mK, Color.White, "SquareSimple");
    foreach (IMyShipConnector c in docks) DBox(P(c.GetPosition()), new Vector2(8, 8) * mK, DockCol(c), "SquareSimple");
    for (int j = 0; j < jobs.Count; j++)
    {
        Vector3 jp = JobPos(jobs[j]);
        float half = jobs[j].w * jobs[j].tw / 2;
        DLine(P(jp) - new Vector2(half * sx, 0), P(jp) + new Vector2(half * sx, 0), Color.Orange, 4 * mK);
        DText(JobLabel(j) + " " + HtS(jp), P(jp) + new Vector2(0, 6 * mK), Color.Orange, 0.6f, TextAlignment.CENTER);
    }
    foreach (TCShip t in tcShips) DShip(t, P);
}

String JobLabel(int j)
{
    int n = 0;
    foreach (char c in jobs[j].done) if (c == '1') n++;
    return "Job " + (j + 1) + " " + n + "/" + jobs[j].done.Length;
}

// a top-down view of the points (equal scale both ways, the way to the job up); returns the projection
Func<Vector3, Vector2> TopFrame(List<Vector3> pts, Vector2 at, Vector2 size, float top, float bottom, float minRange, out float s)
{
    float x0 = 0, x1 = 0, y0 = 0, y1 = 0;
    foreach (Vector3 v in pts)
    {
        float x = Vector3.Dot(v - mO, mRight), y = Vector3.Dot(v - mO, mAlong);
        x0 = Math.Min(x0, x); x1 = Math.Max(x1, x); y0 = Math.Min(y0, y); y1 = Math.Max(y1, y);
    }
    float m = Math.Max(Math.Max(x1 - x0, y1 - y0) * 0.08f, minRange), cx = (x0 + x1) / 2, cy = (y0 + y1) / 2;
    Vector2 area = new Vector2(size.X, size.Y - top - bottom), ctr = at + new Vector2(size.X / 2, top + area.Y / 2);
    float sc = Math.Min(area.X / (x1 - x0 + 2 * m), area.Y / (y1 - y0 + 2 * m));
    s = sc;
    return v => ctr + new Vector2(Vector3.Dot(v - mO, mRight) - cx, cy - Vector3.Dot(v - mO, mAlong)) * sc;
}

// scale bar (top right of a view)
void ScaleBar(Vector2 at, Vector2 size, float s)
{
    float st = MapStep(size.X / 4 / s, 1);
    Vector2 b = at + new Vector2(size.X - 10 * mK, 14 * mK), a = b - new Vector2(st * s, 0);
    DLine(a, b, Color.Gray, 2 * mK);
    DText(st >= 1000 ? st / 1000 + " km" : st + " m", a - new Vector2(4 * mK, 9 * mK), Color.Gray, 0.5f, TextAlignment.RIGHT);
}

// top view: path, job sites (their size, with progress and height), station connectors, ships
void DrawTopMap(Vector2 at, Vector2 size)
{
    List<Vector3> pts = new List<Vector3>();
    foreach (List<Vector3> l in mPaths) pts.AddRange(l);
    foreach (JobRec j in jobs) pts.Add(JobPos(j));
    foreach (TCShip t in tcShips) pts.Add(t.pos);
    float s;
    Func<Vector3, Vector2> P = TopFrame(pts, at, size, 30 * mK, 30 * mK, 10, out s);
    DText("Top view: station -> job (up)", at + new Vector2(10 * mK, 4 * mK), Color.White, 0.7f, TextAlignment.LEFT);
    ScaleBar(at, size, s);
    DPaths(P);
    for (int j = 0; j < jobs.Count; j++)
    {
        Vector2 q = P(JobPos(jobs[j]));
        DBox(q, Vector2.Max(new Vector2(jobs[j].w * jobs[j].tw, jobs[j].h * jobs[j].th) * s, new Vector2(10, 10) * mK), Color.Orange, "SquareHollow");
        DText(JobLabel(j) + " " + HtS(JobPos(jobs[j])), q + new Vector2(0, 10 * mK), Color.Orange, 0.6f, TextAlignment.CENTER);
    }
    List<IMyShipConnector> docks = DockList();
    DBox(P(mO), new Vector2(16, 16) * mK, Color.White, "SquareHollow");
    foreach (IMyShipConnector c in docks) DBox(P(c.GetPosition()), new Vector2(6, 6) * mK, DockCol(c), "SquareSimple");
    DText("Station", P(mO) + new Vector2(0, 10 * mK), Color.White, 0.6f, TextAlignment.CENTER);
    foreach (TCShip t in tcShips) DShip(t, P);
}

// docking area zoomed in: [PAM-Zone] boxes, connectors with their lanes (dockLaneLength), blocked
// lanes from the camera to what is in them, ships near the docks or in the dock queue
void DrawDockMap(Vector2 at, Vector2 size)
{
    List<IMyShipConnector> docks = DockList();
    DText("Dock area" + (dockAreaStatus != "" ? ": " + dockAreaStatus : ""), at + new Vector2(10 * mK, 4 * mK), Color.White, 0.6f, TextAlignment.LEFT);
    if (docks.Count == 0) { DText("No [PAM-Dock] connectors, no ship asked yet", at + size / 2, Color.Gray, 0.6f, TextAlignment.CENTER); return; }
    List<Vector3> pts = new List<Vector3>(), box = new List<Vector3>();
    foreach (IMyShipConnector c in docks) { pts.Add(c.GetPosition()); pts.Add(c.GetPosition() + c.WorldMatrix.Forward * dockLaneLength); }
    foreach (IMySensorBlock z in zoneSensors)
    {
        MatrixD m = z.WorldMatrix;
        for (int i = 0; i < 8; i++)
            box.Add(m.Translation + (i % 2 == 0 ? m.Left * z.LeftExtend : m.Right * z.RightExtend) + (i / 2 % 2 == 0 ? m.Up * z.TopExtend : m.Down * z.BottomExtend) +
                (i / 4 == 0 ? m.Forward * z.FrontExtend : m.Backward * z.BackExtend));
    }
    pts.AddRange(box);
    // ships in the queue, cleared or docking, or within the area so far
    float r = 0;
    foreach (Vector3 v in pts) r = Math.Max(r, Vector3.Distance(v, mO));
    foreach (TCShip t in tcShips)
        if (t.flag == 1 || ShipDock(t.id) != null || t.id == clearedShip || Vector3.Distance(t.pos, mO) < r * 1.5f) pts.Add(t.pos);
    int lines = (docks.Count + 1) / 2;
    float s, bottom = (58 + 16 * lines) * mK;
    Func<Vector3, Vector2> P = TopFrame(pts, at, size, 30 * mK, bottom, 5, out s);
    ScaleBar(at, size, s);
    // zone boxes (all 12 edges, seen from above)
    for (int k = 0; k < box.Count; k += 8)
        for (int i = 0; i < 8; i++)
            for (int b = 1; b < 8; b <<= 1)
                if ((i & b) == 0) DLine(P(box[k + i]), P(box[k + (i | b)]), Color.Violet, mK);
    for (int i = 0; i < docks.Count; i++)
    {
        IMyShipConnector c = docks[i];
        Color col = DockCol(c);
        Vector2 q = P(c.GetPosition());
        DLine(q, P(c.GetPosition() + c.WorldMatrix.Forward * dockLaneLength), new Color(col.R / 3, col.G / 3, col.B / 3), 6 * mK);
        DBox(q, new Vector2(12, 12) * mK, col, "SquareSimple");
        DText("" + (i + 1), q + new Vector2(0, 6 * mK), col, 0.5f, TextAlignment.CENTER);
        LaneBlock lb = LaneBlockOf(c.EntityId);
        if (lb != null) { DLine(P(lb.cam), P(lb.hit), Color.Red, 2 * mK); DText(lb.what, P(lb.hit), Color.Red, 0.5f, TextAlignment.LEFT); }
        // key under the map: number, name, state (2 per line)
        DText((i + 1) + " " + c.CustomName.Replace(dockPoolTag, "").Trim() + ": " + DockSt(c),
            at + new Vector2(10 * mK + i % 2 * size.X / 2, size.Y - bottom + (10 + i / 2 * 16) * mK), col, 0.5f, TextAlignment.LEFT);
    }
    foreach (TCShip t in tcShips) DShip(t, P);
}

Vector3 JobPos(JobRec job) { String[] k = job.key.Split('|'); return new Vector3(float.Parse(k[0]), float.Parse(k[1]), float.Parse(k[2])); }
