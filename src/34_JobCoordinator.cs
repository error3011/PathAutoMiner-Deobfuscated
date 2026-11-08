// ========================== JOB COORDINATION (station, [PAM-Controller] PB) ==========================
// The controller listens to the ships' traffic messages and keeps the master record of each co-op
// job: finished holes and which ship has which hole. A ship that needs a hole sets a want flag;
// the controller assigns the next free hole in the mining order and broadcasts the job state twice a second. Claims are kept while a ship is out of
// range (e.g. unloading far away) and dropped after JOB_SHIP_TIMEOUT without a message.

const float JOB_SHIP_TIMEOUT = 600f; //[s]

class JobShip
{
    public long id;
    public int cell = -1; // -1 none, -2 nothing left for it
    public bool want;
    public float fw, fh;
    public DateTime seen;
}

class JobRec
{
    public String key;
    public int w, h;
    public float tw, th;
    public String done;
    public List<int> order = new List<int>();
    public List<JobShip> ships = new List<JobShip>();
}

List<JobRec> jobs = new List<JobRec>();
IMyBroadcastListener jobTrafficListener = null;
int jobSendTick = 0;

void UpdateJobCoordination()
{
    if (jobTrafficListener == null) jobTrafficListener = IGC.RegisterBroadcastListener(TRAFFIC_TAG);
    int budget = 50;
    while (jobTrafficListener.HasPendingMessage && budget-- > 0)
    {
        String data = jobTrafficListener.AcceptMessage().Data as String;
        if (data == null) continue;
        try { HandleJobTraffic(data); } catch { }
        try { HandleTrafficControl(data); } catch { }
    }
    for (int j = jobs.Count - 1; j >= 0; j--)
    {
        JobRec job = jobs[j];
        for (int i = job.ships.Count - 1; i >= 0; i--)
            if ((TimeNow() - job.ships[i].seen).TotalSeconds > JOB_SHIP_TIMEOUT) job.ships.RemoveAt(i);
        if (job.ships.Count == 0) { jobs.RemoveAt(j); continue; }
        AssignJobHoles(job);
    }
    if (++jobSendTick < 3) return; // ~2 per second
    jobSendTick = 0;
    for (int j = 0; j < jobs.Count; j++) IGC.SendBroadcastMessage(JOB_TAG, BuildJobMessage(jobs[j]));
}

void AssignJobHoles(JobRec job)
{
    for (int i = 0; i < job.ships.Count; i++)
    {
        JobShip s = job.ships[i];
        int was = s.cell;
        if (s.want && (s.cell < 0 || job.done[s.cell] == '1')) s.cell = NextFreeHole(job, s);
        if (s.cell != was && s.cell >= 0) AddLog("Job: " + ShipName(s.id) + " -> hole " + (job.order.IndexOf(s.cell) + 1));
    }
}

String BuildJobMessage(JobRec job)
{
    String msg = "J;" + job.key + ";" + job.done + ";";
    // only ships heard just now: a ship missing from the list knows the controller can't hear it
    // (it may still hear the controller) and picks its holes itself instead of waiting
    bool first = true;
    for (int i = 0; i < job.ships.Count; i++)
    {
        if ((TimeNow() - job.ships[i].seen).TotalSeconds > 5) continue;
        msg += (first ? "" : ",") + job.ships[i].id + ":" + job.ships[i].cell;
        first = false;
    }
    return msg;
}

// traffic message: id;pos;vel;dia;job;hole;doneHoles;dock,phase;heading;queued;want,footW,footH
void HandleJobTraffic(String data)
{
    String[] f = data.Split(';');
    if (f.Length < 11 || f[4] == "") return; // no job, or a build without coordination
    String[] k = f[4].Split('|');
    if (k.Length < 8) return;
    JobRec job = null;
    for (int i = 0; i < jobs.Count; i++) if (jobs[i].key == f[4]) job = jobs[i];
    bool fresh = job == null; // just heard of this job (e.g. controller restarted): no log line per finished hole
    if (job == null)
    {
        job = new JobRec();
        job.key = f[4];
        job.w = int.Parse(k[3]); job.h = int.Parse(k[4]);
        job.tw = float.Parse(k[6]); job.th = float.Parse(k[7]);
        if (job.w < 1 || job.h < 1) return;
        BuildHoleOrder(job.w, job.h, (StartPosition)int.Parse(k[5]), job.order);
        job.done = new String('0', job.w * job.h);
        jobs.Add(job);
    }
    // finished holes reported by the ship
    String d = f[6];
    if (d.Length == job.done.Length)
    {
        char[] c = job.done.ToCharArray();
        for (int i = 0; i < c.Length; i++)
            if (d[i] == '1' && c[i] != '1')
            {
                c[i] = '1';
                if (fresh) continue;
                int n = 0;
                for (int q = 0; q < c.Length; q++) if (c[q] == '1') n++;
                AddLog(n == c.Length ? "Job: all " + n + " holes done" : "Job: hole " + (job.order.IndexOf(i) + 1) + " done (" + n + "/" + c.Length + ")");
            }
        job.done = new String(c);
    }
    long id = long.Parse(f[0]);
    // the ship works this job now: drop it from any other job record (an old job then disappears)
    for (int j = 0; j < jobs.Count; j++) if (jobs[j] != job) jobs[j].ships.RemoveAll(o => o.id == id);
    JobShip s = null;
    for (int i = 0; i < job.ships.Count; i++) if (job.ships[i].id == id) s = job.ships[i];
    if (s == null) { s = new JobShip(); s.id = id; job.ships.Add(s); }
    s.seen = TimeNow();
    String[] w = f[10].Split(',');
    s.want = w[0] == "1";
    if (w.Length >= 3) { s.fw = float.Parse(w[1]); s.fh = float.Parse(w[2]); }
    // the ship's own claim (picked without the controller, or after a controller restart): keep it
    // unless another ship already has that hole
    int hole = int.Parse(f[5]);
    if (hole >= 0 && hole < job.done.Length && (s.cell < 0 || job.done[s.cell] == '1') && job.done[hole] != '1' && HoleOwner(job, hole, s) == null)
        s.cell = hole;
}

JobShip HoleOwner(JobRec job, int cell, JobShip except)
{
    for (int i = 0; i < job.ships.Count; i++)
    {
        JobShip o = job.ships[i];
        if (o != except && o.cell == cell)
            return o;
    }
    return null;
}

int NextFreeHole(JobRec job, JobShip s)
{
    for (int k = 0; k < job.order.Count; k++)
    {
        int c = job.order[k];
        if (job.done[c] != '1' && HoleOwner(job, c, s) == null) return c;
    }
    return -2;
}

String GetJobLines()
{
    String s = "";
    for (int j = 0; j < jobs.Count; j++)
    {
        JobRec job = jobs[j];
        int done = 0;
        for (int i = 0; i < job.done.Length; i++) if (job.done[i] == '1') done++;
        s += "Job " + (j + 1) + ": " + done + "/" + job.done.Length + " holes, " + job.ships.Count + " ships\n";
    }
    return s;
}
