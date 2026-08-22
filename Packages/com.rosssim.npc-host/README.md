# NPC Host (Unity package)

**This folder is the package.** Everything under `Packages/com.rosssim.npc-host/` is what another Unity project should import.

The playable sample lives in `Assets/NpcDemo/` of the [NPC-demo](https://github.com/RossSim/NPC-demo) repository. It is **not** shipped with this package.

## What it does

Puts one [Personality Engine](https://github.com/RossSim/personality-engine) `AffectEngine` on a GameObject, seeded from an [Archetypes](https://github.com/RossSim/archetypes) catalog id.

- `Update` calls `Tick(Time.deltaTime)` (idle decay)
- Buttons, combat, or a designer map call `Notify(...)` (host-tagged events, no extra decay that frame)
- `SaveToJson` / `LoadFromJson` round-trip `AffectPersist` (rebuild the mind first; snapshot floats alone are not a save)

There is no LLM SDK here. The engine does not write dialogue.

## Setup

1. Add this package (git URL with `?path=/Packages/com.rosssim.npc-host`).
2. Drop `PersonalityEngine.Core.dll` (0.6.1), `Archetypes.Core.dll` (1.0.0), and System.Text.Json **8.0.5** (plus its netstandard2.0 graph) into your project’s `Assets/Plugins`.
3. Player Settings → API Compatibility Level → **.NET Standard 2.1**.
4. Add `NpcMind`. Set **Preset Id** to a public catalog id (`village-smith`, `wilderness-scout`, …).
5. Optional IL2CPP: keep the `link.xml` in this package so `CatalogJson` reflection survives stripping. Prefer compile-time `Catalog.*` rows when you can; this package resolves known ids from `Catalog.Seeds` first.

Do not wrap `PresetBuilder` in `AlmaComposition`. Do not put `AffectEngine` on a `ScriptableObject`. One engine instance per mind.

## Host events

`HostEventKind` is a Unity-facing list of Personality Engine’s project-convention helpers (harm, threat, gratitude, …). The library still does not infer that a hit was anger — your game chooses the kind. Full table: [Hosting](https://github.com/RossSim/personality-engine/blob/main/docs/HOSTING.md).

`HostEventMap` is an optional ScriptableObject: world verb `"damage"` → `Harm`. Use it so combat code never names OCC.

## Legal

MIT. See the repo [LICENSE](../../LICENSE) and [DISCLAIMER.md](../../DISCLAIMER.md). Read the Personality Engine and Archetypes disclaimers as well. This package is not a psychometric test.
