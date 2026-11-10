// ========================== TRAFFIC CONTROL (station, [PAM-Controller] PB) ==========================
// The controller hears every PAM ship's traffic message, so it sees all of them at once, also
// ships that are out of each other's radio range. Every tick it predicts each pair a few seconds
// ahead (velocity, or the direction a stopped ship wants to go) and, when two would come closer
// than their size, tells one of them to slow down or hold. Who gives way, the same rules the ships
// use themselves: the ship cleared to dock goes; then the higher right of way; going the same way,
// the one catching up gives way; otherwise the one farther from where it is going; otherwise the
// higher id. Ships meeting head-on pass each other on the right by themselves. The choice is kept
// while the conflict lasts (unless a right of way changes), so it can't flip back and forth. Ships parked or
// hovering without wanting to move are obstacles for the ships' own avoidance, not for this.

// tccHorizon [s]: how far ahead conflicts are predicted

class TCShip
{
    public long id, pb; // grid id (traffic), PB id (the controller's ship list)
    public Vector3 pos, vel, heading;
    public float dia, fw, fh, limit = -1;
    public int jobFlag; // 1 in the job area, 2 deep in its hole
    public int prio;    // right of way: 2 leaving the job site, 1 moving between holes / arriving
    public float metric = -1; // distance it still has to go (whole metres)
    public String job = "";
    public int phase;
    public DateTime time;
    public int hole = -1, flag, bat = -1, h2, ur, nav = -1; // claimed hole, 1 queued / 2 parked, energy, flight state (screens)
    public bool emg;
    public Vector3 up;
    public float ms = -1; // its script's average run time
}

class TCPair
{
    public long a, b, yielder;
    public int sig; // both priorities when decided: decide again when one changes (e.g. a ship turns home)
    public DateTime last;
}

List<TCShip> tcShips = new List<TCShip>();
List<TCPair> tcPairs = new List<TCPair>();
int tcSendTick = 0;

// traffic message: id;pos;vel;dia;job;hole;doneHoles;dock,phase;heading;queued;want,footW,footH,jobFlag
void HandleTrafficControl(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 9) return;
    long id = long.Parse(f[0]);
    TCShip s = null;
    for (int i = 0; i < tcShips.Count; i++) if (tcShips[i].id == id) s = tcShips[i];
    bool isNew = s == null;
    if (isNew) { s = new TCShip(); s.id = id; tcShips.Add(s); }
    s.pos = StringToVector(f[1]);
    s.vel = StringToVector(f[2]);
    s.dia = float.Parse(f[3]);
    s.job = f[4];
    s.phase = int.Parse(f[7].Split(',')[1]);
    s.heading = StringToVector(f[8]);
    s.jobFlag = 0; s.prio = 0;
    if (f.Length >= 11)
    {
        String[] w = f[10].Split(',');
        if (w.Length >= 4) { s.fw = float.Parse(w[1]); s.fh = float.Parse(w[2]); s.jobFlag = int.Parse(w[3]); if (w.Length >= 5) s.prio = int.Parse(w[4]); s.metric = w.Length >= 6 ? float.Parse(w[5]) : -1; }
    }
    s.pb = f.Length >= 12 ? long.Parse(f[11]) : 0;
    s.time = TimeNow();
    s.hole = int.Parse(f[5]);
    s.flag = f.Length >= 10 ? int.Parse(f[9]) : 0;
    if (f.Length >= 13)
    {
        String[] e = f[12].Split(',');
        s.bat = int.Parse(e[0]); s.h2 = int.Parse(e[1]); s.ur = int.Parse(e[2]); s.emg = e[3] == "1";
        s.nav = e.Length >= 6 ? int.Parse(e[4]) : -1; s.up = StringToVector(e[e.Length - 1]) / 10;
        s.ms = e.Length >= 7 ? float.Parse(e[5]) : -1;
    }
    if (isNew) AddLog("Contact: " + TCName(s));
}

// not moving and not wanting to move (docked, parked, waiting in the dock queue, idle)
bool TCParked(TCShip s) { return s.vel.LengthSquared() < 0.25f && (s.phase == 3 || s.heading.LengthSquared() < 0.25f); }

// where the ship is going: its velocity, or for a stopped ship the direction it wants to go
Vector3 TCIntent(TCShip s) { return s.vel.LengthSquared() >= 1f ? s.vel : s.heading * 2f; }

void UpdateTrafficControl()
{
    ComputeTrafficControl();
    if (++tcSendTick < 3) return; // ~2 per second
    tcSendTick = 0;
    IGC.SendBroadcastMessage(TCC_TAG, BuildTrafficControlMessage());
}

