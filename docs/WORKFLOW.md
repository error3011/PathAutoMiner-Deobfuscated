# PAM workflow

How a miner, the station controller and the other ships work together in the current version.
Diagrams are Mermaid (they render on GitHub and in VS Code with a Mermaid preview).

## 1. Miner cycle

```mermaid
flowchart TD
    D([Docked: Unloading / Charging / Hydrogen / Uranium]) -->|undock trigger met| U0{Station controller?}
    U0 -->|yes| UW[Wait for undock clearance<br/>granted only when nobody is docking]
    U0 -->|no| U1
    UW --> U1[Undocking: connector off,<br/>back out straight to the approach point,<br/>on along the lane to the lane entry]
    U1 --> JA[FlyToJobArea: follow path<br/>lane offset, cameras, traffic]
    JA --> XY[FlyToXY: claim a hole]
    XY -->|controller assigns / peers' claims| H{Free hole?}
    H -->|none yet| WR[Wait on the ring above the job<br/>own spot by rank; right of way -1]
    WR --> XY
    H -->|yes| FX[Fly to hole top, drills OFF<br/>not adjacent: approach from above<br/>10 m/s cap near the job]
    FX -->|stuck 5 s: ground in the way| M
    FX -->|at hole top, lined up| M[Mining: drills ON, down to depth]
    M -->|hole done, cargo ok| R1[Returning: back out, rise one ship size]
    R1 --> XY
    M -->|full / low energy / job done / HOMEPOS| RD[ReturnToDock: back out of the hole]
    RD --> FP[FlyToPath: join the path<br/>skips waypoints inside the job area]
    FP --> DA[FlyToDockArea: path home]
    DA --> DOCK[[Docking flow, section 2]]
    DOCK --> D
```

## 2. Docking with a station controller

```mermaid
flowchart TD
    A[On the path home] -->|within 300 m of home| RQ[Ask the station: R message<br/>usable pool connectors, bad connectors, emergency flag]
    RQ --> CL{Cleared?}
    CL -->|no, next waypoint inside the zone box| Q{Emergency?<br/>low energy heading home}
    Q -->|no| QH[Hold at the last waypoint outside<br/>the PAM-Zone sensor box, facing the way it flew<br/>queue flag 1; leaving / cleared ships pass right]
    Q -->|yes| EL[Come down 3 m/s straight below<br/>landed: gears lock, thrusters OFF<br/>parked flag: others pass]
    QH --> CL
    EL --> CL
    CL -->|yes| TO{Was landed?}
    TO -->|yes| TK[Thrusters ON, climb back to where it came down<br/>at least one ship size]
    TO -->|no| IN
    TK --> IN[Into the docking area, facing the flight direction<br/>pool connector: leave the path before it nears home]
    IN --> AP[ApproachDock: right of way 3<br/>in the lane: holds only, never steps aside]
    AP -->|connector already connectable / connected| DK
    AP -->|off the lane line| LE[Fly to the lane entry<br/>one ship size beyond the approach point<br/>holds heading in the last 5 m]
    AP -->|on the lane line| LT
    LE --> LT[Turn round to the dock attitude<br/>at most 8 s]
    LT --> SI[Back straight in to the approach point<br/>phase 2 = station may clear the next ship]
    SI --> AL[AlignDock: precise alignment]
    AL --> DK[Docking: lock connector]
    DK -->|locked| DONE([Docked: D message, station frees the request])
    DK -->|no lock| RT[RetryDocking: back to the approach point]
    RT -->|fewer than 3 tries| AL
    RT -->|3 tries| BD[Connector on the bad list<br/>station drops that clearance,<br/>ship keeps its queue place]
    BD --> CL
```

Station side, every run:

```mermaid
flowchart LR
    S1[Requests from ships<br/>expire after 3 s silence] --> S2{Zone busy?<br/>another ship moving in the sensor box}
    S2 -->|yes| S1
    S2 -->|no| P3[Pass 3: emergency docking<br/>to a free, usable, not-bad connector]
    P3 -->|none| P2[Pass 2: undock<br/>only when nobody is docking]
    P2 -->|none| P1[Pass 1: oldest docking request<br/>home connector first, else nearest usable pool connector,<br/>lane camera clear]
    P3 --> C[Clearance: ship + connector]
    P2 --> C
    P1 --> C
    C -->|ship reports phase 2 = turned round in its lane| DL[Docking list: keeps the connector,<br/>next ship may be cleared]
    C -->|120 s without docking| S1
```

## 3. Right of way between ships

```mermaid
flowchart TD
    P[Two ships in each other's way] --> L{Right-of-way level<br/>as last broadcast}
    L -->|different| HI[Higher level goes:<br/>3 emergency or docking/undocking > 2 heading home ><br/>1 between holes / arriving > 0 other > -1 waiting]
    L -->|same| DD{Distance to destination<br/>whole metres, as last broadcast<br/>to join the road + along the path}
    DD -->|different| CL[Closer goes]
    DD -->|same| ID[Lower ID goes]
    HI --> ACT
    CL --> ACT
    ID --> ACT
    ACT[The other one:] --> K{Situation}
    K -->|head-on| HO[Both steer to a point beside each other<br/>where they meet, slow until beside]
    K -->|crossing / collision course| CP[Gives way: holds;<br/>both stop if under 2 s]
    K -->|same direction| FO[Follows at a distance]
    K -->|stopped in the other's line| SA[Moves aside at 2 m/s<br/>until out of the line]
    K -->|other waiting / parked| PS[Passes it on the right at once<br/>unless it waits where I'm going]
    CP -->|held 15 s| ES[Passes on the right for 10 s<br/>still stops for a ship under 2 s away]
```

The controller sees every ship and predicts conflicts with the same order (for two ships going
the same way, the one catching up gives way). It sends speed limits to the ship that should give
way. Its decision for a pair is kept while the conflict lasts, unless one of the two changes level.
Cameras run all the time in free flight. They brake for anything in the way: other PAM ships only
when right ahead within one ship size, plus PAM ships parked at connectors.
