// ========================== CONTROLLER SCREENS ==========================
// Extra LCD modes (Custom Data "mode=..."):
//   fleet          one line per ship: state, cargo, energy, hole, flags; docking area and jobs
//   ship:<name>    one ship in detail
//   log            coordination events (docking, holes, giving way, emergencies, contact), newest first
//   job:<n>        hole matrix of job n (drawn, as seen behind a ship drilling it)
//   map            side view base -> job: path, job site, ships (drawn; wide LCD recommended)
// The drawn ones redraw about once a second.

List<String> logLines = new List<String>();
List<DateTime> logTimes = new List<DateTime>();
Dictionary<long, String> shipPaths = new Dictionary<long, String>(); // grid id -> recorded path "x|y|z:..."
IMyBroadcastListener pathListener = null;
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
        int k = data.IndexOf(';');
        long id;
        if (k > 0 && long.TryParse(data.Substring(0, k), out id)) shipPaths[id] = data.Substring(k + 1);
    }
}

bool SpriteMode(LCDMode m) { return m == LCDMode.Map || m == LCDMode.Job; }

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

String BuildFleetScreen()
{
    String text = "PAM fleet | " + connectedShips.Count + " ships\n" + GetDockAreaLine() + GetJobLines() + GetTrafficControlLine();
    text += Pad("Ship", 10) + Pad("State", 12) + " Crg  Bat  H2  Hole Flags\n";
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
        text += Pad(s.name, 9) + " " + Pad(s.IsTimedOut() ? "NO CONTACT" : ShortState(s.stateText), 11) + " " +
            Pct((int)(SafeDiv(s.currentVolume, s.maxVolume) * 100)) + " " + Pct(t != null ? t.bat : -1) + " " + Pct(t != null ? t.h2 : -1) + " " + hole + "  " + ShipFlags(t) + "\n";
    }
    return text;
}

