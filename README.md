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
│   └── 28_UIHelpers.cs         # UI string formatting
├── tools/
│   ├── build_pam.py            # Combines src/ into PAM_Combined.cs
│   └── minify_pam.py           # Minifies into SE-ready script
├── PAM_ORIGINAL.cs             # Original obfuscated script (reference)
├── PAM_Combined.cs             # Build output (combined from src/)
└── PAM_Minified.cs             # Final minified script for SE
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

## Usage in Space Engineers

1. Build the project (see above)
2. Open `PAM_Minified.cs` and copy all contents
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

## Changes from the original 1.3.1

Behaviour follows the original script (`PAM_ORIGINAL.cs`). Fixes on top of it:

- **Controller** (`[PAM-Controller]` PB): ship list, ship detail, send-command menus, inventory view and per-LCD modes (`mode=main:<page>`, `mainX`, `menu`, `inventory`, `menu:<ship>`, `inventory:<ship>`) are fully ported.
- **Weight limit on long paths:** the zero-gravity waypoint used by the max-weight calculation is a separate field again (it had been merged with path-following's current waypoint, which affected paths over ~100 waypoints).
- **Broadcasts:** every queued message is processed each run (one per run fell behind with ~10+ ships); non-text IGC messages on the PAM tag are ignored instead of crashing; `;` in ship names, status or menu text no longer shifts the fields the controller reads.
- **Saves:** timer block names containing `;` or line breaks no longer corrupt the saved data. The save format is unchanged, so existing saves still load. `Save()` builds the data with a `StringBuilder`.
- **Server restart / world load:** the dampeners are switched back on when the script starts. The port assumed they were already on, so a ship saved mid-flight (dampeners off) dropped as soon as its thrust overrides were cleared. A job that was running when the world was saved now resumes automatically. Set `resumeAfterRestart = false` in the settings block to stay paused like the original.
- **Errors:** after an exception, run the PB with the argument `RETRY` to restart without recompiling.
- **Controller arguments:** `SEND` / `SENDALL` without a target or command show usage instead of crashing.
- Scan interval uses total elapsed seconds (was the 0-59 seconds component).

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
