// ========================== DEBUG DRAW (station, [PAM-Controller] PB) ==========================
// With debugDraw on and Digi's "(DevTool) Programmable Block DebugAPI" mod in the world (offline
// worlds only), the controller draws what it knows in 3D, twice a second:
//  - every PAM ship: wire sphere of its size (green moving, yellow giving way, grey parked),
//    white line where it will be in 3 s, cyan line the way it wants to go
//  - red line between two ships the controller sees on a collision course
//  - with debugDraw on the ships too: blue line to each ship's flight target, its camera rays
//    (green clear, red hit), and the hole grid of each job (grey done, orange claimed with a line
//    from the ship to its hole, white free)
//  - docking area: [PAM-Zone] spheres, [PAM-Lane] camera rays, green line from the ship cleared
//    to dock to its connector
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

    public DebugAPI(MyGridProgram program)
    {
        _pb = program.Me;
        var methods = _pb.GetProperty("DebugAPI")?.As<IReadOnlyDictionary<string, Delegate>>()?.GetValue(_pb);
        if (methods == null) return;
        _rmd = (Action<IMyProgrammableBlock>)methods["RemoveDraw"];
        _p = (Func<IMyProgrammableBlock, Vector3D, Color, float, float, bool, int>)methods["Point"];
        _ln = (Func<IMyProgrammableBlock, Vector3D, Vector3D, Color, float, float, bool, int>)methods["Line"];
        _sph = (Func<IMyProgrammableBlock, BoundingSphereD, Color, int, float, int, float, bool, int>)methods["Sphere"];
        ((Action<IMyProgrammableBlock>)methods["RemoveAll"])(_pb);
        ModDetected = true;
    }

    public void RemoveDraw() { if (_rmd != null) _rmd(_pb); }
    public void Line(Vector3D a, Vector3D b, Color c, float thickness) { if (_ln != null) _ln(_pb, a, b, c, thickness, -1, false); }
    public void Point(Vector3D a, Color c, float radius) { if (_p != null) _p(_pb, a, c, radius, -1, false); }
    public void Sphere(Vector3D center, double radius, Color c) { if (_sph != null) _sph(_pb, new BoundingSphereD(center, radius), c, 1, 0.05f, 30, -1, false); }
}

class DbgShip
{
    public long id;
    public Vector3 jobPos, left, down, target;
    public bool hasJob;
    public List<Vector3> rayFrom = new List<Vector3>(), rayTo = new List<Vector3>();
    public List<bool> rayHit = new List<bool>();
    public DateTime time;
}

DebugAPI dbg = null;
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
    s.time = DateTime.Now;
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
    for (int i = dbgShips.Count - 1; i >= 0; i--) if ((DateTime.Now - dbgShips[i].time).TotalSeconds > 5) dbgShips.RemoveAt(i);

    dbg.RemoveDraw();
    // ships
    for (int i = 0; i < tcShips.Count; i++)
    {
        TCShip s = tcShips[i];
        Color c = TCParked(s) ? Color.Gray : s.limit >= 0 ? Color.Yellow : Color.Lime;
        dbg.Sphere(s.pos, s.dia / 2, c);
        if (s.vel.LengthSquared() > 0.25f) dbg.Line(s.pos, s.pos + s.vel * 3, Color.White, 0.1f);
        if (s.heading.LengthSquared() > 0.25f) dbg.Line(s.pos, s.pos + s.heading * s.dia, Color.Cyan, 0.1f);
    }
    // ships on a collision course
    for (int i = 0; i < tcPairs.Count; i++)
    {
        if ((DateTime.Now - tcPairs[i].last).TotalSeconds > 0.5) continue;
        TCShip a = null, b = null;
        for (int k = 0; k < tcShips.Count; k++)
        {
            if (tcShips[k].id == tcPairs[i].a) a = tcShips[k];
            if (tcShips[k].id == tcPairs[i].b) b = tcShips[k];
        }
        if (a != null && b != null) dbg.Line(a.pos, b.pos, Color.Red, 0.2f);
    }
    // what the ships send: flight target, camera rays, hole grid
    for (int i = 0; i < dbgShips.Count; i++)
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
        for (int c = 0; c < job.done.Length; c++)
        {
            int col = c % job.w, row = c / job.w;
            Vector3 p = d.jobPos + d.left * ((job.w - 1f) / 2f - col) * job.tw - d.down * ((job.h - 1f) / 2f - row) * job.th;
            JobShip owner = HoleOwner(job, c, null);
            dbg.Point(p, job.done[c] == '1' ? Color.Gray : owner != null ? Color.Orange : Color.White, Math.Min(job.tw, job.th) * 0.2f);
            if (owner != null && owner.id == d.id) dbg.Line(t.pos, p, Color.Orange, 0.05f);
        }
    }
    // docking area
    for (int i = 0; i < zoneSensors.Count; i++)
    {
        IMySensorBlock z = zoneSensors[i];
        dbg.Sphere(z.GetPosition(), new Vector3D(Math.Max(z.LeftExtend, z.RightExtend), Math.Max(z.TopExtend, z.BottomExtend), Math.Max(z.FrontExtend, z.BackExtend)).Length(), Color.Cyan);
    }
    for (int i = 0; i < laneCameras.Count; i++)
        dbg.Line(laneCameras[i].GetPosition(), laneCameras[i].GetPosition() + laneCameras[i].WorldMatrix.Forward * dockLaneLength, Color.Purple, 0.05f);
    if (clearedShip != 0 && clearedConn != 0)
    {
        IMyShipConnector c = StationConnector(clearedConn);
        for (int k = 0; k < tcShips.Count; k++)
            if (c != null && tcShips[k].id == clearedShip) dbg.Line(tcShips[k].pos, c.GetPosition(), Color.Lime, 0.15f);
    }
}
