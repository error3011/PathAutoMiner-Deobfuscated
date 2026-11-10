using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using IMyProgrammableBlock = Sandbox.ModAPI.IMyProgrammableBlock;

namespace PamUpdater
{
    /// <summary>
    /// Pushes a new version of the PAM script to all of a player's PAM programmable blocks.
    /// The code comes from a PB named [PAM-Ship-Master] (ships) / [PAM-Controller-Master] (controllers)
    /// owned by the same player. Only PBs that already run PAM and are owned by that player are changed.
    /// Triggers: chat "/pamupdate" (ships and controllers), or a PB script calling the "PamUpdate" terminal
    /// property (ships only; the PAM controller's "Update scripts" menu entry / UPDATE argument).
    /// Job data is kept: before recompiling, the game runs the old script's Save() and passes its Storage on.
    /// </summary>
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class PamUpdaterSession : MySessionComponentBase
    {
        public const string ModName = "PamUpdater";
        const ushort NetId = 47391;
        const string Command = "/pamupdate";
        const string ShipMaster = "[PAM-Ship-Master]";
        const string ControllerMaster = "[PAM-Controller-Master]";
        const string ControllerTag = "[PAM-Controller]";
        const string PamMark = "[PAM] - Path Auto Miner"; // in the header of every PAM version
        const int TicksPerBlock = 60; // one recompile per second: compiling PAM is heavy
        public const string PropertyId = "PamUpdate";

        public static PamUpdaterSession Instance;
        public static bool PropertyCreated;

        class Job { public IMyProgrammableBlock Pb; public string Code; public Batch Batch; }
        class Batch { public long Owner; public ulong SteamId; public int Total, Done, Failed; public List<string> FailedNames = new List<string>(); }

        readonly List<Job> queue = new List<Job>();
        readonly HashSet<IMyEntity> tmpGrids = new HashSet<IMyEntity>();
        readonly List<IMySlimBlock> tmpSlims = new List<IMySlimBlock>();
        bool isServer;
        int tick;

        public override void LoadData()
        {
            try
            {
                Instance = this;
                isServer = MyAPIGateway.Session.IsServer;
                MyAPIGateway.Multiplayer.RegisterSecureMessageHandler(NetId, OnMessage);
                if (!MyAPIGateway.Utilities.IsDedicated)
                    MyAPIGateway.Utilities.MessageEnteredSender += OnChat;
            }
            catch (Exception e) { Log(e); }
        }

        protected override void UnloadData()
        {
            try
            {
                MyAPIGateway.Multiplayer.UnregisterSecureMessageHandler(NetId, OnMessage);
                MyAPIGateway.Utilities.MessageEnteredSender -= OnChat;
            }
            catch (Exception e) { Log(e); }
            queue.Clear();
            Instance = null;
            PropertyCreated = false;
        }

        /// <summary>Called by <see cref="PbHook"/> on the first PB that spawns.</summary>
        public void CreateProperty()
        {
            if (PropertyCreated) return;
            PropertyCreated = true;
            var prop = MyAPIGateway.TerminalControls.CreateProperty<Func<Sandbox.ModAPI.Ingame.IMyTerminalBlock, string>, IMyProgrammableBlock>(PropertyId);
            prop.Getter = b => FromPb;
            prop.Setter = (b, v) => { };
            MyAPIGateway.TerminalControls.AddControl<IMyProgrammableBlock>(prop);
        }

        // ---- triggers ----

        // PB scripts run on the server only: the calling PB's owner is the player
        static string FromPb(Sandbox.ModAPI.Ingame.IMyTerminalBlock caller)
        {
            try
            {
                PamUpdaterSession s = Instance;
                IMyProgrammableBlock pb = caller as IMyProgrammableBlock;
                if (s == null || !s.isServer || pb == null) return "Update: not available here";
                return s.Queue(pb.OwnerId, false, pb);
            }
            catch (Exception e) { Log(e); return "Update: error, see the server log"; }
        }

        void OnChat(ulong sender, string text, ref bool sendToOthers)
        {
            try
            {
                if (text == null || !text.Trim().Equals(Command, StringComparison.OrdinalIgnoreCase)) return;
                sendToOthers = false;
                if (isServer) // singleplayer / lobby host
                {
                    IMyPlayer me = MyAPIGateway.Session.Player;
                    if (me != null) Show(Queue(me.IdentityId, true, null));
                    return;
                }
                MyAPIGateway.Multiplayer.SendMessageToServer(NetId, Encoding.UTF8.GetBytes("U"), true);
            }
            catch (Exception e) { Log(e); }
        }

        void OnMessage(ushort handlerId, byte[] data, ulong sender, bool fromServer)
        {
            try
            {
                if (isServer && !fromServer)
                {
                    // the sender's identity comes from the game (secure handler), not from the message
                    long identity = MyAPIGateway.Players.TryGetIdentityId(sender);
                    if (identity == 0 || data == null || data.Length != 1 || data[0] != (byte)'U') return;
                    Send(sender, Queue(identity, true, null));
                }
                else if (fromServer && data != null)
                    Show(Encoding.UTF8.GetString(data));
            }
            catch (Exception e) { Log(e); }
        }

        // ---- the update ----

        // Finds the player's masters and PAM PBs, queues the ones whose code differs. Returns the reply text.
        string Queue(long owner, bool controllers, IMyProgrammableBlock caller)
        {
            if (owner == 0) return "Update: the PB has no owner";
            foreach (Job j in queue)
                if (j.Pb.OwnerId == owner) return "Update: already running, " + queue.Count + " PB(s) left";

            List<IMyProgrammableBlock> pbs = new List<IMyProgrammableBlock>();
            tmpGrids.Clear();
            MyAPIGateway.Entities.GetEntities(tmpGrids, e => e is IMyCubeGrid);
            foreach (IMyEntity e in tmpGrids)
            {
                tmpSlims.Clear();
                ((IMyCubeGrid)e).GetBlocks(tmpSlims, s => s.FatBlock is IMyProgrammableBlock);
                foreach (IMySlimBlock s in tmpSlims)
                {
                    IMyProgrammableBlock pb = (IMyProgrammableBlock)s.FatBlock;
                    if (pb.OwnerId == owner && !pb.MarkedForClose) pbs.Add(pb);
                }
            }
            tmpGrids.Clear();
            tmpSlims.Clear();

            Batch batch = new Batch();
            batch.Owner = owner;
            batch.SteamId = MyAPIGateway.Players.TryGetSteamId(owner);
            int same = 0;
            string text = "";
            string[] masters = controllers ? new[] { ShipMaster, ControllerMaster } : new[] { ShipMaster };
            foreach (string tag in masters)
            {
                bool forController = tag == ControllerMaster;
                string kind = forController ? "Controllers" : "Ships";
                IMyProgrammableBlock master = null;
                int count = 0;
                foreach (IMyProgrammableBlock pb in pbs)
                    if (pb.CustomName.Contains(tag)) { master = pb; count++; }
                if (count == 0) { if (!forController) text += "No PB named " + tag + " of yours. "; continue; }
                if (count > 1) { text += kind + ": more than one " + tag + ". "; continue; }
                string code = master.ProgramData ?? "";
                if (!code.Contains(PamMark)) { text += kind + ": " + tag + " doesn't hold the PAM script. "; continue; }
                int queued = 0;
                foreach (IMyProgrammableBlock pb in pbs)
                {
                    if (pb == master || pb == caller || pb.CustomName.Contains(ShipMaster) || pb.CustomName.Contains(ControllerMaster)) continue;
                    string old = pb.ProgramData ?? "";
                    if (!old.Contains(PamMark)) continue; // another script: never touched
                    if (pb.CustomName.Contains(ControllerTag) != forController) continue;
                    if (old == code) { same++; continue; }
                    Job j = new Job();
                    j.Pb = pb; j.Code = code; j.Batch = batch;
                    queue.Add(j);
                    queued++;
                }
                text += kind + ": " + queued + " to update. ";
            }
            batch.Total = 0;
            foreach (Job j in queue) if (j.Batch == batch) batch.Total++;
            if (same > 0) text += same + " already up to date. ";
            if (batch.Total > 0) text += "About " + batch.Total + " s.";
            MyLog.Default.WriteLineAndConsole("[" + ModName + "] " + owner + ": " + text);
            return "Update: " + text.Trim();
        }

        public override void UpdateBeforeSimulation()
        {
            if (!isServer || queue.Count == 0 || ++tick < TicksPerBlock) return;
            tick = 0;
            Job j = queue[0];
            queue.RemoveAt(0);
            try
            {
                // owner checked again: the block may have changed hands since it was queued
                if (!j.Pb.MarkedForClose && j.Pb.OwnerId == j.Batch.Owner)
                {
                    // the server recompiles at once; the old script's Save() runs first and its Storage is kept
                    j.Pb.ProgramData = j.Code;
                    if (j.Pb.HasCompileErrors) { j.Batch.Failed++; j.Batch.FailedNames.Add(j.Pb.CustomName); }
                }
            }
            catch (Exception e) { Log(e); j.Batch.Failed++; j.Batch.FailedNames.Add(j.Pb.CustomName); }
            j.Batch.Done++;
            if (j.Batch.Done >= j.Batch.Total)
            {
                string msg = "Update done: " + (j.Batch.Done - j.Batch.Failed) + " PB(s) updated" +
                    (j.Batch.Failed > 0 ? ", compile errors in: " + string.Join(", ", j.Batch.FailedNames) : "") + ".";
                MyLog.Default.WriteLineAndConsole("[" + ModName + "] " + msg);
                if (j.Batch.SteamId != 0) Send(j.Batch.SteamId, msg);
            }
        }

        // ---- messages ----

        void Send(ulong steamId, string text)
        {
            IMyPlayer me = MyAPIGateway.Session.Player;
            if (me != null && me.SteamUserId == steamId) { Show(text); return; } // local player on the server
            MyAPIGateway.Multiplayer.SendMessageTo(NetId, Encoding.UTF8.GetBytes(text), steamId, true);
        }

        static void Show(string text)
        {
            MyAPIGateway.Utilities.ShowMessage(ModName, text);
        }

        public static void Log(Exception e)
        {
            MyLog.Default.WriteLineAndConsole("[" + ModName + "] ERROR: " + e);
        }
    }

    /// <summary>Only job: add the "PamUpdate" terminal property once the first PB exists.</summary>
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_MyProgrammableBlock), false)]
    public class PbHook : MyGameLogicComponent
    {
        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (!PamUpdaterSession.PropertyCreated)
                NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            try
            {
                if (PamUpdaterSession.Instance != null)
                    PamUpdaterSession.Instance.CreateProperty();
            }
            catch (Exception e) { PamUpdaterSession.Log(e); }
        }
    }
}
