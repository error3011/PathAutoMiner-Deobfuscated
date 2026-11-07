#!/usr/bin/env python3
"""
Builds a single-mode variant of PAM (ship-only or controller-only) from PAM_Combined.cs.

The programmable block limit is 100,000 characters. One script holding both the ship logic
and the controller logic doesn't leave room for new features, so the build produces:
  - PAM_Ship.cs        : miners / grinders / shuttles (controller code removed)
  - PAM_Controller.cs  : the [PAM-Controller] PB    (ship main loop removed)

How: the entry method of the other mode is replaced by a short stub that tells the player to
paste the other file, then every method that is no longer reachable from Main / the
constructor / Save is dropped. Reachability is by identifier name, so it errs on the side of
keeping code (a name used anywhere in kept code keeps every overload of that method).
Fields, types and enums are always kept.

Pure Python, no dependencies.
"""

import re

ROOTS = {"Main", "Program", "Save"}

VARIANTS = {
    "ship": {
        "remove": ["ControllerMainLoop"],
        "stub": 'void ControllerMainLoop(string argument) { Echo("[PAM] This is the ship script.\\n'
                'Paste PAM_Controller.cs into the [PAM-Controller] PB."); }',
    },
    "controller": {
        "remove": ["ShipMainLoop"],
        "stub": 'void ShipMainLoop(string argument) { Echo("[PAM] This is the controller script.\\n'
                'Paste PAM_Ship.cs into the ship PB, or add [PAM-Controller] to this PB\'s name."); }',
    },
}

_IDENT = re.compile(r"[A-Za-z_]\w*")


def _split_top_level(code):
    """Split class-body source into top-level items (methods, fields, types).
    Returns a list of (start, end) spans covering the whole text."""
    items = []
    i, n = 0, len(code)
    depth = 0
    start = 0
    while i < n:
        c = code[i]
        nxt = code[i + 1] if i + 1 < n else ""
        if c == "/" and nxt == "/":
            j = code.find("\n", i)
            i = n if j < 0 else j
            continue
        if c == "/" and nxt == "*":
            j = code.find("*/", i + 2)
            i = n if j < 0 else j + 2
            continue
        if c == '"':
            if i > 0 and code[i - 1] == "@":  # verbatim string
                j = i + 1
                while j < n:
                    if code[j] == '"' and (j + 1 >= n or code[j + 1] != '"'):
                        break
                    j += 2 if code[j] == '"' else 1
                i = j + 1
                continue
            j = i + 1
            while j < n and code[j] != '"':
                j += 2 if code[j] == "\\" else 1
            i = j + 1
            continue
        if c == "'":
            j = i + 1
            while j < n and code[j] != "'":
                j += 2 if code[j] == "\\" else 1
            i = j + 1
            continue
        if c == "{":
            depth += 1
        elif c == "}":
            depth -= 1
            if depth == 0:
                # a block at depth 0 ends the item unless it is part of a field initializer
                k = i + 1
                while k < n and code[k] in " \t\r\n":
                    k += 1
                if k >= n or code[k] not in ";,)":
                    items.append((start, i + 1))
                    start = i + 1
        elif c == ";" and depth == 0:
            items.append((start, i + 1))
            start = i + 1
        i += 1
    if code[start:].strip():
        items.append((start, n))
    return items


_NOISE = re.compile(r'@"(?:[^"]|"")*"|"(?:[^"\\\n]|\\.)*"|\'(?:[^\'\\]|\\.)\'|/\*.*?\*/|//[^\n]*', re.S)


def _strip_noise(text):
    """Remove comments, strings and char literals so identifiers can be scanned (one pass, so a
    // inside a string is not taken for a comment)."""
    def repl(m):
        t = m.group(0)
        if t.startswith("/"):
            return " "
        return "' '" if t.startswith("'") else '""'
    return _NOISE.sub(repl, text)


_METHOD_HEAD = re.compile(
    r"^\s*(?:[\w<>\[\],.?]+\s+)*?([A-Za-z_]\w*)\s*(?:<[\w\s,]*>)?\s*\([^;{]*\)\s*(?:\{|=>)", re.S)


