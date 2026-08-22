# NpcDemo (playable)

**This folder is the demo.** It is not part of the UPM package. Games that add `com.rosssim.npc-host` from git do not receive these scripts.

| Package (import this) | Demo (this folder) |
| --- | --- |
| `Packages/com.rosssim.npc-host/` | `Assets/NpcDemo/` |
| Reusable adapter | Yard HUD, tagged lines, Editor menu |

## Play

- **No Unity:** download the macOS universal zip from [Releases](https://github.com/RossSim/NPC-demo/releases/latest) and open `NPC-demo.app`.
- **From this repo:** restore DLLs (`bash scripts/restore-unity-libs.sh`), then Unity Play, or `bash scripts/run-mac-demo.sh` (Editor still required to *build*).

Menu **NPC Demo → Create Yard Scene** writes `Assets/NpcDemo/Scenes/NpcYard.unity` for player builds.

## What it is allowed to do

- Seed two public catalog ids (default village-smith and wilderness-scout; **Randomize personas** picks two other catalog rows and a new event-caption set)
- Zoom and a decay-speed slider (realtime is fastest; slow end is about 5× slower)
- Thirteen event-caption beats; line charts at 60 Hz, left edge fixed at t=0, compressed up to 60s
- Send host-tagged events (button captions change per beat; OCC kinds stay the same)
- Pick a **pre-authored** line from mood/emotion bands
- Rank `stay` / `leave` / `haggle` with `WeightActions`

## What it is not

- Not a second affect engine
- Not a catalog (no new jobs or clans)
- Not an LLM client, prompt packer, or vendor SDK
- Not a psychometric display — labels are game numbers

See the repo [Disclaimer](../../DISCLAIMER.md).
