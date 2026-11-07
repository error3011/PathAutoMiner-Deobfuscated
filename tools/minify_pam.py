#!/usr/bin/env python3
"""
PAM C# Minifier for Space Engineers Programmable Blocks.

Reads readable C# source, renames identifiers to short Unicode names,
strips comments/whitespace, and outputs SE-ready minified code.

Usage:
    python minify_pam.py PAM_Deobfuscated.cs --output PAM_Minified.cs
"""

import re
import sys
import argparse
import itertools

# ─── Unicode pools for short identifier names ────────────────────────────
# Only ranges verified to work in Space Engineers programmable blocks.
# The original PAM obfuscation used Greek + Cyrillic + Latin Extended.
# IPA Extensions, Latin Extended Additional, and Cyrillic Supplement are
# excluded — SE's font/runtime may not fully support them.
UNICODE_POOLS = [
    list(range(0x0391, 0x03D0)),   # Greek (safe - used by original PAM)
    list(range(0x0400, 0x0460)),   # Cyrillic (safe - used by original PAM)
    list(range(0x01CD, 0x0234)),   # Latin Extended-B (safe)
    list(range(0x0100, 0x01CD)),   # Latin Extended-A (safe)
]

def build_name_pool():
    """Generate pool of short identifier names from Unicode ranges."""
    pool = []
    chars = []
    for r in UNICODE_POOLS:
        for cp in r:
            c = chr(cp)
            if c.isalpha():
                chars.append(c)
    # Single char names first
    for c in chars:
        pool.append(c)
    # Two char combos
    for a in chars[:80]:
        for b in chars[:80]:
            pool.append(a + b)
    return pool

# ─── C# keywords (never rename) ──────────────────────────────────────────
CS_KEYWORDS = {
    "abstract","as","base","bool","break","byte","case","catch","char","checked",
    "class","const","continue","decimal","default","delegate","do","double","else",
    "enum","event","explicit","extern","false","finally","fixed","float","for",
    "foreach","goto","if","implicit","in","int","interface","internal","is","lock",
    "long","namespace","new","null","object","operator","out","override","params",
    "private","protected","public","readonly","ref","return","sbyte","sealed","short",
    "sizeof","stackalloc","static","string","struct","switch","this","throw","true",
    "try","typeof","uint","ulong","unchecked","unsafe","ushort","using","var",
    "virtual","void","volatile","while","String","Int32","Single","Double","Boolean",
    "Math","Array","Enum","List","Dictionary","DateTime","Exception","Console",
}