String BuildShipScreen(String name)
{
    ShipInfo s = FindShip("", name);
    if (s == null) return "Unknown ship: " + name;
    TCShip t = FindTCShip(s);
    String text = s.name + " | " + s.distance + " m from here" + (s.IsTimedOut() ? " | NO CONTACT" : "") + "\n" + "—————————————————————\n";
    text += "State:  " + s.stateText + "\n" + "Status: " + s.statusMessage + "\n";
    text += ProgressBar("Cargo", SafeDiv(s.currentVolume, s.maxVolume) * 100f, 100, 20, 8, 7) + "%\n";
    if (t == null) return text + "(no traffic data: traffic off or out of range)\n";
    text += "Battery " + Pct(t.bat).Trim() + "  H2 " + Pct(t.h2).Trim() + "  U " + t.ur + " kg\n";
    text += "Speed " + Math.Round(t.vel.Length(), 1) + " m/s\n";
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

String BuildLogScreen()
{
    String text = "Coordination log (newest first)\n" + "—————————————————————\n";
    for (int i = 0; i < logLines.Count; i++) text += logLines[i] + "\n";
    if (logLines.Count == 0) text += "(nothing yet)\n";
    return text;
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

void DText(String s, Vector2 at, Color c, float scale, TextAlignment align)
{
    MySprite t = MySprite.CreateText(s, "Debug", c, scale, align);
    t.Position = at;
    drawFrame.Add(t);
}

void DrawSpriteScreen(IMyTextPanel p, LCDMode mode, String param)
{
    if (p.ContentType != ContentType.SCRIPT) { p.ContentType = ContentType.SCRIPT; p.Script = ""; }
    p.ScriptBackgroundColor = new Color(0, 0, 0);
    Vector2 size = p.SurfaceSize, at = (p.TextureSize - size) / 2;
    drawFrame = p.DrawFrame();
    if (mode == LCDMode.Job) DrawJobMatrix(param, at, size);
    else DrawSideMap(at, size);
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
    float top = 40, bottom = 30;
    float cell = Math.Min(size.X / job.w, (size.Y - top - bottom) / job.h);
    Vector2 origin = at + new Vector2((size.X - cell * job.w) / 2, top + (size.Y - top - bottom - cell * job.h) / 2);
    for (int c = 0; c < job.done.Length; c++)
    {
        Vector2 ctr = origin + new Vector2((c % job.w + 0.5f) * cell, (c / job.w + 0.5f) * cell);
        JobShip owner = HoleOwner(job, c, null);
        Color col = job.done[c] == '1' ? new Color(90, 90, 90) : owner != null ? Color.Orange : new Color(30, 60, 90);
        DBox(ctr, new Vector2(cell * 0.9f, cell * 0.9f), col, "SquareSimple");
        if (owner == null) continue;
        String nm = ShipName(owner.id);
        DText(nm.Substring(Math.Max(0, nm.Length - 2)).Trim(), ctr - new Vector2(0, cell * 0.3f), Color.Black, Math.Min(1.2f, cell / 40), TextAlignment.CENTER);
    }
    DText("grey done | orange: ship working | blue free", at + new Vector2(size.X / 2, size.Y - 24), Color.Gray, 0.6f, TextAlignment.CENTER);
}

// side view: horizontal = distance from the station towards the job, vertical = height (gravity),
// each with its own scale (heights are exaggerated to be readable)
void DrawSideMap(Vector2 at, Vector2 size)
{
    Vector3 o = Me.GetPosition();
    Vector3 up = new Vector3();
    for (int i = 0; i < tcShips.Count; i++) up += tcShips[i].up;
    if (up.LengthSquared() < 0.01f) up = Me.WorldMatrix.Up;
    up.Normalize();
    // towards the first job (else the far end of a path, else the first ship)
    Vector3 far = o;
    if (jobs.Count > 0) { String[] k = jobs[0].key.Split('|'); far = new Vector3(float.Parse(k[0]), float.Parse(k[1]), float.Parse(k[2])); }
    List<Vector3> pts = new List<Vector3>();
    foreach (String ps in shipPaths.Values) { pts.Clear(); foreach (String w in ps.Split(':')) if (w != "") pts.Add(StringToVector(w)); break; }
    if (far == o && pts.Count > 0) far = Vector3.Distance(pts[0], o) > Vector3.Distance(pts[pts.Count - 1], o) ? pts[0] : pts[pts.Count - 1];
    if (far == o && tcShips.Count > 0) far = tcShips[0].pos;
    Vector3 along = far - o - up * Vector3.Dot(far - o, up);
    if (along.LengthSquared() < 1) along = Vector3.Cross(up, Me.WorldMatrix.Right);
    along.Normalize();
    // bounds
    float x0 = 0, x1 = 20, y0 = -10, y1 = 10;
    List<Vector3> all = new List<Vector3>(pts);
    all.Add(far);
    for (int i = 0; i < tcShips.Count; i++) all.Add(tcShips[i].pos);
    for (int i = 0; i < all.Count; i++)
    {
        Vector3 d = all[i] - o;
        float x = Vector3.Dot(d, along), y = Vector3.Dot(d, up);
        x0 = Math.Min(x0, x); x1 = Math.Max(x1, x); y0 = Math.Min(y0, y); y1 = Math.Max(y1, y);
    }
    float mx = (x1 - x0) * 0.08f + 5, my = (y1 - y0) * 0.15f + 5;
    x0 -= mx; x1 += mx; y0 -= my; y1 += my;
    float top = 30, bottom = 26;
    Vector2 area = new Vector2(size.X, size.Y - top - bottom);
    float sx = area.X / (x1 - x0), sy = area.Y / (y1 - y0);
    Func<Vector3, Vector2> P = v => { Vector3 d = v - o; return at + new Vector2((Vector3.Dot(d, along) - x0) * sx, top + (y1 - Vector3.Dot(d, up)) * sy); };

    DText("Side view: station -> job | heights x" + Math.Round(sy / sx, 1), at + new Vector2(size.X / 2, 4), Color.White, 0.7f, TextAlignment.CENTER);
    // ground line at the station's height
    DLine(P(o + along * x0), P(o + along * x1), new Color(40, 40, 40), 1);
    // path (fewer segments when they'd be shorter than ~3 px)
    Vector2 last = new Vector2(-9999, -9999);
    for (int i = 0; i < pts.Count; i++)
    {
        Vector2 q = P(pts[i]);
        if (i > 0 && (q - last).Length() < 3 && i < pts.Count - 1) continue;
        if (i > 0) DLine(last, q, Color.SkyBlue, 2);
        last = q;
    }
    // station, job site
    DBox(P(o), new Vector2(14, 14), Color.White, "SquareSimple");
    DText("Station", P(o) + new Vector2(0, 10), Color.White, 0.6f, TextAlignment.CENTER);
    for (int j = 0; j < jobs.Count; j++)
    {
        String[] k = jobs[j].key.Split('|');
        Vector3 jp = new Vector3(float.Parse(k[0]), float.Parse(k[1]), float.Parse(k[2]));
        int n = 0;
        for (int i = 0; i < jobs[j].done.Length; i++) if (jobs[j].done[i] == '1') n++;
        float half = jobs[j].w * jobs[j].tw / 2;
        DLine(P(jp - along * half), P(jp + along * half), Color.Orange, 4);
        DText("Job " + (j + 1) + " " + n + "/" + jobs[j].done.Length, P(jp) + new Vector2(0, 6), Color.Orange, 0.6f, TextAlignment.CENTER);
    }
    // ships: colour by what they do, a line where they're heading
    for (int i = 0; i < tcShips.Count; i++)
    {
        TCShip t = tcShips[i];
        Color c = t.emg ? Color.Red : t.limit >= 0 ? Color.Yellow : t.prio < 0 ? Color.Violet : TCParked(t) ? Color.Gray : Color.Lime;
        Vector2 q = P(t.pos);
        if (t.vel.LengthSquared() > 0.25f) DLine(q, P(t.pos + t.vel * 5), c, 2);
        DBox(q, new Vector2(12, 12), c, "Circle");
        DText(TCName(t), q - new Vector2(0, 26), c, 0.55f, TextAlignment.CENTER);
    }
    DText("green moving | yellow giving way | violet waiting | grey parked | red emergency", at + new Vector2(size.X / 2, size.Y - 22), Color.Gray, 0.5f, TextAlignment.CENTER);
}