void ComputeTrafficControl()
{
    for (int i = tcShips.Count - 1; i >= 0; i--)
        // stopped ships (docked, idle) run and send less often: kept longer
        if ((TimeNow() - tcShips[i].time).TotalSeconds > (tcShips[i].vel.LengthSquared() < 0.25f ? 12 : 3)) { AddLog("Contact lost: " + TCName(tcShips[i])); tcShips.RemoveAt(i); }
    for (int i = tcPairs.Count - 1; i >= 0; i--)
        if ((TimeNow() - tcPairs[i].last).TotalSeconds > 2) tcPairs.RemoveAt(i); // conflict over: forget who gave way
    for (int i = 0; i < tcShips.Count; i++) tcShips[i].limit = -1;

    for (int i = 0; i < tcShips.Count; i++)
        for (int j = i + 1; j < tcShips.Count; j++)
        {
            TCShip a = tcShips[i], b = tcShips[j];
            if (TCParked(a) || TCParked(b)) continue;
            Vector3 pa = a.pos + a.vel * (float)(TimeNow() - a.time).TotalSeconds;
            Vector3 pb = b.pos + b.vel * (float)(TimeNow() - b.time).TotalSeconds;
            Vector3 ua = TCIntent(a), ub = TCIntent(b);
            Vector3 rel = pb - pa;
            float dist = rel.Length();
            float clear = (a.dia + b.dia) * 0.55f;
            // two ships working the same job drill different holes: only closer than one hole spacing counts
            bool sameJob = a.job != "" && a.job == b.job;
            if (sameJob && (a.jobFlag == 2 || b.jobFlag == 2)) continue; // one is inside the rock in its hole
            if (sameJob && a.jobFlag == 1 && b.jobFlag == 1)
            {
                String[] k = a.job.Split('|');
                if (k.Length >= 8) clear = Math.Min(float.Parse(k[6]), float.Parse(k[7])) * 0.9f;
            }
            Vector3 ru = ub - ua;
            float ru2 = ru.LengthSquared();
            float t = ru2 > 0.01f ? -Vector3.Dot(rel, ru) / ru2 : 0;
            bool tooClose = dist < clear;
            if (!tooClose && (t <= 0 || t > tccHorizon || (rel + ru * t).Length() >= clear)) continue;
            // head-on: the ships pass each other on the right themselves (holding one would stop it from
            // stepping aside); the controller handles ships catching up and crossing paths
            if (!tooClose && a.prio == b.prio && ua.LengthSquared() > 0.25f && ub.LengthSquared() > 0.25f && Vector3.Dot(Vector3.Normalize(ua), Vector3.Normalize(ub)) < -0.7f)
                continue;

            TCPair p = null;
            for (int k = 0; k < tcPairs.Count; k++)
                if ((tcPairs[k].a == a.id && tcPairs[k].b == b.id) || (tcPairs[k].a == b.id && tcPairs[k].b == a.id)) p = tcPairs[k];
            int sig = a.id < b.id ? a.prio * 8 + b.prio : b.prio * 8 + a.prio;
            if (p == null) { p = new TCPair(); p.a = a.id; p.b = b.id; p.sig = sig + 1; tcPairs.Add(p); }
            if (p.sig != sig)
            {
                long was = p.yielder;
                p.sig = sig; p.yielder = TCYielder(a, b, ua, ub, rel, dist);
                if (p.yielder != was) AddLog("Traffic: " + (p.yielder == a.id ? TCName(a) + " gives way to " + TCName(b) : TCName(b) + " gives way to " + TCName(a)));
            }
            p.last = TimeNow();
            TCShip y = p.yielder == a.id ? a : b;
            float lim = tooClose || t < 4 ? 0 : Math.Max(1, y.vel.Length() * 0.5f); // close: hold, later: slow down
            if (y.limit < 0 || lim < y.limit) y.limit = lim;
        }
    HoldQueuedShips();
}

long TCYielder(TCShip a, TCShip b, Vector3 ua, Vector3 ub, Vector3 rel, float dist)
{
    if (a.id == clearedShip || dockingShips.Contains(a.id)) return b.id; // cleared to dock by this station
    if (b.id == clearedShip || dockingShips.Contains(b.id)) return a.id;
    if (a.prio != b.prio) return a.prio < b.prio ? a.id : b.id; // ships leaving the job site go first
    // going the same way: the one catching up with the other gives way (the ships do that too: they follow)
    if (Vector3.Dot(ua, ub) > 0)
    {
        Vector3 n = dist > 0.01f ? rel / dist : new Vector3(1, 0, 0);
        float ta = Vector3.Dot(ua, n), tb = Vector3.Dot(ub, -n); // speed towards the other ship
        if (ta - tb > 1) return a.id;
        if (tb - ta > 1) return b.id;
    }
    // otherwise the one closer to where it is going goes first (same order as the ships use)
    if (a.metric >= 0 && b.metric >= 0 && a.metric != b.metric) return a.metric > b.metric ? a.id : b.id;
    return a.id > b.id ? a.id : b.id;
}