# ─── SE API names (never rename) ──────────────────────────────────────────
SE_API = {
    # Types
    "IMyTerminalBlock","IMyFunctionalBlock","IMyCubeBlock","IMySlimBlock",
    "IMyShipController","IMyRemoteControl","IMyCockpit",
    "IMyThrust","IMyGyro","IMyShipConnector","IMyShipDrill","IMyShipGrinder",
    "IMySensorBlock","IMyLandingGear","IMyReactor","IMyBatteryBlock",
    "IMyConveyorSorter","IMyGasTank","IMyCargoContainer","IMyTimerBlock",
    "IMyRadioAntenna","IMyTextPanel","IMyTextSurface","IMyTextSurfaceProvider",
    "IMyGridTerminalSystem","IMyInventory","IMyBroadcastListener",
    "IMyCubeGrid","IMyProgrammableBlock",
    # Structs/Classes
    "Vector3","Vector3D","Vector3I","MatrixD","MyInventoryItem",
    "MyIGCMessage","MyDetectedEntityInfo","MyCubeSize","ContentType",
    "BoundingSphereD",
    # Enums
    "MyShipConnectorStatus","ChargeMode","UpdateType","UpdateFrequency",
    "MyDetectedEntityType",
    # Enum values
    "Connected","Connectable","Unconnected",
    "Recharge","Discharge","Auto",
    "Update1","Update10","Update100",
    "SmallGrid","LargeGrid","Small","Large",
    "TEXT_AND_IMAGE",
    "None",
    # Properties/Methods that appear after . (protected contextually)
    "WorldMatrix","Forward","Backward","Left","Right","Up","Down",
    "CubeGrid","GetPosition","Position","WorldVolume","Radius",
    "CenterOfMass","GetShipSpeed","GetShipVelocities","LinearVelocity",
    "GetNaturalGravity","TryGetPlanetPosition",
    "CalculateShipMass","PhysicalMass",
    "CustomName","CustomData","BlockDefinition","SubtypeId","TypeId",
    "IsFunctional","Enabled","ShowOnHUD",
    "MaxThrust","MaxEffectiveThrust",
    "GyroOverride","GyroPower",
    "Status","OtherConnector","ThrowOut","Connect",
    "LastDetectedEntity","IsEmpty","Type",
    "MaxStoredPower","CurrentStoredPower","CurrentInput","CurrentOutput",
    "FilledRatio","Stockpile",
    "LeftExtend","RightExtend","TopExtend","BottomExtend","FrontExtend","BackExtend",
    "DetectFloatingObjects","DetectAsteroids","DetectLargeShips","DetectSmallShips",
    "DetectStations","DetectOwner","DetectSubgrids","DetectPlayers",
    "DetectEnemy","DetectFriendly","DetectNeutral",
    "DampenersOverride","AutoLock","Lock","Unlock",
    "EnableBroadcasting","Trigger",
    "GridSizeEnum","GridTerminalSystem",
    "GetBlocksOfType","SearchBlocksOfName","GetBlockWithName",
    "GetCubeBlock","FatBlock",
    "GetInventory","InventoryCount","MaxVolume","CurrentVolume",
    "GetItems","TransferItemTo","IsConnectedTo",
    "Amount","GetActionWithName","Apply",
    "SetValueFloat",
    "RegisterBroadcastListener","SetMessageCallback","DisableBroadcastListener",
    "SendBroadcastMessage","HasPendingMessage","AcceptMessage","Data",
    "ContentType","Font","FontSize","WriteText",
    "GetSurface","SurfaceCount",
    "IsUnderControl",
    # Programmable block
    "Program","Main","Save","Echo","Storage","Runtime","Me","IGC",
    "UpdateFrequency","CurrentInstructionCount","MaxInstructionCount",
    "LastRunTimeMs","GetId",
    # .NET
    "ToString","ToUpper","ToLower","Contains","StartsWith","IndexOf",
    "Substring","Remove","Replace","Split","Trim","Length","Count",
    "Add","Clear","Insert","RemoveAt","Move","Sort","ToList","ToArray",
    "First","Last","ElementAt","ElementAtOrDefault","DefaultIfEmpty",
    "GetValues","GetValue","IndexOf","GetType","GetLength",
    "Parse","TryParse","IsNullOrEmpty",
    "ContainsKey","TryGetValue","KeyValuePair",
    "Round","Max","Min","Abs","Floor","Ceiling","Pow","Sqrt","Sin","Cos",
    "Acos","PI","Sign","IsNaN",
    "Normalize","Distance","Transform","TransformNormal","Transpose",
    "CreateFromDir","Sum","AbsMin",
    "Now","Seconds","TotalSeconds",
    # Common property names that appear after dots
    "X","Y","Z","Count","Length",
    "Keys","Values","Key","Value",
    "Char","CompareTo","PadRight","Move",
}

# ─── Identifiers from the deobfuscated code that map to user logic ────────
# The const values that appear in strings or are important to preserve
CONST_PRESERVE = {
    "VERSION","DATAREV",  # settings are fields read from Custom Data now: renamed like the rest
    "BROADCAST_TAG","BROADCAST_SELF_ID","BROADCAST_SEP",
    # String values used in SE API calls
    "OnOff_On","OnOff_Off","Override","Roll","Pitch","Yaw",
    "Connector","ConnectorMedium","ConnectorSmall","HYDROGEN",
    "URANIUM","_INGOT","STONE","ICE","ORE","COMPONENT","INGOT",
    "MyObjectBuilder_",
    "INSTRUCTIONS","DEBUG",
}

def strip_comments(code):
    """Remove C# single-line and multi-line comments, preserving strings."""
    result = []
    i = 0
    in_string = False
    string_char = None
    
    while i < len(code):
        # String literals
        if not in_string and code[i] in ('"', "'"):
            in_string = True
            string_char = code[i]
            result.append(code[i])
            i += 1
            continue
        
        if in_string:
            if code[i] == '\\':
                result.append(code[i:i+2])
                i += 2
                continue
            if code[i] == string_char:
                in_string = False
            result.append(code[i])
            i += 1
            continue
        
        # Single-line comment
        if code[i:i+2] == '//':
            while i < len(code) and code[i] != '\n':
                i += 1
            continue
        
        # Multi-line comment
        if code[i:i+2] == '/*':
            end = code.find('*/', i + 2)
            if end == -1:
                break
            i = end + 2
            continue
        
        result.append(code[i])
        i += 1
    
    return ''.join(result)

