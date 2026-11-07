# PAM - Path Auto Miner (Deobfuscated)

Deobfuscated and modular source code for **[PAM] Path Auto Miner v1.3.1** by Keks.

The original script is published as a single minified/obfuscated file. This project restores it into readable, well-structured C# source files that can be modified, rebuilt, and re-minified for use in Space Engineers programmable blocks.

## Original Script

- **Author:** Keks
- **Steam Workshop:** https://steamcommunity.com/sharedfiles/filedetails/?id=1507646929
- **Guide:** https://steamcommunity.com/sharedfiles/filedetails/?id=1553126390

## Project Structure

```
├── src/                        # Deobfuscated source files (28 modules)
│   ├── 01_Constants.cs         # Constants, version, limits
│   ├── 02_Enums.cs             # All enum definitions
│   ├── 03_Types.cs             # Structs (WaypointInfo, ShipInfo, etc.)
│   ├── 04_Fields.cs            # Field declarations
│   ├── 05_Constructor.cs       # Program() constructor
│   ├── 06_Main.cs              # Main() entry point
│   ├── 07_ShipLoop.cs          # ShipMainLoop - tick processing
│   ├── 08_Commands.cs          # Argument/command processing
│   ├── 09_ShuttleConfig.cs     # Shuttle mode configuration
│   ├── 10_MenuNav.cs           # Menu navigation helpers
│   ├── 11_MenuBuilder.cs       # LCD menu screen builder
│   ├── 12_ConfigHelpers.cs     # Config value adjustment
│   ├── 13_PathRecording.cs     # Waypoint path recording
│   ├── 14_JobControl.cs        # Job start/stop/continue
│   ├── 15_Navigation.cs        # Navigation state machine
│   ├── 16_MiningProgress.cs    # Mining progress tracking
│   ├── 17_Resources.cs         # Battery, uranium, hydrogen
│   ├── 18_InventoryBalance.cs  # Inventory balancing across drills
│   ├── 19_Flight.cs            # Flight controller (UpdateFlight)
│   ├── 20_ThrustGyro.cs        # Thrust vectors, weight calc, gyro
│   ├── 21_NavTargets.cs        # Flight targets, alignment
│   ├── 22_BlockScan.cs         # Block scanning and filtering
│   ├── 23_MathAndSerial.cs     # Math utils, serialization
│   ├── 24_Persistence.cs       # Save/Load to Storage
│   ├── 25_Display.cs           # LCD display, error handling
│   ├── 26_Broadcast.cs         # IGC broadcast system
│   ├── 27_Controller.cs        # Controller mode (multi-ship)
│   ├── 28_UIHelpers.cs         # UI string formatting
│   ├── 29_ObstacleBrake.cs     # Camera raycast obstacle brake (new)
│   ├── 30_Traffic.cs           # Lanes, spacing, co-op mining (new)
│   ├── 31_DockPool.cs          # Ship-to-ship dock pool (new)
│   ├── 32_DockControl.cs       # Station dock control, controller side (new)
│   ├── 33_DockClearance.cs     # Dock clearance, ship side (new)
│   ├── 34_JobCoordinator.cs    # Controller: co-op job record, hole assignment (new)
│   ├── 35_TrafficControl.cs    # Controller: traffic control, staged "Move home" (new)
│   ├── 36_DebugDraw.cs         # Controller: 3D debug drawing via the DebugAPI mod (new)
│   └── 37_Settings.cs          # Settings in the PB's Custom Data (new)
├── tools/
│   ├── build_pam.py            # Combines src/, minifies, writes the ship/controller scripts
│   ├── minify_pam.py           # Minifies into SE-ready script
│   ├── strip_unused.py         # Makes the ship-only / controller-only variants
│   └── validate_minified.py    # Sanity checks + 100,000-char limit
├── PAM_ORIGINAL.cs             # Original obfuscated script (reference)
├── PAM_Combined.cs             # Build output (combined from src/)
├── PAM_Ship.cs                 # Paste into miner / grinder / shuttle PBs
├── PAM_Controller.cs           # Paste into the [PAM-Controller] PB
└── PAM_Minified.cs             # Both modes in one script (only while it fits)
```

