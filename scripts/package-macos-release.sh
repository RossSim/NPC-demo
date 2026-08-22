#!/usr/bin/env bash
# Zip the local Mac player plus license files for a GitHub Release.
# Run after scripts/run-mac-demo.sh --rebuild.
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
VERSION="${VERSION:-0.1.0}"
APP="$ROOT/Builds/macOS/NPC-demo.app"
NAME="NPC-demo-${VERSION}-macos-apple-silicon"
STAGE="$ROOT/dist/${NAME}"
ZIP="$ROOT/dist/${NAME}.zip"

if [[ ! -d "$APP" ]]; then
  echo "Missing $APP — run scripts/run-mac-demo.sh --rebuild first." >&2
  exit 1
fi

rm -rf "$STAGE" "$ZIP"
mkdir -p "$STAGE"
cp -R "$APP" "$STAGE/"
cp "$ROOT/LICENSE" "$STAGE/LICENSE.txt"
cp "$ROOT/DISCLAIMER.md" "$STAGE/"
cat > "$STAGE/PLAY.txt" <<EOF
NPC-demo ${VERSION} — macOS Apple silicon player

Open NPC-demo.app. You do not need the Unity Editor.

The app is unsigned. First launch: right-click → Open.

This is game software, not a psychological test.
Personality Engine 0.6.1 and Archetypes 1.0.0 are MIT and bundled inside the player.
Read DISCLAIMER.md and LICENSE.txt.

https://github.com/RossSim/NPC-demo
https://github.com/RossSim/personality-engine
https://github.com/RossSim/archetypes
EOF

ditto -c -k --keepParent "$STAGE" "$ZIP"
echo "Wrote $ZIP"
ls -lh "$ZIP"
file "$STAGE/NPC-demo.app/Contents/MacOS/NPC-demo"