def extract_strings(code):
    """Replace string literals with placeholders, return map."""
    strings = {}
    counter = [0]
    
    def replacer(m):
        key = f"__STR{counter[0]}__"
        strings[key] = m.group(0)
        counter[0] += 1
        return key
    
    # Match "..." strings (including escaped quotes)
    pattern = r'"(?:[^"\\]|\\.)*"'
    code = re.sub(pattern, replacer, code)
    return code, strings

def restore_strings(code, strings):
    """Restore string literals from placeholders."""
    for key, val in strings.items():
        code = code.replace(key, val)
    return code

def find_identifiers(code):
    """Find all identifiers in the code."""
    # Match word-boundary identifiers
    pattern = r'\b([A-Za-z_]\w*)\b'
    return set(re.findall(pattern, code))

def build_protected_set():
    """Build complete set of names that should never be renamed."""
    protected = set()
    protected.update(CS_KEYWORDS)
    protected.update(SE_API)
    protected.update(CONST_PRESERVE)
    # C# contextual keywords
    protected.update({"get","set","value","add","remove","partial","async","await","yield","dynamic","nameof","when"})
    # Common patterns
    protected.update({"q","i","j","e"})  # common lambda/loop vars that might collide
    return protected

DECL_KEYWORD_BLOCK = {"new", "return", "else", "case", "goto", "throw", "in", "is", "as",
                      "typeof", "await", "yield", "using", "namespace", "base", "this"}

def find_declared_identifiers(code):
    """Names the script itself declares (types, enum members, methods, fields, locals,
    parameters, lambda parameters). Only these are safe to rename: anything else is an
    API name (game or .NET) and renaming it silently breaks the build in-game."""
    declared = set()
    # modifiers would otherwise be taken for the type in "public float[] x"
    code = re.sub(r'\b(?:public|private|protected|internal|static|readonly|const|override|virtual|'
                  r'abstract|sealed|ref|out|params|partial)\b', ' ', code)
    for m in re.finditer(r'\b(?:class|struct|enum|interface)\s+([A-Za-z_]\w*)', code):
        declared.add(m.group(1))
    # enum members
    for m in re.finditer(r'\benum\s+\w+\s*\{([^}]*)\}', code):
        for part in m.group(1).split(','):
            name = part.split('=')[0].strip()
            if re.match(r'^[A-Za-z_]\w*$', name):
                declared.add(name)
    # "Type name" followed by = ; , ( ) : fields, locals, parameters, methods, foreach vars
    type_re = r'([A-Za-z_]\w*(?:\.[A-Za-z_]\w*)*(?:<[^<>;(){}]*(?:<[^<>;(){}]*>[^<>;(){}]*)*>)?(?:\[[,\s]*\])*)'
    for m in re.finditer(type_re + r'\s+([A-Za-z_]\w*)\s*(?:<[^<>;(){}]*>\s*)?(?=[=;,()]|\bin\b)', code):
        type_tok = m.group(1).split('.')[0].split('<')[0]
        if type_tok in DECL_KEYWORD_BLOCK:
            continue
        declared.add(m.group(2))
    # further declarators: "int a = 0, b = 0;" / "float x, y;" / "Vector3 a, b, c;"
    for m in re.finditer(type_re + r'\s+([A-Za-z_]\w*(?:\s*,\s*[A-Za-z_]\w*)+)\s*;', code):
        if m.group(1).split('.')[0].split('<')[0] in DECL_KEYWORD_BLOCK:
            continue
        for name in m.group(2).split(','):
            declared.add(name.strip())
    for m in re.finditer(r',\s*([A-Za-z_]\w*)\s*(?==[^=>]|;)', code):
        declared.add(m.group(1))
    # lambda parameters: x => ... and (a, b) => ...
    for m in re.finditer(r'\b([A-Za-z_]\w*)\s*=>', code):
        declared.add(m.group(1))
    for m in re.finditer(r'\(([^()]*)\)\s*=>', code):
        for part in m.group(1).split(','):
            toks = part.split()
            if toks: declared.add(toks[-1])
    return declared

def collapse_whitespace(code):
    """Aggressively collapse whitespace while keeping code valid."""
    lines = code.split('\n')
    result = []
    
    for line in lines:
        stripped = line.strip()
        if stripped:
            result.append(stripped)
    
    # Join lines, but keep newlines where needed for C# syntax
    output = []
    for line in result:
        output.append(line)
    
    return '\n'.join(output)

