// ========================== DEBUG DRAW (station, [PAM-Controller] PB) ==========================
// With debugDraw on and Digi's "(DevTool) Programmable Block DebugAPI" mod in the world (offline
// worlds only), the controller draws what it knows in 3D, twice a second:
//  - every PAM ship: wire sphere of its size (green moving, yellow giving way, grey parked),
//    white line where it will be in 3 s, cyan line the way it wants to go
//  - red line between two ships the controller sees on a collision course
//  - with debugDraw on the ships too: blue line to each ship's flight target, its camera rays
//    (green clear, red hit), and the hole grid of each job (grey done, orange claimed with a line
//    from the ship to its hole, white free)
//  - docking area: [PAM-Zone] sensor boxes, [PAM-Lane] camera rays, green line from the ship cleared
//    to dock to its connector
// Text labels (HUD markers) name the ships with their state, the conflicts (who gives way), the
// claimed holes, the docking area and the cleared ship. The colour key is in the PB's output.
// Without the mod nothing is drawn and nothing else changes.

const String DBG_TAG = "[PAMDBG]";

// trimmed copy of the DebugAPI class from the mod's ForPB.cs (github.com/THDigi/PBDrawAPI)
class DebugAPI
{
    public readonly bool ModDetected;
    IMyProgrammableBlock _pb;
    Action<IMyProgrammableBlock> _rmd;
    Func<IMyProgrammableBlock, Vector3D, Color, float, float, bool, int> _p;
    Func<IMyProgrammableBlock, Vector3D, Vector3D, Color, float, float, bool, int> _ln;
    Func<IMyProgrammableBlock, BoundingSphereD, Color, int, float, int, float, bool, int> _sph;
    Func<IMyProgrammableBlock, string, Vector3D, Color?, float, int> _gps;

    public DebugAPI(MyGridProgram program)
    {
        _pb = program.Me;
        var methods = _pb.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string, Delegate>>()?.GetValue(_pb);
        if (methods == null) return;
        _rmd = (Action<IMyProgrammableBlock>)methods["RemoveDraw"];
        _p = (Func<IMyProgrammableBlock, Vector3D, Color, float, float, bool, int>)methods["Point"];
        _ln = (Func<IMyProgrammableBlock, Vector3D, Vector3D, Color, float, float, bool, int>)methods["Line"];
        _sph = (Func<IMyProgrammableBlock, BoundingSphereD, Color, int, float, int, float, bool, int>)methods["Sphere"];
        _gps = (Func<IMyProgrammableBlock, string, Vector3D, Color?, float, int>)methods["GPS"];
        ((Action<IMyProgrammableBlock>)methods["RemoveAll"])(_pb);
        ModDetected = true;
    }

    public void RemoveDraw() { if (_rmd != null) _rmd(_pb); }
    public void Line(Vector3D a, Vector3D b, Color c, float thickness) { if (_ln != null) _ln(_pb, a, b, c, thickness, -1, false); }
    public void Point(Vector3D a, Color c, float radius) { if (_p != null) _p(_pb, a, c, radius, -1, false); }
    public void Label(String text, Vector3D at, Color c) { if (_gps != null) _gps(_pb, text, at, c, -1); }
    public void Sphere(Vector3D center, double radius, Color c) { if (_sph != null) _sph(_pb, new BoundingSphereD(center, radius), c, 1, 0.05f, 30, -1, false); }
}

class DbgShip
{
    public long id;
    public Vector3 jobPos, left, down, target, fwd;
    public bool hasJob;
    public List<Vector3> rayFrom = new List<Vector3>(), rayTo = new List<Vector3>();
    public List<bool> rayHit = new List<bool>();
    public DateTime time;
}

DebugAPI dbg = null;
int dbgTick = 0;
IMyBroadcastListener dbgListener = null;
List<DbgShip> dbgShips = new List<DbgShip>();

