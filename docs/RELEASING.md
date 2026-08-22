# Releasing

Product docs live in this repository. Do not put private issue-tracker URLs, project keys, or ticket ids in tags, Releases, or notes.

Current cut: **0.1.0**. Personality Engine **0.6.1**, Archetypes **1.0.0**.

## Playable (no Unity install)

The GitHub Release **macOS universal** zip is the demo people can run without the Unity Editor. Rebuild the player, then zip:

```bash
bash scripts/restore-unity-libs.sh
bash scripts/run-mac-demo.sh --rebuild
bash scripts/package-macos-release.sh
```

`package-macos-release.sh` writes `dist/NPC-demo-<version>-macos-universal.zip` (the `.app` plus LICENSE, disclaimer, and a play note). The zip is gitignored.

## Tag and GitHub Release

1. Changelog section for the version is complete.
2. `Packages/com.rosssim.npc-host/package.json` version matches.
3. Annotated tag `vMAJOR.MINOR.PATCH` on `main`.
4. `gh release create` with that tag, the zip, and notes that:
   - name the **macOS player** as the no-Editor demo
   - link [Personality Engine](https://github.com/RossSim/personality-engine) 0.6.1 and [Archetypes](https://github.com/RossSim/archetypes) 1.0.0 (MIT, their disclaimers)
   - say the app is **unsigned** (right-click Open)
   - say this is game software, not a test

The UPM package is the git URL with `?path=/Packages/com.rosssim.npc-host`. It is not on nuget.org.