def join_lines(code):
    """Join lines where safe (after ; or { or before })."""
    lines = code.split('\n')
    result = []
    buf = ""
    
    for line in lines:
        stripped = line.strip()
        if not stripped:
            continue
        
        if buf:
            # Can we join with previous?
            last_char = buf.rstrip()[-1] if buf.rstrip() else ''
            first_char = stripped[0] if stripped else ''
            
            if last_char in (';', '{', '}') or first_char in ('{', '}'):
                buf = buf.rstrip() + stripped
            elif last_char == ')' and first_char == '{':
                buf = buf.rstrip() + stripped
            else:
                buf = buf.rstrip() + '\n' + stripped
        else:
            buf = stripped
        
        # Flush if line is getting long (SE has line limits)
        if len(buf.split('\n')[-1]) > 300:
            result.append(buf)
            buf = ""
    
    if buf:
        result.append(buf)
    
    return '\n'.join(result)

def is_ident_char(c):
    """Check if character can be part of an identifier."""
    return c.isalnum() or c == '_' or ord(c) > 127

def tokenize_cs(code):
    """Simple C# tokenizer that splits into identifiers, strings, and punctuation."""
    tokens = []
    i = 0
    while i < len(code):
        c = code[i]
        # Whitespace
        if c in ' \t':
            i += 1
            continue
        # Newline (keep as token)
        if c == '\n':
            tokens.append(('\n', 'newline'))
            i += 1
            continue
        # String literal (already as __STR placeholder or actual)
        if c == '"':
            j = i + 1
            while j < len(code) and code[j] != '"':
                if code[j] == '\\':
                    j += 1
                j += 1
            tokens.append((code[i:j+1], 'string'))
            i = j + 1
            continue
        # Char literal
        if c == "'":
            j = i + 1
            while j < len(code) and code[j] != "'":
                if code[j] == '\\':
                    j += 1
                j += 1
            tokens.append((code[i:j+1], 'string'))
            i = j + 1
            continue
        # Identifier or keyword (including unicode)
        if c.isalpha() or c == '_' or ord(c) > 127:
            j = i
            while j < len(code) and is_ident_char(code[j]):
                j += 1
            tokens.append((code[i:j], 'ident'))
            i = j
            continue
        # Number
        if c.isdigit() or (c == '.' and i+1 < len(code) and code[i+1].isdigit()):
            j = i
            while j < len(code) and (code[j].isdigit() or code[j] in '.fFdDmMlLuU'):
                j += 1
            tokens.append((code[i:j], 'number'))
            i = j
            continue
        # Multi-char operators
        if i+1 < len(code):
            two = code[i:i+2]
            if two in ('==','!=','<=','>=','&&','||','++','--','+=','-=','*=','/=','%=','<<','>>','??','?.','->','::','=>'):
                tokens.append((two, 'op'))
                i += 2
                continue
        # Single char
        tokens.append((c, 'punct'))
        i += 1
    return tokens

def minimize_spaces(code):
    """Remove unnecessary spaces while preserving valid C# syntax."""
    tokens = tokenize_cs(code)
    result = []
    for i, (tok, typ) in enumerate(tokens):
        if i > 0:
            prev_tok, prev_typ = tokens[i-1]
            need_space = False
            # Space needed between two identifiers/numbers
            if prev_typ in ('ident','number') and typ in ('ident','number'):
                need_space = True
            # Space needed between keyword/identifier and string/char literal
            # e.g. case "UP", return "hello", etc.
            if prev_typ == 'ident' and typ == 'string':
                need_space = True
            # Space between string and identifier (e.g. after closing quote)
            if prev_typ == 'string' and typ == 'ident':
                need_space = True
            # Space between closing paren/bracket and identifier: )x => ) x
            if prev_typ == 'punct' and prev_tok == ')' and typ == 'ident':
                need_space = True
            # Space between identifier and negative sign that starts a number
            if prev_typ == 'ident' and typ == 'punct' and tok == '-':
                pass  # handled by C# parser
            if need_space:
                result.append(' ')
        result.append(tok)
    return ''.join(result)

CONFIG_END_MARKER = "// ===== End of settings: don't edit below this line ====="

def minify(source_code):
    """Kept as-is: everything up to and including CONFIG_END_MARKER (the user-editable
    settings block with its comments, as in the original script). Minified: the rest."""
    if CONFIG_END_MARKER in source_code:
        cut = source_code.index(CONFIG_END_MARKER) + len(CONFIG_END_MARKER)
        header = source_code[:cut]
        # drop build_pam's file-section banners from the kept header
        header = "\n".join(l for l in header.split("\n") if not l.lstrip().startswith("// =========================="))
        header = header.replace("\r\n", "\n").strip("\n")
        # never rename names the settings block declares
        header_names = set(re.findall(r'\bconst\s+\w+\s+([A-Za-z_]\w*)', header))
        body, mapping = _minify_body(source_code[cut:], header_names)
        return header + "\n" + body, mapping
    return _minify_body(source_code, set())