## Requirements

- Python 3.8+

## Building

**Step 1: Combine** source files into a single file:

```bash
python tools/build_pam.py
```

This reads all `src/*.cs` files (sorted by numeric prefix) and outputs `PAM_Combined.cs`.

**Step 2: Minify** for Space Engineers:

```bash
python tools/minify_pam.py PAM_Combined.cs --output PAM_Minified.cs --map tools/identifier_map.txt
```

This renames identifiers to short Unicode names, strips whitespace/comments, and produces a script that fits within SE's programmable block limits.

**One-step build + minify:**

```bash
python tools/build_pam.py --minify
```

This writes `PAM_Combined.cs`, `PAM_Minified.cs`, `PAM_Ship.cs` and `PAM_Controller.cs`, and reports how many characters each script has left under the limit.

## Usage in Space Engineers

1. Build the project (see above)
2. Open `PAM_Ship.cs` (ships) or `PAM_Controller.cs` (controller) and copy all contents
3. In Space Engineers, open a Programmable Block
4. Paste the script and click "Check code" then "Remember & Exit"
5. Use the PAM interface on LCD panels tagged with `[PAM]`

For full usage instructions, see the [official PAM guide](https://steamcommunity.com/sharedfiles/filedetails/?id=1553126390).

## Modifications

Edit files in `src/` and rebuild. The modular structure makes it easy to find and modify specific systems:

- **Mining behavior:** `15_Navigation.cs` (Mining state)
- **Flight control:** `19_Flight.cs`, `20_ThrustGyro.cs`
- **Speed/limits:** `01_Constants.cs`
- **Menu/UI:** `11_MenuBuilder.cs`
- **Save/Load format:** `24_Persistence.cs`

**Check the result** (balanced braces, required identifiers, 100,000-character limit; exits non-zero on problems):

```bash
python tools/validate_minified.py PAM_Minified.cs
```

The minifier only renames identifiers the script itself declares, so game/.NET API names are never renamed, and its output is identical on every run.

## Which file to paste

The programmable block limit is 100,000 characters, and one script with both the ship logic and the controller logic no longer leaves room for new features. So the build writes two scripts:

- **`PAM_Ship.cs`** for miners, grinders and shuttles
- **`PAM_Controller.cs`** for the `[PAM-Controller]` PB

Pasting the wrong one is harmless: the PB just says which file it needs. `PAM_Minified.cs` (both modes in one script) no longer fits under the limit and is only kept as a build check.

## Settings

The settings are in the **Custom Data of the programmable block**, in the `[PAM settings]` section below the broadcast settings. The script writes the section with the defaults the first time it runs. To change a setting, edit the value there and press **Recompile**. Delete a line to get its default back. The settings stay when you paste a new version of the script, and a new version adds its new settings by itself. A value that can't be read keeps its default and the PB says "Bad setting, default used: <name>". Each script lists only the settings it uses.

| Setting | Default | Script | Meaning |
|---|---|---|---|
| `pamTag` | `[PAM]` | both | Tag of the PAM PB and its LCDs (`[PAM:<lcdIndex>]`, e.g. `[PAM:1]`) |
| `controllerTag` | `[PAM-Controller]` | both | Tag that makes a PB the controller |
| `dataTag` | `[PAM-Data]` | both | Block whose Custom Data EXPORT/IMPORT use |
| `dockPoolTag` | `[PAM-Dock]` | both | Station connectors ships may use when their home dock is taken |
| `zoneTag` | `[PAM-Zone]` | controller | Sensor over the docking area: nobody is cleared while another ship moves in it |
| `laneTag` | `[PAM-Lane]` | controller | Camera at a connector facing out along its approach: the lane must be clear |
| `dockLaneLength` | 40 | controller | [m] how far the lane cameras check |
| `dockRequestDistance` | 300 | ship | [m] ships ask for clearance this close to home, then wait outside the zone |
| `gyroSpeedSmall` / `gyroSpeedLarge` | 15 / 5 | ship | [RPM] gyro speed of small / large ships |
| `generalSpeedLimit` | 100 | ship | [m/s] 0 = no limit |
| `dockingSpeed` | 0.5 | ship | [m/s] final approach to the connector |
| `jobAreaSpeed` | 10 | ship | [m/s] near the job area (between holes, backing out, arriving, leaving) |
| `dockAreaSpeed` | 5 | ship | [m/s] docking area: inside the `[PAM-Zone]` area, on the way from the path end to the connector, aligning, undocking |
| `dockDist` | 0.6 | both | x ship size: approach point in front of the home connector |
| `followPathDock` / `followPathJob` | 2 / 1 | ship | x ship size: stop following the path and fly straight to the dock / job |
| `useDockDirectionDist` / `useJobDirectionDist` | 1 / 0 | ship | x ship size: use the dock's / job's orientation instead of the waypoint's |
| `wpReachedDist` | 2 | ship | [m] waypoint reached |
| `drillRadius` | 1.4 | both | [m] cut radius of grinders and modded drills (vanilla drills use their real radius) |
| `sensorRange` | 2 | ship | grinding: fly slow when blocks are found in this range |
| `fastSpeed` | 10 | ship | grinding: speed when no blocks are detected |
| `minAccelerationSmall` / `minAccelerationLarge` | 0.2 / 0.1 | both | [m/s²] minimum acceleration in space before the ship counts as too heavy |
| `minEjection` | 25 | ship | [%] minimum ejected cargo to continue the job |
| `setLCDFontAndSize` | true | both | set font and size of the LCDs |
| `obstacleBrake` | true | ship | camera obstacle brake |
| `obstacleLookAhead` | 3 | ship | [s] scan distance = speed x this + ship size |
| `obstacleTimeout` | 10 | ship | [s] show "Path blocked" after holding this long |
| `trafficEnabled` | true | both | traffic between PAM ships (lanes, spacing, right of way, co-op holes) |
| `laneOffset` | 0.75 | ship | x ship size: keep right of the path; 0 = fly on the path |
| `followGap` | 1 | ship | x ship size: free space behind a PAM ship ahead |
| `resumeAfterRestart` | true | ship | continue a running job after a server restart / world load |
| `debugDraw` | false | both | 3D debug drawing (offline world + DebugAPI mod) |

## Multiple ships: obstacle brake, traffic and co-op mining

All three are on by default and can be turned off in the settings (see Settings below).

- **Docking approach:** the camera sweep also runs on the way from the path end to the connector. Anything right at the approach point (the station around the connector) doesn't count. Blocked there for 4 s, the ship tries to go around: one ship size up, right, left and down from the straight line, one after the other, and from the point beside the obstacle straight to the connector again. If none works it stays where it is ("Docking blocked: <type>, stopped").
- **Obstacle brake:** the ship uses its own camera blocks to raycast where it's flying (its velocity once it moves, otherwise towards its flight target), and holds position while something is in the way. Each run casts one ray along the centre of the flight line and one to the next of 8 edge points: the four edges of the ship at the scan distance and at half of it, so blocks only the ship's edge would clip are found too. Each ray comes from the camera that faces that point best and has charge left. The edge rays look for blocks only; terrain is checked on the centre line, so low paths don't trigger it. A full sweep without a hit releases the hold. Hits on the ship's own subgrids (rotors, pistons) are ignored. The info page lists which ship directions the cameras cover; if no camera can look where the ship is going for 2 s, it says "Cameras can't see ahead". It's active during path flights and hole-to-hole moves, and off while mining, backing out of a hole and docking. A camera only scans within 45° of where it faces, so put cameras facing each direction the ship travels: forward, and sideways or up and down for the moves between holes. After `obstacleTimeout` seconds the ship shows "Path blocked: <type>" (also on the controller). Terrain (planet, asteroid) doesn't count on moves between holes (the drills are on and the hole tops sit on the ground), or right at the flight target.
- **Traffic:** PAM ships send a short traffic message about twice a second. This needs a working antenna and is independent of `Broadcast_Channel`. On the recorded path, ships keep right by `laneOffset` × ship size, merging back onto the path near the dock and the job. Ships going opposite ways therefore pass side by side, and a ship meeting another head-on moves further right. A ship slows down or holds behind a PAM ship ahead, keeping `followGap` × ship size of free space. Ships also send the direction they're heading, so ships meeting head-on pass each other even when both have slowed down or stopped. When two ships' paths cross and each has the other ahead, the lower ID goes first. A ship held by traffic for 15 s stops waiting for 10 s and passes on the right ("Traffic: passing a stuck ship"). Avoidance also runs while docking, undocking and backing out of a hole: there the ship doesn't step aside, it slows down and holds behind a PAM ship in its way (on a head-on, the higher ID holds), and it never uses the 15 s escape. In every flight state a ship also stops for a moving PAM ship on a collision course from the side: the higher ID stops, and both stop when it's less than 2 s away. Ships parked at a connector are ignored. Two ships working the same job (moving between holes, mining, backing out) are aligned to it and drill different holes, so between them a ship only counts as in the way when it is closer than one hole spacing; ships at neighbouring holes don't hold each other up. A ship deeper in its hole than half its size is inside the rock and doesn't count for the others at all. Backing out of a hole (also when full or ordered home) uses the same rules.
- **Job site:** near the job area (moving between holes, backing out, arriving and leaving) ships fly at most `jobAreaSpeed` (10 m/s). A ship leaving for home first backs out of its hole to one ship size above the job surface, clear of the ships working there, and then flies to the nearest path waypoint outside the job area before it turns towards the path. A ship flying to a hole that isn't the next one over (also when arriving from the path or coming from the waiting spot) first flies over the hole at least one ship size above the job surface, above the ships working there, then straight down. Only the move to the neighbouring hole stays at the surface. If the ship makes no progress for 5 s (not held by traffic or the cameras): over the hole, the ground is in the way down (terrain higher than the job surface), so it starts the hole where it is, with the drills on; anywhere else a hill is in the way, so it goes over it, one ship size higher. A ship heading home that isn't in a hole (e.g. low on energy while waiting) goes straight to the path.
- **Job grid:** the hole spacing is the drill area the job was set up with (saved with the job and copied by IMPORT JOB), so a ship that loses or gains a drill keeps the same grid as the others; changing the overlap sets it again. Each job setup gets its own number in the co-op job key, so a job set up again at the same spot is a new job, not an already finished one.
- **Moving aside:** a ship in free flight that has stopped to give way to a ship coming through where it is moves slowly (2 m/s) sideways out of that ship's line; the camera brake stops it if there is no room on that side. Not while docking, undocking or in a hole, and not for a ship on a collision course from the side (then it just stops).
- **Right of way:** at the job site ships leaving for home go first, then ships moving between holes or arriving; the others give way (hold instead of passing). Between ships with the same right of way (anywhere, also on the path), the ship closer to its next checkpoint goes first: its next path waypoint, or the path waypoint it is flying to when it isn't on the path yet; about the same distance: the lower ID. The controller uses the same order.
- **Traffic control by the controller:** a `[PAM-Controller]` PB that hears the ships watches all of them at once (also ships out of each other's radio range). Twice a second it predicts every pair 8 s ahead, using each ship's velocity or, for a stopped ship, the direction it wants to go. When two would come closer than their size, one of them slows down, then holds ("Traffic: giving way (controller)"). Who gives way: the ship cleared to dock goes first; otherwise the ship closing in on the other (the one behind, or the one flying into the other's path); otherwise the higher ID. The choice is kept while the conflict lasts. Ships meeting head-on still pass each other on the right by themselves. Ships parked or hovering without wanting to move are left to the ships' own avoidance. The controller's order applies on top of the ship's own avoidance, so without a controller nothing changes. While docking, undocking or backing out of a hole, a ship now holds for every ship in its way, including head-on.
- **Dock pool:** name base connectors with `[PAM-Dock]`. A ship still docks at its own recorded home whenever it's free. If another PAM ship is docked there, or about to dock, it takes the nearest free `[PAM-Dock]` connector, or waits at the end of the path ("Waiting for a free dock"). The ship learns the pool while it's docked at the base, so it has to dock there once with this version first. Pool connectors must be the same type as the home connector, and each needs a clear straight line from the end of the path. The base must not move. Shuttles keep their two fixed docks.
- **Station dock control:** for a busy base, put a PB tagged `[PAM-Controller]` (with `PAM_Controller.cs` and an antenna) on the station. It clears one ship at a time to move in the docking area; the others wait outside it ("Waiting for dock clearance") or docked ("Waiting for undock clearance"). Ships ask for clearance within `dockRequestDistance` (300 m) of their home dock. With a `[PAM-Zone]` sensor, a ship that isn't cleared stops at the last path waypoint before the sensor area and turns towards its dock there; other ships heading home line up behind it (the one closer to the waiting point goes first), and ships leaving or already cleared pass the queue on the right. A cleared ship keeps the dock orientation and approach speed inside the area. Without a sensor, ships wait at the end of their path. Ships waiting to undock go first, so a full dock area can't jam. The station picks the dock from the real connector status, so it also sees idle ships and ships that don't run PAM. A connector that shows *Connectable* (a ship hovering at it) counts as taken. Optional blocks on the station: a sensor named `[PAM-Zone]` sized to the docking area (while any other ship moves inside it, nobody is cleared; ships wait outside a sphere around the sensor that contains its whole detection area, so keep the extents close to the docking area), and a camera named `[PAM-Lane]` within 10 m of each connector, facing out along the approach (if its ray, `dockLaneLength` m long, hits anything but the station or the ship asking, that connector isn't used). The station only answers ships whose home connector is on its grid; if no station answers within 3 s the ship uses the dock pool rules above. A cleared ship that hasn't docked or left after 120 s loses its clearance and goes to the back of the queue. The controller's main screen shows the docking-area status.
- **Co-op mining:** set up the job on one ship, then `EXPORT` it. On the other ships, run `IMPORT JOB` (this keeps their own path and home dock) and then `CONT`. Ships on the same job take holes in the normal mining order, each one the next hole no other ship has. The next ship takes the hole right beside the other one. A ship keeps its hole while it is away unloading, and the ships share which holes are finished. When every hole left is taken by other ships, the ship waits on a ring 2.5 ship sizes above the job centre, each ship on its own place of the ring, above the ships working or passing ("Waiting for a free hole"), and takes the next hole that frees up. It returns home if its energy runs low while waiting. If a `[PAM-Controller]` PB (with `PAM_Controller.cs`) hears the ships, it keeps the master record of the job and hands out the holes: it remembers finished holes and each ship's hole, so a ship keeps its hole even while it's out of radio range unloading (dropped after 10 minutes without a message). A full IMPORT doesn't copy the exporting ship's claimed hole, and a ship drops a hole of its own when the controller has given it to another ship. When a ship moves to another job it leaves the old job's record on the controller, so the old job disappears from the list. A ship asking for a hole waits up to 5 s for the controller ("Waiting for the controller"), then picks one itself as above. A ship the controller can't hear (its antenna doesn't reach the station, even if it hears the station) picks its holes itself right away. The controller's main screen shows each job's progress. For the same job, the ships need the same drill layout. A single ship mines in the original order.

## Changes from the original 1.3.1

Behaviour follows the original script (`PAM_ORIGINAL.cs`). Fixes on top of it:

- **Controller** (`[PAM-Controller]` PB): ship list, ship detail, send-command menus, inventory view and per-LCD modes (`mode=main:<page>`, `mainX`, `menu`, `inventory`, `menu:<ship>`, `inventory:<ship>`) are fully ported.
- **Weight limit on long paths:** the zero-gravity waypoint used by the max-weight calculation is a separate field again (it had been merged with path-following's current waypoint, which affected paths over ~100 waypoints).
- **Broadcasts:** every queued message is processed each run (one per run fell behind with ~10+ ships); non-text IGC messages on the PAM tag are ignored instead of crashing; `;` in ship names, status or menu text no longer shifts the fields the controller reads.
- **Saves:** timer block names containing `;`, `&` or line breaks no longer corrupt the saved data (escaped as `%3B` / `%0A` / `%25`). The save format is unchanged, so existing saves still load. `Save()` builds the data with a `StringBuilder`.
- **Server restart / world load:** the dampeners are switched back on when the script starts. The port assumed they were already on, so a ship saved mid-flight (dampeners off) dropped as soon as its thrust overrides were cleared. A job that was running when the world was saved now resumes automatically. Set `resumeAfterRestart = false` in the settings block to stay paused like the original.
- **Obstacle brake, traffic lanes, spacing, co-op mining and dock pool:** see the section above. Saves now have two more sections at the end (co-op holes, dock pool); older saves still load.
- **EXPORT / IMPORT:** `EXPORT` copies the path, home dock, job position, mining area and settings into the Custom Data of a block named `[PAM-Data]`; `IMPORT` loads them back (a running job comes back paused). Use it as a backup, or to copy a setup to an identical ship. The game limits Custom Data to 64,000 characters, about 400 waypoints; a bigger export is refused with a message instead of being cut off.
- **Errors:** after an exception, run the PB with the argument `RETRY` to restart without recompiling.
- **Move home for all ships** (controller, *Send command to all*): every ship gets the order at once (so none starts a new hole), and the controller lets them leave one at a time, holding the others where they are (in their holes, or hovering). Ships already out of the job area go first, then ships at the surface, then ships still deep in their holes; in each group the ship closest to the job position (where the path starts) goes first. The next ship may leave when the previous one has moved 3 ship sizes away (at least 40 m), right away if the previous one was docked, or after 45 s at the latest. The command result shows which ships are still waiting to leave. Any other command cancels the queue.
- **Debug drawing in 3D** (offline worlds): add Digi's *(DevTool) Programmable Block DebugAPI* mod (Workshop 2654858862) to an offline world, and set `debugDraw=true` in the [PAM settings] section of the Custom Data of the controller PB and of the ships (then Recompile). The mod doesn't work in multiplayer: on a server it does nothing, so leave `debugDraw` off there. The controller then draws, refreshed twice a second:
  - every PAM ship as a wire sphere of its size: green moving, yellow giving way, grey parked. A white line shows where it will be in 3 s, a cyan line the way it wants to go.
  - a red line between two ships the controller sees on a collision course.
  - from the ships: a blue line to the flight target, the camera rays (green clear, red hit), and each job's hole grid: grey done, orange claimed (with a line from the ship to its hole), white free.
  - the docking area: `[PAM-Zone]` spheres, `[PAM-Lane]` camera rays (purple), and a green line from the ship cleared to dock to its connector.
  - text labels (HUD markers): each ship's name and state (moving, slowing/holding while giving way, parked), each conflict with who gives way, each claimed hole with its ship, the job's progress, the docking area with its status, and the ship cleared to dock or undock.
  - the colour key, and whether the mod was found, are in the controller PB's output (the text on the right when you select the PB in the terminal).
- **Controller command feedback:** the result of a command (received / pending / no answer) is shown only on the page it was sent from and disappears after 20 s, with one line per ship it went to.
- **Controller arguments:** `SEND` / `SENDALL` without a target or command show usage instead of crashing.
- Scan interval uses total elapsed seconds (was the 0-59 seconds component).
- **Drill area:** vanilla drills use their real cut radius from the block definitions (small 1.3 m, large 1.9 m, large Prototech 6 m) instead of 1.4 m for every drill; grinders and modded drills still use `drillRadius`. The info page shows the drill area and the ship's size across the drill face.

## Identifier Map

The minifier generates `tools/identifier_map.txt` which maps readable names to their minified equivalents. This is useful for debugging minified output:

```
stuckCounter -> π
distToTarget -> Ѝ
UpdateFlight -> ą
...
```

## License

The original PAM script is by Keks (Steam Workshop). This deobfuscation is provided for educational and modding purposes.
