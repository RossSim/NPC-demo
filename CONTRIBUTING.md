# Contributing

This repository is public MIT. Product docs live here. Read [DISCLAIMER.md](DISCLAIMER.md) first.

## Two folders

| Change belongs in | Path |
| --- | --- |
| Reusable Unity adapter (any game could import this) | `Packages/com.rosssim.npc-host/` |
| Playable sample, tagged lines, IMGUI HUD | `Assets/NpcDemo/` |
| How to drop Personality Engine / Archetypes DLLs | `Assets/Plugins/PersonalityEngine/` |

Do not move demo UI into the package. Do not add `IAffectProvider` implementations here — those stay in [personality-engine](https://github.com/RossSim/personality-engine). Do not add catalog rows here — those stay in [archetypes](https://github.com/RossSim/archetypes).

Releases: [docs/RELEASING.md](docs/RELEASING.md). The macOS player zip is how people run the demo without Unity.

## Public hygiene

Do not put private issue-tracker URLs, project keys, or ticket ids in this repository, pull requests, issues, commit messages, Releases, or release notes. Do not name other private or internal projects.

## License

Contributions are under the [MIT License](LICENSE). Copyright holder: RossSim (see LICENSE).