def _minify_body(source_code, extra_protected):
    """Main minification pipeline."""
    code = source_code
    
    # Step 1: Strip comments
    code = strip_comments(code)
    
    # Step 2: Extract string literals
    code, strings = extract_strings(code)
    
    # Step 3: Find all identifiers
    all_idents = find_identifiers(code)
    protected = build_protected_set()
    protected.update(extra_protected)
    
    # Filter to only user-defined identifiers: must be declared in the script and not protected
    declared = find_declared_identifiers(code)
    user_idents = set()
    skipped_undeclared = set()
    for ident in all_idents:
        if ident in protected:
            continue
        if ident.startswith("__STR"):
            continue
        if len(ident) <= 1:
            continue
        if ident not in declared:
            skipped_undeclared.add(ident)
            continue
        user_idents.add(ident)
    minify.skipped_undeclared = skipped_undeclared
    
    # Step 4: Count frequency to assign shortest names to most common
    freq = {}
    for ident in user_idents:
        count = len(re.findall(r'\b' + re.escape(ident) + r'\b', code))
        freq[ident] = count
    
    # Sort by frequency (most frequent gets shortest name)
    sorted_idents = sorted(freq.keys(), key=lambda x: (-freq[x], x))  # tie-break by name -> deterministic output
    
    # Step 5: Assign short names
    name_pool = build_name_pool()
    mapping = {}
    pool_idx = 0
    
    for ident in sorted_idents:
        if pool_idx >= len(name_pool):
            # Ran out of short names, keep original
            continue
        new_name = name_pool[pool_idx]
        # Make sure new name doesn't collide with protected names
        while new_name in protected or new_name in mapping.values():
            pool_idx += 1
            if pool_idx >= len(name_pool):
                break
            new_name = name_pool[pool_idx]
        
        if pool_idx < len(name_pool):
            mapping[ident] = new_name
            pool_idx += 1
    
    # Step 6: Replace identifiers (longest first to avoid partial matches)
    for ident in sorted(mapping.keys(), key=len, reverse=True):
        pattern = r'\b' + re.escape(ident) + r'\b'
        code = re.sub(pattern, mapping[ident], code)
    
    # Step 7: Restore string literals
    code = restore_strings(code, strings)
    
    # Step 8: Collapse whitespace
    code = collapse_whitespace(code)
    
    # Step 9: Join lines where safe
    code = join_lines(code)
    
    # Step 10: Token-aware space minimization
    code = minimize_spaces(code)
    
    return code, mapping

def main():
    parser = argparse.ArgumentParser(description='Minify PAM C# for Space Engineers')
    parser.add_argument('input', help='Input .cs file')
    parser.add_argument('--output', '-o', default='PAM_Minified.cs', help='Output file')
    parser.add_argument('--map', '-m', help='Output identifier mapping file')
    parser.add_argument('--stats', action='store_true', help='Show size statistics')
    args = parser.parse_args()
    
    with open(args.input, 'r', encoding='utf-8') as f:
        source = f.read()
    
    minified, mapping = minify(source)
    
    with open(args.output, 'w', encoding='utf-8') as f:
        f.write(minified)
    
    if args.map:
        with open(args.map, 'w', encoding='utf-8') as f:
            f.write("# Identifier mapping (original -> minified)\n")
            for orig, short in sorted(mapping.items()):
                f.write(f"{orig} -> {short}\n")
    
    if args.stats:
        orig_size = len(source)
        mini_size = len(minified)
        ratio = mini_size / orig_size * 100
        print(f"Original:  {orig_size:>8} chars ({orig_size // 1024} KB)")
        print(f"Minified:  {mini_size:>8} chars ({mini_size // 1024} KB)")
        print(f"Ratio:     {ratio:>7.1f}%")
        print(f"Identifiers renamed: {len(mapping)}")
        skipped = sorted(getattr(minify, 'skipped_undeclared', set()))
        print(f"Kept (not declared in script, treated as API): {len(skipped)}")
        if skipped: print("           " + ", ".join(skipped))
        print(f"SE limit:  100,000 chars -> {'OK' if mini_size < 100000 else 'TOO BIG'}")

if __name__ == '__main__':
    main()
