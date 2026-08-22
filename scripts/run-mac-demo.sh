#!/usr/bin/env bash
# Restore engine DLLs if needed, build a macOS player with Unity, and open it.
# The HUD is a Unity Game view — there is no non-Unity playable in this repo.
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
APP="$ROOT/Builds/macOS/NPC-demo.app"
UNITY="${UNITY:-/Applications/Unity/Hub/Editor/6000.5.8f1/Unity.app/Contents/MacOS/Unity}"
DLL="$ROOT/Assets/Plugins/PersonalityEngine/PersonalityEngine.Core.dll"
mkdir -p "$ROOT/Logs"

if [[ ! -x "$UNITY" ]]; then
  echo "Unity 6 editor not found at $UNITY" >&2
  echo "Install Unity 6000.5 (Hub) or set UNITY= to the Editor binary." >&2
  exit 1
fi

if [[ ! -f "$DLL" ]]; then
  echo "Restoring Personality Engine / Archetypes DLLs..."
  bash "$ROOT/scripts/restore-unity-libs.sh"
fi

FORCE="${1:-}"
if [[ "$FORCE" == "--rebuild" ]] || [[ ! -d "$APP" ]]; then
  echo "Building Mac player (Unity batchmode)..."
  "$UNITY" -batchmode -nographics -quit \
    -projectPath "$ROOT" \
    -logFile "$ROOT/Logs/mac-player-build.log" \
    -executeMethod RossSim.NpcDemo.Editor.NpcDemoMenu.BuildMacPlayer
fi

if [[ ! -d "$APP" ]]; then
  echo "Build did not produce $APP" >&2
  echo "See Logs/mac-player-build.log" >&2
  exit 1
fi

echo "Launching $APP"
open "$APP"
