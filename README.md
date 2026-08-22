# NPC-demo

A **Unity host** for [Personality Engine](https://github.com/RossSim/personality-engine) **0.6.1** and [Archetypes](https://github.com/RossSim/archetypes) **1.0.0**.

Personality Engine keeps mood and feeling as named numbers while the game runs. Archetypes is a catalog of starting minds plus a builder. This repository is neither library. It is the Unity adapter and a small playable: two NPCs stay themselves after a slight, a gift, or a threat.

There is **no language model** here. Demo lines are tagged stems. A game that uses a model can still sit outside; see [Language models as a host](https://github.com/RossSim/personality-engine/blob/main/docs/LANGUAGE_MODELS.md).

[![License: MIT](https://img.shields.io/badge/license-MIT-yellow.svg)](LICENSE)
[![Release](https://img.shields.io/github/v/release/RossSim/NPC-demo)](https://github.com/RossSim/NPC-demo/releases/latest)

## Play without Unity (macOS)

Download **`NPC-demo-*-macos-universal.zip`** from [Releases](https://github.com/RossSim/NPC-demo/releases/latest). Unzip and open `NPC-demo.app`. You do **not** need the Unity Editor or a Unity license to run that player.

The build is **unsigned**. On first launch, right-click the app → **Open** (or System Settings → Privacy & Security). Universal binary (Apple silicon and Intel). It is a game demo, not a test; see [Disclaimer](DISCLAIMER.md).

## Package vs demo

This GitHub repo is a Unity project. Inside it, two trees have different jobs. **Do not mix them.**

```text
NPC-demo/                              this GitHub repo
│
├── Packages/com.rosssim.npc-host/     PACKAGE — add this folder to your game
│   └── Runtime/                       NpcMind, persist, host-event map
│
├── Assets/NpcDemo/                    DEMO — playable sample, not in the package
│   ├── Scripts/                       HUD, charts, tagged lines
│   └── Editor/                        menu to build the yard scene
│
└── Assets/Plugins/PersonalityEngine/  PE + Archetypes DLLs (not git; restore locally)
```

| You want to… | Use |
| --- | --- |
| Try two catalog minds on a Mac | [Release player](https://github.com/RossSim/NPC-demo/releases/latest) (no Unity install) |
| Put PE + Archetypes on an NPC in *your* game | The package only |
| Change the demo HUD | Clone this repo, restore DLLs, Unity 6 Play |
| Change psychology (new providers) | [personality-engine](https://github.com/RossSim/personality-engine), not here |
| Change starting minds (new jobs or clans) | [archetypes](https://github.com/RossSim/archetypes), not here |

When another project adds the package from git, Unity copies `Packages/com.rosssim.npc-host/` only. `Assets/NpcDemo/` does not come along. That is intentional.

## Build from source (Unity 6)

1. Install **Unity 6** (this repo’s editor project is `6000.5`). The UPM package itself declares **2022.3** as the minimum. API Compatibility: **.NET Standard 2.1** (menu **NPC Demo → Use .NET Standard 2.1**).
2. Clone this repository and add the folder in Unity Hub.
3. Restore engine DLLs, then Play or build a Mac player:

```bash
bash scripts/restore-unity-libs.sh
bash scripts/run-mac-demo.sh
```

You need **both** nupkgs. Archetypes does not embed Personality Engine. Do not also drop the PE zip beside the nupkg DLL (duplicate types).

Pinned versions: Personality Engine **0.6.1**, Archetypes **1.0.0**. Cutting a GitHub Release: [docs/RELEASING.md](docs/RELEASING.md).

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

Default pair: **village smith** and **wilderness scout** from the public Archetypes catalog. **Randomize personas** picks two other catalog rows (jobs, fantasy clans, or temperament climates) and a new set of event captions (thirteen beats). Buttons still send Personality Engine [host events](https://github.com/RossSim/personality-engine/blob/main/docs/HOSTING.md); the captions change, the OCC kind does not. Mood and anger decay while you wait. Charts sample Extraversion, Conscientiousness, Pleasure, Arousal, and anger at 60 Hz for up to 60 seconds. Each NPC picks a tagged line and ranks `stay` / `leave` / `haggle`.

## Legal

MIT. See [LICENSE](LICENSE) and [DISCLAIMER.md](DISCLAIMER.md).

This host is game software. It is not a test, not a clinic, and not professional advice. Personality Engine and Archetypes keep their own disclaimers; read them. Unity is a trademark of Unity Technologies; this project is not affiliated with Unity.

## Docs in this repo

| Doc | What it is |
| --- | --- |
| [Package README](Packages/com.rosssim.npc-host/README.md) | Adapter API for a real game |
| [Demo README](Assets/NpcDemo/README.md) | What the playable is allowed to do |
| [Releasing](docs/RELEASING.md) | Tag, macOS zip, GitHub Release |
| [Changelog](CHANGELOG.md) | Version notes |
| [Contributing](CONTRIBUTING.md) | Where a patch belongs |
| [Security](SECURITY.md) | Private vulnerability reports |
| [Disclaimer](DISCLAIMER.md) | Game software; not a test |