// ship debug message: id;jobPos;left;down;target;rays (from,to,hit:...)
void HandleDebugMessage(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 6) return;
    long id = long.Parse(f[0]);
    DbgShip s = null;
    for (int i = 0; i < dbgShips.Count; i++) if (dbgShips[i].id == id) s = dbgShips[i];
    if (s == null) { s = new DbgShip(); s.id = id; dbgShips.Add(s); }
    s.jobPos = StringToVector(f[1]); s.left = StringToVector(f[2]); s.down = StringToVector(f[3]);
    s.hasJob = s.left.LengthSquared() > 0.5f;
    s.target = StringToVector(f[4]);
    s.time = TimeNow();
    if (f.Length >= 7) s.fwd = StringToVector(f[6]);
    if (f[5] == "") return; // keep the last rays until new ones come
    s.rayFrom.Clear(); s.rayTo.Clear(); s.rayHit.Clear();
    String[] rays = f[5].Split(':');
    for (int i = 0; i < rays.Length; i++)
    {
        String[] r = rays[i].Split(',');
        if (r.Length < 3) continue;
        s.rayFrom.Add(StringToVector(r[0])); s.rayTo.Add(StringToVector(r[1])); s.rayHit.Add(r[2] == "1");
    }
}

void UpdateDebugDraw()
{
    if (!debugDraw) return;
    if (dbg == null) dbg = new DebugAPI(this);
    if (!dbg.ModDetected) return;
    if (dbgListener == null) dbgListener = IGC.RegisterBroadcastListener(DBG_TAG);
    int budget = 20;
    while (dbgListener.HasPendingMessage && budget-- > 0)
    {
        String data = dbgListener.AcceptMessage().Data as String;
        if (data != null) try { HandleDebugMessage(data); } catch { }
    }
    for (int i = dbgShips.Count - 1; i >= 0; i--) if ((TimeNow() - dbgShips[i].time).TotalSeconds > 5) dbgShips.RemoveAt(i);
    if (++dbgTick % 3 != 0) return; // redraw ~2x a second (the drawings stay until removed)

    dbg.RemoveDraw();
    // many ships with debugDraw on: everything below stops near the instruction limit instead of the
    // script being stopped ("Script Too Complex"); the rest is drawn on the next redraw
    // recorded paths (ships sharing a path: drawn once)
    List<String> drawn = new List<String>();
    foreach (String ps in shipPaths.Values)
    {
        if (ps == "" || drawn.Contains(ps) || Heavy()) continue;
        drawn.Add(ps);
        String[] pts = ps.Split(':');
        for (int k = 1; k < pts.Length; k++) dbg.Line(StringToVector(pts[k - 1]), StringToVector(pts[k]), Color.SkyBlue, 0.1f);
        dbg.Label("Path start", StringToVector(pts[0]), Color.SkyBlue);
        dbg.Label("Path end", StringToVector(pts[pts.Length - 1]), Color.SkyBlue);
    }
    // ships
    for (int i = 0; i < tcShips.Count && !Heavy(); i++)
    {
        TCShip s = tcShips[i];
        Color c = TCParked(s) ? Color.Gray : s.limit >= 0 ? Color.Yellow : Color.Lime;
        dbg.Sphere(s.pos, s.dia / 2, c);
        dbg.Label(TCName(s) + ": " + (TCParked(s) ? "parked" : s.limit == 0 ? "holding (giving way)" : s.limit > 0 ? "slowing (giving way)" : "moving"), s.pos, c);
        if (s.vel.LengthSquared() > 0.25f) dbg.Line(s.pos, s.pos + s.vel * 3, Color.White, 0.1f);
        if (s.heading.LengthSquared() > 0.25f) dbg.Line(s.pos, s.pos + s.heading * s.dia, Color.Cyan, 0.1f);
    }
    // ships on a collision course
    for (int i = 0; i < tcPairs.Count; i++)
    {
        if ((TimeNow() - tcPairs[i].last).TotalSeconds > 0.5) continue;
        TCShip a = null, b = null;
        for (int k = 0; k < tcShips.Count; k++)
        {
            if (tcShips[k].id == tcPairs[i].a) a = tcShips[k];
            if (tcShips[k].id == tcPairs[i].b) b = tcShips[k];
        }
        if (a == null || b == null) continue;
        dbg.Line(a.pos, b.pos, Color.Red, 0.2f);
        dbg.Label("Conflict: " + TCName(tcPairs[i].yielder == a.id ? a : b) + " gives way", (a.pos + b.pos) / 2, Color.Red);
    }
    // what the ships send: flight target, camera rays, hole grid
    drawn.Clear(); // jobs whose hole grid is drawn
    for (int i = 0; i < dbgShips.Count && !Heavy(); i++)
    {
        DbgShip d = dbgShips[i];
        TCShip t = null;
        for (int k = 0; k < tcShips.Count; k++) if (tcShips[k].id == d.id) t = tcShips[k];
        if (t != null) dbg.Line(t.pos, d.target, Color.Blue, 0.08f);
        for (int r = 0; r < d.rayFrom.Count; r++) dbg.Line(d.rayFrom[r], d.rayTo[r], d.rayHit[r] ? Color.Red : Color.Green, 0.04f);
        if (t == null || !d.hasJob) continue;
        JobRec job = null;
        for (int j = 0; j < jobs.Count; j++) if (jobs[j].key == t.job) job = jobs[j];
        if (job == null) continue;
        int doneCount = 0;
        for (int c = 0; c < job.done.Length; c++) if (job.done[c] == '1') doneCount++;
        bool firstOfJob = !drawn.Contains(job.key);
        if (firstOfJob) drawn.Add(job.key);
        if (firstOfJob)
        {
            dbg.Label("Job: " + doneCount + "/" + job.done.Length + " holes done", d.jobPos, Color.White);
            // waiting ring (as the ships compute it, for this ship's size): 6 spots
            for (int k = 0; k < 6; k++)
            {
                double a = k * Math.PI / 3;
                dbg.Point(d.jobPos - d.fwd * t.dia * 2.5f + (d.left * (float)Math.Cos(a) + d.down * (float)Math.Sin(a)) * t.dia * 1.2f, Color.Violet, t.dia * 0.15f);
            }
            dbg.Label("Waiting ring", d.jobPos - d.fwd * t.dia * 2.5f, Color.Violet);
        }
        // the grid once per job; for the other ships only their own hole
        JobShip own = job.ships.Find(q => q.id == d.id);
        for (int c = 0; c < job.done.Length; c++)
        {
            if (!firstOfJob && (own == null || own.cell != c)) continue;
            int col = c % job.w, row = c / job.w;
            Vector3 p = d.jobPos + d.left * ((job.w - 1f) / 2f - col) * job.tw - d.down * ((job.h - 1f) / 2f - row) * job.th;
            JobShip owner = HoleOwner(job, c, null);
            Color hc = job.done[c] == '1' ? Color.Gray : owner != null ? Color.Orange : Color.White;
            dbg.Point(p, hc, Math.Min(job.tw, job.th) * 0.2f);
            // where ships line up over the hole (one ship size above the surface) and come straight down
            if (firstOfJob) dbg.Line(p, p - d.fwd * t.dia, hc, 0.05f);
            if (owner == null || owner.id != d.id) continue;
            dbg.Line(t.pos, p, Color.Orange, 0.05f);
            dbg.Label("Hole " + (job.order.IndexOf(c) + 1) + ": " + TCName(t), p, Color.Orange);
        }
    }
    // docking area
    for (int i = 0; i < zoneSensors.Count; i++)
    {
        IMySensorBlock z = zoneSensors[i];
        // the sensor box: 12 edges
        MatrixD m = z.WorldMatrix;
        Vector3D[] c = new Vector3D[8];
        for (int k = 0; k < 8; k++)
            c[k] = z.GetPosition() + m.Right * ((k & 1) == 0 ? -z.LeftExtend : z.RightExtend) + m.Up * ((k & 2) == 0 ? -z.BottomExtend : z.TopExtend) + m.Forward * ((k & 4) == 0 ? -z.BackExtend : z.FrontExtend);
        for (int k = 0; k < 8; k++)
            for (int b = 1; b < 8; b <<= 1)
                if ((k & b) == 0) dbg.Line(c[k], c[k | b], Color.Cyan, 0.1f);
        dbg.Label("Docking area" + (dockAreaStatus != "" ? ": " + dockAreaStatus : ""), z.GetPosition(), Color.Cyan);
    }
    for (int i = 0; i < laneCameras.Count; i++)
        dbg.Line(laneCameras[i].GetPosition(), laneCameras[i].GetPosition() + laneCameras[i].WorldMatrix.Forward * dockLaneLength, Color.Purple, 0.05f);
    // blocked lanes: red from the lane camera to what's in the way, with what and which connector
    foreach (LaneBlock b in laneBlocks)
    {
        IMyShipConnector bc = StationConnector(b.conn);
        dbg.Line(b.cam, b.hit, Color.Red, 0.2f);
        dbg.Point(b.hit, Color.Red, 0.6f);
        dbg.Label("Lane blocked: " + b.what + (bc != null ? " (" + bc.CustomName + ")" : "") + (b.ship != 0 ? ", " + ShipName(b.ship) + " can't dock" : ""), b.hit, Color.Red);
    }
    if (clearedShip != 0 && clearedConn != 0)
    {
        IMyShipConnector c = StationConnector(clearedConn);
        for (int k = 0; k < tcShips.Count; k++)
            if (c != null && tcShips[k].id == clearedShip)
            {
                dbg.Line(tcShips[k].pos, c.GetPosition(), Color.Lime, 0.15f);
                dbg.Label("Cleared to " + (clearedKind == 2 ? "undock: " : "dock: ") + TCName(tcShips[k]), c.GetPosition(), Color.Lime);
            }
    }
}

