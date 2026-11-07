// ========================== TRAFFIC CONTROL (station, [PAM-Controller] PB) ==========================
// The controller hears every PAM ship's traffic message, so it sees all of them at once, also
// ships that are out of each other's radio range. Every tick it predicts each pair a few seconds
// ahead (velocity, or the direction a stopped ship wants to go) and, when two would come closer
// than their size, tells one of them to slow down or hold. Who gives way, the same rules the ships
// use themselves: the ship cleared to dock goes; otherwise the ship closing in on the other gives
// way (the one behind, or the one flying into the other's path); otherwise the higher id. Ships
// meeting head-on pass each other on the right by themselves, as before. The
// choice is kept while the conflict lasts, so it can't flip back and forth. Ships parked or
// hovering without wanting to move are obstacles for the ships' own avoidance, not for this.

const float TCC_HORIZON = 8f; //[s] how far ahead conflicts are predicted

class TCShip
{
    public long id;
    public Vector3 pos, vel, heading;
    public float dia, fw, fh, limit = -1;
    public bool inJob;
    public String job = "";
    public int phase;
    public DateTime time;
}

class TCPair
{
    public long a, b, yielder;
    public DateTime last;
}

List<TCShip> tcShips = new List<TCShip>();
List<TCPair> tcPairs = new List<TCPair>();
int tcSendTick = 0;

// traffic message: id;pos;vel;dia;job;hole;doneHoles;dock,phase;heading;queued;want,footW,footH,inJob
void HandleTrafficControl(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 9) return;
    long id = long.Parse(f[0]);
    TCShip s = null;
    for (int i = 0; i < tcShips.Count; i++) if (tcShips[i].id == id) s = tcShips[i];
    if (s == null) { s = new TCShip(); s.id = id; tcShips.Add(s); }
    s.pos = StringToVector(f[1]);
    s.vel = StringToVector(f[2]);
    s.dia = float.Parse(f[3]);
    s.job = f[4];
    s.phase = int.Parse(f[7].Split(',')[1]);
    s.heading = StringToVector(f[8]);
    s.inJob = false;
    if (f.Length >= 11)
    {
        String[] w = f[10].Split(',');
        if (w.Length >= 4) { s.fw = float.Parse(w[1]); s.fh = float.Parse(w[2]); s.inJob = w[3] == "1"; }
    }
    s.time = DateTime.Now;
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
        if ((DateTime.Now - tcShips[i].time).TotalSeconds > 3) tcShips.RemoveAt(i);
    for (int i = tcPairs.Count - 1; i >= 0; i--)
        if ((DateTime.Now - tcPairs[i].last).TotalSeconds > 2) tcPairs.RemoveAt(i); // conflict over: forget who gave way
    for (int i = 0; i < tcShips.Count; i++) tcShips[i].limit = -1;

    for (int i = 0; i < tcShips.Count; i++)
        for (int j = i + 1; j < tcShips.Count; j++)
        {
            TCShip a = tcShips[i], b = tcShips[j];
            if (TCParked(a) || TCParked(b)) continue;
            Vector3 pa = a.pos + a.vel * (float)(DateTime.Now - a.time).TotalSeconds;
            Vector3 pb = b.pos + b.vel * (float)(DateTime.Now - b.time).TotalSeconds;
            Vector3 ua = TCIntent(a), ub = TCIntent(b);
            Vector3 rel = pb - pa;
            float dist = rel.Length();
            float clear = (a.dia + b.dia) * 0.55f;
            // two ships working the same job drill different holes: only closer than one hole spacing counts
            if (a.inJob && b.inJob && a.job != "" && a.job == b.job)
            {
                String[] k = a.job.Split('|');
                if (k.Length >= 8) clear = Math.Min(float.Parse(k[6]), float.Parse(k[7])) * 0.9f;
            }
            Vector3 ru = ub - ua;
            float ru2 = ru.LengthSquared();
            float t = ru2 > 0.01f ? -Vector3.Dot(rel, ru) / ru2 : 0;
            bool tooClose = dist < clear;
            if (!tooClose && (t <= 0 || t > TCC_HORIZON || (rel + ru * t).Length() >= clear)) continue;
            // head-on: the ships pass each other on the right themselves (holding one would stop it from
            // stepping aside); the controller handles ships catching up and crossing paths
            if (!tooClose && ua.LengthSquared() > 0.25f && ub.LengthSquared() > 0.25f && Vector3.Dot(Vector3.Normalize(ua), Vector3.Normalize(ub)) < -0.7f)
                continue;

            TCPair p = null;
            for (int k = 0; k < tcPairs.Count; k++)
                if ((tcPairs[k].a == a.id && tcPairs[k].b == b.id) || (tcPairs[k].a == b.id && tcPairs[k].b == a.id)) p = tcPairs[k];
            if (p == null) { p = new TCPair(); p.a = a.id; p.b = b.id; p.yielder = TCYielder(a, b, ua, ub, rel, dist); tcPairs.Add(p); }
            p.last = DateTime.Now;
            TCShip y = p.yielder == a.id ? a : b;
            float lim = tooClose || t < 4 ? 0 : Math.Max(1, y.vel.Length() * 0.5f); // close: hold, later: slow down
            if (y.limit < 0 || lim < y.limit) y.limit = lim;
        }
}

long TCYielder(TCShip a, TCShip b, Vector3 ua, Vector3 ub, Vector3 rel, float dist)
{
    if (a.id == clearedShip) return b.id; // cleared to dock by this station
    if (b.id == clearedShip) return a.id;
    Vector3 n = dist > 0.01f ? rel / dist : new Vector3(1, 0, 0);
    float ta = Vector3.Dot(ua, n), tb = Vector3.Dot(ub, -n); // speed towards the other ship
    if (ta - tb > 1) return a.id;
    if (tb - ta > 1) return b.id;
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