String BuildTrafficControlMessage()
{
    String msg = "C;";
    bool first = true;
    for (int i = 0; i < tcShips.Count; i++)
    {
        if (tcShips[i].limit < 0) continue;
        msg += (first ? "" : ",") + tcShips[i].id + ":" + Math.Round(tcShips[i].limit, 1);
        first = false;
    }
    return msg;
}

String GetTrafficControlLine()
{
    int moving = 0, held = 0;
    for (int i = 0; i < tcShips.Count; i++)
    {
        if (!TCParked(tcShips[i])) moving++;
        if (tcShips[i].limit >= 0) held++;
    }
    if (tcShips.Count == 0) return "";
    return "Traffic: " + moving + " moving" + (held > 0 ? ", " + held + " giving way" : "") + "\n";
}

// ---- "Move home" for all ships: one at a time ----
// Every ship gets the order at once, so none of them starts a new hole; traffic control then holds
// all but one (in their holes, or where they are). Order: ships already out of the job area first,
// then ships at the surface of the job area, then ships still deep in their holes; within each
// group the ship closest to the job position (where the path starts) goes first. The next ship
// may go when the previous one has moved away (3 ship sizes, at least 40 m), at once if the
// previous one is docked, or after 45 s at the latest.
List<ShipInfo> homeQueue = new List<ShipInfo>();
ShipInfo homeCurrent = null;
Vector3 homeStartPos;
DateTime homeSentTime;
float homeReleaseDist = 40;
bool homeCurrentDocked = false;

TCShip FindTCShip(ShipInfo s)
{
    TCShip best = null;
    for (int i = 0; i < tcShips.Count; i++) if (tcShips[i].pb != 0 && "" + tcShips[i].pb == s.id) return tcShips[i];
    for (int i = 0; i < tcShips.Count; i++)
        if (Vector3.Distance(tcShips[i].pos, s.position) < 15 && (best == null ||
            Vector3.Distance(tcShips[i].pos, s.position) < Vector3.Distance(best.pos, s.position))) best = tcShips[i];
    return best;
}

float HomeOrderKey(ShipInfo s)
{
    TCShip t = FindTCShip(s);
    if (t == null) return 0;
    float k = t.jobFlag * 1000000f;
    String[] key = t.job.Split('|');
    if (key.Length >= 3) k += Vector3.Distance(t.pos, new Vector3(float.Parse(key[0]), float.Parse(key[1]), float.Parse(key[2])));
    return k;
}

void StartStagedHome()
{
    homeQueue.Clear();
    homeCurrent = null;
    homeHeld.Clear();
    List<float> keys = new List<float>();
    for (int i = 0; i < connectedShips.Count; i++)
    {
        float k = HomeOrderKey(connectedShips[i]);
        int at = 0;
        while (at < keys.Count && keys[at] <= k) at++;
        keys.Insert(at, k);
        homeQueue.Insert(at, connectedShips[i]);
        connectedShips[i].ProcessCommand("HOMEPOS", true, false, false);
        SendBroadcastTo("HOMEPOS", connectedShips[i].id, "HOMEPOS");
    }
}

// ships of the staged "Move home" that have to wait: hold them (called after the pair checks)
List<TCShip> homeHeld = new List<TCShip>();
void HoldQueuedShips()
{
    homeHeld.Clear();
    for (int i = 0; i < homeQueue.Count; i++)
    {
        TCShip t = FindTCShip(homeQueue[i]);
        if (t == null) continue;
        t.limit = 0;
        homeHeld.Add(t);
    }
}

void UpdateStagedHome()
{
    if (homeCurrent != null)
    {
        bool gone = homeCurrentDocked || Vector3.Distance(homeCurrent.position, homeStartPos) > homeReleaseDist ||
            (TimeNow() - homeSentTime).TotalSeconds > 45 || homeCurrent.IsTimedOut();
        if (!gone) return;
        homeCurrent = null;
    }
    if (homeQueue.Count == 0) return;
    homeCurrent = homeQueue[0];
    homeQueue.RemoveAt(0);
    AddLog("Move home: " + homeCurrent.name + " goes (" + homeQueue.Count + " waiting)");
    homeStartPos = homeCurrent.position;
    homeSentTime = TimeNow();
    TCShip t = FindTCShip(homeCurrent);
    homeReleaseDist = t == null ? 40 : Math.Max(40, t.dia * 3);
    homeCurrentDocked = t != null && t.phase == 3 && t.vel.LengthSquared() < 0.25f;
}