// ship name from the controller's ship list (matched by position), else the end of its id
String TCName(TCShip t)
{
    ShipInfo best = null;
    for (int i = 0; i < connectedShips.Count; i++) if (t.pb != 0 && connectedShips[i].id == "" + t.pb) return connectedShips[i].name;
    for (int i = 0; i < connectedShips.Count; i++)
        if (Vector3.Distance(connectedShips[i].position, t.pos) < 30 && (best == null ||
            Vector3.Distance(connectedShips[i].position, t.pos) < Vector3.Distance(best.position, t.pos))) best = connectedShips[i];
    if (best != null && best.name != "") return best.name;
    String id = t.id.ToString();
    return "Ship " + id.Substring(Math.Max(0, id.Length - 4));
}

// colour key and mod status for the PB's output
String GetDebugDrawLegend()
{
    if (!debugDraw) return "";
    if (dbg == null || !dbg.ModDetected) return "\nDebug draw: DebugAPI mod not found (offline world + mod needed, then recompile)\n";
    return "\nDebug draw: on\n" +
        "Ship sphere: green moving, yellow giving way, grey parked\n" +
        "White: where it is in 3 s, cyan: where it wants to go\n" +
        "Red line: collision course (label: who gives way)\n" +
        "Blue: flight target; thin green/red: camera rays (clear/hit)\n" +
        "Points: grey hole done, orange claimed, white free\n" +
        "Line up from a hole: where ships line up over it and come down\n" +
        "Violet: waiting ring; sky blue: recorded path\n" +
        "Cyan box: docking area, purple: lane cameras, red: lane blocked\n" +
        "Thick green: ship cleared to its connector\n";
}
