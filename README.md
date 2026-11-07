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
│   └── 33_DockClearance.cs     # Dock clearance, ship side (new)
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

## Multiple ships: obstacle brake, traffic and co-op mining

All three are on by default and can be turned off in the settings block at the top of the script.

- **Obstacle brake:** the ship uses its own camera blocks to raycast towards where it's flying, and holds position while something is in the way. It's active during path flights and hole-to-hole moves, and off while mining, backing out of a hole and docking. A camera only scans within 45° of where it faces, so put cameras facing each direction the ship travels: forward, and sideways or up and down for the moves between holes. After `obstacleTimeout` seconds the ship shows "Path blocked: <type>" (also on the controller).
- **Traffic:** PAM ships send a short traffic message about twice a second. This needs a working antenna and is independent of `Broadcast_Channel`. On the recorded path, ships keep right by `laneOffset` × ship size, merging back onto the path near the dock and the job. Ships going opposite ways therefore pass side by side, and a ship meeting another head-on moves further right. A ship slows down or holds behind a PAM ship ahead, keeping `followGap` × ship size of free space. Ships also send the direction they're heading, so ships meeting head-on pass each other even when both have slowed down or stopped. When two ships' paths cross and each has the other ahead, the lower ID goes first. A ship held by traffic for 15 s stops waiting for 10 s and passes on the right ("Traffic: passing a stuck ship"). Avoidance also runs while docking, undocking and backing out of a hole: there the ship doesn't step aside, it slows down and holds behind a PAM ship in its way (on a head-on, the higher ID holds), and it never uses the 15 s escape. In every flight state a ship also stops for a moving PAM ship on a collision course from the side: the higher ID stops, and both stop when it's less than 2 s away. Ships parked at a connector are ignored. Two ships working the same job (moving between holes, mining, backing out) are both aligned to the job, so between them only their size across the drill face counts, not the bounding sphere; ships at nearby holes no longer hold each other up.
- **Traffic control by the controller:** a `[PAM-Controller]` PB that hears the ships watches all of them at once (also ships out of each other's radio range). Twice a second it predicts every pair 8 s ahead, using each ship's velocity or, for a stopped ship, the direction it wants to go. When two would come closer than their size, one of them slows down, then holds ("Traffic: giving way (controller)"). Who gives way: the ship cleared to dock goes first; otherwise the ship closing in on the other (the one behind, or the one flying into the other's path); otherwise the higher ID. The choice is kept while the conflict lasts. Ships meeting head-on still pass each other on the right by themselves. Ships parked or hovering without wanting to move are left to the ships' own avoidance. The controller's order applies on top of the ship's own avoidance, so without a controller nothing changes. While docking, undocking or backing out of a hole, a ship now holds for every ship in its way, including head-on.
- **Dock pool:** name base connectors with `[PAM-Dock]`. A ship still docks at its own recorded home whenever it's free. If another PAM ship is docked there, or about to dock, it takes the nearest free `[PAM-Dock]` connector, or waits at the end of the path ("Waiting for a free dock"). The ship learns the pool while it's docked at the base, so it has to dock there once with this version first. Pool connectors must be the same type as the home connector, and each needs a clear straight line from the end of the path. The base must not move. Shuttles keep their two fixed docks.
- **Station dock control:** for a busy base, put a PB tagged `[PAM-Controller]` (with `PAM_Controller.cs` and an antenna) on the station. It clears one ship at a time to move in the docking area; the others wait outside it ("Waiting for dock clearance") or docked ("Waiting for undock clearance"). Ships ask for clearance within `dockRequestDistance` (300 m) of their home dock. With a `[PAM-Zone]` sensor, a ship that isn't cleared stops at the last path waypoint before the sensor area and turns towards its dock there; other ships heading home line up behind it (the one closer to the waiting point goes first), and ships leaving or already cleared pass the queue on the right. A cleared ship keeps the dock orientation and approach speed inside the area. Without a sensor, ships wait at the end of their path. Ships waiting to undock go first, so a full dock area can't jam. The station picks the dock from the real connector status, so it also sees idle ships and ships that don't run PAM. A connector that shows *Connectable* (a ship hovering at it) counts as taken. Optional blocks on the station: a sensor named `[PAM-Zone]` sized to the docking area (while any other ship moves inside it, nobody is cleared; ships wait outside a sphere around the sensor that contains its whole detection area, so keep the extents close to the docking area), and a camera named `[PAM-Lane]` within 10 m of each connector, facing out along the approach (if its ray, `dockLaneLength` m long, hits anything but the station or the ship asking, that connector isn't used). The station only answers ships whose home connector is on its grid; if no station answers within 3 s the ship uses the dock pool rules above. A cleared ship that hasn't docked or left after 120 s loses its clearance and goes to the back of the queue. The controller's main screen shows the docking-area status.
- **Co-op mining:** set up the job on one ship, then `EXPORT` it. On the other ships, run `IMPORT JOB` (this keeps their own path and home dock) and then `CONT`. Ships on the same job take holes in the normal mining order, each one the next hole no other ship has. A neighbouring hole is skipped only when the ship is wider than the area it drills, so the ships would touch. A ship keeps its hole while it is away unloading, and the ships share which holes are finished. When every hole left is taken by other ships (or too close to them), the ship waits beside the job area, one ship size back from the surface, out of the other ships' way ("Waiting for a free hole"), and takes the next hole that frees up. It returns home if its energy runs low while waiting. If a `[PAM-Controller]` PB (with `PAM_Controller.cs`) hears the ships, it keeps the master record of the job and hands out the holes: it remembers finished holes and each ship's hole, so a ship keeps its hole even while it's out of radio range unloading (dropped after 10 minutes without a message). A ship asking for a hole waits up to 5 s for the controller ("Waiting for the controller"), then picks one itself as above. A ship the controller can't hear (its antenna doesn't reach the station, even if it hears the station) picks its holes itself right away. The controller's main screen shows each job's progress. For the same job, the ships need the same drill layout. A single ship mines in the original order.

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