def _method_name(item_code):
    """Name of the method declared by this item, or None for fields/types."""
    clean = _strip_noise(item_code)
    head = clean.split("{", 1)[0] if "{" in clean else clean
    if re.search(r"\b(class|struct|enum|interface)\b", head):
        return None
    first_paren = clean.find("(")
    if first_paren < 0:
        return None
    before = clean[:first_paren]
    if re.search(r"=(?!>)", before):  # field initializer such as  List<X> a = new List<X>();
        return None
    m = _METHOD_HEAD.match(clean)
    if not m:
        return None
    name = m.group(1)
    if name in ("new", "return", "if", "while", "for", "foreach", "switch", "using", "lock"):
        return None
    return name


_TYPE_DECL = re.compile(r"\b(?:class|struct|enum|interface)\s+([A-Za-z_]\w*)")


def _declared_names(item_code):
    """Names a field or type item declares (empty when it can't tell: then it is always kept)."""
    clean = _strip_noise(item_code).strip()
    if not clean:
        return []
    head = clean.split("{", 1)[0]
    m = _TYPE_DECL.search(head)
    if m:
        return [m.group(1)]
    if not clean.endswith(";") or "(" in clean.split("=", 1)[0]:
        return []
    body = clean[:-1]
    parts, depth, cur = [], 0, ""
    for ch in body:  # split declarators at top-level commas:  int a = 0, b = 1;
        if ch in "<({[":
            depth += 1
        elif ch in ">)}]":
            depth -= 1
        if ch == "," and depth == 0:
            parts.append(cur)
            cur = ""
        else:
            cur += ch
    parts.append(cur)
    names = []
    for part in parts:
        left = part.split("=", 1)[0]
        ids = _IDENT.findall(left)
        if ids:
            names.append(ids[-1])
    return names


def make_variant(source, variant):
    """Return (code, report) for the given variant name ('ship' or 'controller')."""
    cfg = VARIANTS[variant]
    spans = _split_top_level(source)
    items = []
    for s, e in spans:
        text = source[s:e]
        items.append({"text": text, "name": _method_name(text)})

    remove = set(cfg["remove"])
    items = [it for it in items if it["name"] not in remove]
    items.append({"text": "\n" + cfg["stub"] + "\n", "name": cfg["remove"][0]})

    # fields and types are dropped too when nothing that is kept uses them
    for it in items:
        it["decl"] = [it["name"]] if it["name"] else _declared_names(it["text"])

    by_name = {}
    for it in items:
        for n in it["decl"]:
            by_name.setdefault(n, []).append(it)

    def idents(text):
        return set(_IDENT.findall(_strip_noise(text)))

    reachable = set()
    queue = [r for r in ROOTS if r in by_name]
    # items whose declaration can't be told are always kept, with what they use
    for it in items:
        if not it["decl"]:
            queue.extend(x for x in idents(it["text"]) if x in by_name)
    while queue:
        name = queue.pop()
        if name in reachable:
            continue
        reachable.add(name)
        for it in by_name[name]:
            for x in idents(it["text"]):
                if x in by_name and x not in reachable:
                    queue.append(x)

    kept, dropped = [], []
    for it in items:
        if it["decl"] and not any(n in reachable for n in it["decl"]):
            dropped.append(it["decl"][0])
            # keep leading comments (section banners, the settings end marker) of a dropped method
            lead = re.match(r"(?:\s*(?://[^\n]*(?:\n|$)|/\*.*?\*/))*", it["text"], re.S).group(0)
            kept.append(lead)
            continue
        kept.append(it["text"])
    code = "".join(kept)
    report = {"variant": variant, "dropped_methods": sorted(set(dropped)), "methods_kept": len(reachable)}
    return code, report


if __name__ == "__main__":
    import argparse, json
    ap = argparse.ArgumentParser(description="Make a ship-only or controller-only PAM source")
    ap.add_argument("input")
    ap.add_argument("variant", choices=sorted(VARIANTS))
    ap.add_argument("-o", "--output", required=True)
    a = ap.parse_args()
    src = open(a.input, encoding="utf-8").read()
    out, rep = make_variant(src, a.variant)
    open(a.output, "w", encoding="utf-8").write(out)
    print(json.dumps(rep, indent=1))
