# Security

Report vulnerabilities **privately** with [GitHub Security Advisories](https://github.com/RossSim/NPC-demo/security/advisories/new) on this repository. Do not open a public issue for a security report.

This project is a Unity host, not a network service. Useful reports include secret leaks in the repo, supply-chain issues in restore scripts or CI, unsafe handling if a game loads persist JSON or catalog JSON from untrusted players, or a Release `.app` that phones home unexpectedly.

The in-repo demo only round-trips persist JSON it just saved in memory. `NpcPersist.Apply` caps size (256 KiB) and deserializes a fixed POCO (`AffectPersist`), not polymorphic types. Catalog rows come from Archetypes compile-time seeds, not files on disk.

`scripts/restore-unity-libs.sh` pulls pinned GitHub Release nupkgs from RossSim/personality-engine and RossSim/archetypes plus System.Text.Json from nuget.org. The macOS Release zip is an **unsigned** Unity player; treat it like any other unsigned download (right-click Open). It is not a signed or notarized Apple build.

The code is MIT, with no warranty. See [LICENSE](LICENSE) and [DISCLAIMER.md](DISCLAIMER.md).
