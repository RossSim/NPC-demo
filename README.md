# NPC-demo

A **Unity host** for [Personality Engine](https://github.com/RossSim/personality-engine) and [Archetypes](https://github.com/RossSim/archetypes).

Personality Engine keeps mood and feeling as named numbers while the game runs. Archetypes is a catalog of starting minds (smith, scout, and so on) plus a builder that turns those rows into an engine. This repository is neither of those libraries. It is the Unity adapter and a small playable that shows two NPCs staying themselves after you insult them, give a gift, or raise a threat.

There is **no language model** in this project. Lines in the demo are tagged stems. A game that uses a model can still sit outside and read the same numbers; see [Language models as a host](https://github.com/RossSim/personality-engine/blob/main/docs/LANGUAGE_MODELS.md).

[![License: MIT](https://img.shields.io/badge/license-MIT-yellow.svg)](LICENSE)

## Package vs demo

This GitHub repo is a Unity project. Inside it, two trees have different jobs. **Do not mix them.**

```text
NPC-demo/                              this GitHub repo (open this in Unity)
│
├── Packages/com.rosssim.npc-host/     PACKAGE — add this folder to your game
│   └── Runtime/                       NpcMind, persist, host-event map
│
├── Assets/NpcDemo/                    DEMO — playable sample, not in the package
│   ├── Scripts/                       HUD, tagged lines
│   └── Editor/                        menu to build the yard scene
│
└── Assets/Plugins/PersonalityEngine/  your copy of PE + Archetypes DLLs (not git)
```

| You want to… | Use |
| --- | --- |
| Put PE + Archetypes on an NPC in *your* game | The package only |
| See two catalog jobs on screen | Clone this repo and press Play |
| Change psychology (new providers) | [personality-engine](https://github.com/RossSim/personality-engine), not here |
| Change starting minds (new jobs or clans) | [archetypes](https://github.com/RossSim/archetypes), not here |

When another project adds the package from git, Unity copies `Packages/com.rosssim.npc-host/` only. `Assets/NpcDemo/` does not come along. That is intentional.

## Quick start (this repo)

1. Install **Unity 6** (this repo’s editor project is `6000.5`). The UPM package itself declares **2022.3** as the minimum. Set Player API Compatibility to **.NET Standard 2.1** (menu **NPC Demo → Use .NET Standard 2.1**).
2. Clone this repository and open the folder as a Unity project (Unity Hub → Add → `NPC-demo`).
3. Restore engine DLLs (GitHub CLI required):

```bash
bash scripts/restore-unity-libs.sh
```

4. Press Play. The demo HUD spawns on an empty scene. Optional: menu **NPC Demo → Create Yard Scene** saves `Assets/NpcDemo/Scenes/NpcYard.unity`.

You need **both** nupkgs. Archetypes does not embed Personality Engine. Do not also drop the PE zip beside the nupkg DLL (duplicate types).

Pinned versions: Personality Engine **0.6.1**, Archetypes **1.0.0**.

## Add the package to another Unity project

In that project’s `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.rosssim.npc-host": "https://github.com/RossSim/NPC-demo.git?path=/Packages/com.rosssim.npc-host"
  }
}
```

Then put `PersonalityEngine.Core.dll`, `Archetypes.Core.dll`, and the System.Text.Json 8.0.5 graph in that project’s `Assets/Plugins` (same restore script, different destination). Tick `Time.deltaTime`. One `NpcMind` per character, on a plain C# field inside the component — do not hang the engine off a Unity asset.

How to tick, save, and fold action weights in any host (Unity or not): [Hosting](https://github.com/RossSim/personality-engine/blob/main/docs/HOSTING.md).

## What you will see

The demo builds a **village smith** and a **wilderness scout** from the public Archetypes catalog. Buttons send Personality Engine [host events](https://github.com/RossSim/personality-engine/blob/main/docs/HOSTING.md) (harm, gratitude, threat). Mood and anger decay while you wait. Each NPC picks a tagged line and ranks `stay` / `leave` / `haggle` from the snapshot. Same buttons, two different people.

## Legal

MIT. See [LICENSE](LICENSE) and [DISCLAIMER.md](DISCLAIMER.md).

This host is game software. It is not a test, not a clinic, and not professional advice. Personality Engine and Archetypes keep their own disclaimers; read them. Unity is a trademark of Unity Technologies; this project is not affiliated with Unity.

## Docs in this repo

| Doc | What it is |
| --- | --- |
| [Package README](Packages/com.rosssim.npc-host/README.md) | Adapter API for a real game |
| [Demo README](Assets/NpcDemo/README.md) | What the playable is allowed to do |
| [Changelog](CHANGELOG.md) | Version notes |
| [Contributing](CONTRIBUTING.md) | Where a patch belongs |
| [Security](SECURITY.md) | Private vulnerability reports |
| [Disclaimer](DISCLAIMER.md) | Game software; not a test |
