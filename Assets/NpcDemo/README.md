# NpcDemo (playable)

**This folder is the demo.** It is not part of the UPM package. Games that add `com.rosssim.npc-host` from git do not receive these scripts.

| Package (import this) | Demo (this folder) |
| --- | --- |
| `Packages/com.rosssim.npc-host/` | `Assets/NpcDemo/` |
| Reusable adapter | Yard HUD, tagged lines, Editor menu |

## Play

Restore DLLs from the repo root (`bash scripts/restore-unity-libs.sh`), open the Unity project, press Play. `NpcDemoHud` spawns itself on an empty scene.

Menu **NPC Demo → Create Yard Scene** writes `Assets/NpcDemo/Scenes/NpcYard.unity` for player builds.

## What it is allowed to do

- Seed two public catalog ids: `village-smith` and `wilderness-scout`
- Send host-tagged events (insult, gift, threat)
- Pick a **pre-authored** line from mood/emotion bands
- Rank `stay` / `leave` / `haggle` with `WeightActions`

## What it is not

- Not a second affect engine
- Not a catalog (no new jobs or clans)
- Not an LLM client, prompt packer, or vendor SDK
- Not a psychometric display — labels are game numbers

See the repo [Disclaimer](../../DISCLAIMER.md).
