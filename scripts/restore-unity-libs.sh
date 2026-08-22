#!/usr/bin/env bash
# Download Personality Engine + Archetypes nupkgs and the System.Text.Json graph
# into Assets/Plugins/PersonalityEngine (gitignored). Requires gh and dotnet.
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
DEST="$ROOT/Assets/Plugins/PersonalityEngine"
WORK="$ROOT/.restore"
PE_VERSION="${PE_VERSION:-0.6.1}"
ARCH_VERSION="${ARCH_VERSION:-1.0.0}"
STJ_VERSION="${STJ_VERSION:-8.0.5}"

mkdir -p "$DEST" "$WORK"

echo "Downloading PersonalityEngine.Core $PE_VERSION"
gh release download "v$PE_VERSION" \
  --repo RossSim/personality-engine \
  --pattern "PersonalityEngine.Core.${PE_VERSION}.nupkg" \
  --dir "$WORK" \
  --clobber

echo "Downloading Archetypes.Core $ARCH_VERSION"
gh release download "v$ARCH_VERSION" \
  --repo RossSim/archetypes \
  --pattern "Archetypes.Core.${ARCH_VERSION}.nupkg" \
  --dir "$WORK" \
  --clobber

extract_lib() {
  local nupkg="$1"
  unzip -o -j "$nupkg" "lib/netstandard2.1/*.dll" -d "$DEST" >/dev/null
}

extract_lib "$WORK/PersonalityEngine.Core.${PE_VERSION}.nupkg"
extract_lib "$WORK/Archetypes.Core.${ARCH_VERSION}.nupkg"

STJ_PROJ="$WORK/stj/StjRestore.csproj"
mkdir -p "$WORK/stj"
cat > "$STJ_PROJ" <<EOF
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
    <RestorePackagesPath>$WORK/nuget</RestorePackagesPath>
    <DisableImplicitNuGetFallbackFolder>true</DisableImplicitNuGetFallbackFolder>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="System.Text.Json" Version="$STJ_VERSION" />
  </ItemGroup>
</Project>
EOF

dotnet restore "$STJ_PROJ" --nologo -v q

# Unity needs the netstandard2.0 graph System.Text.Json 8 pulls in.
while IFS= read -r dll; do
  case "$(basename "$dll")" in
    System.Text.Json.dll|System.Text.Encodings.Web.dll|System.Memory.dll|System.Buffers.dll|System.IO.Pipelines.dll|System.Runtime.CompilerServices.Unsafe.dll|Microsoft.Bcl.AsyncInterfaces.dll|System.Threading.Tasks.Extensions.dll|System.Numerics.Vectors.dll)
      cp -f "$dll" "$DEST/"
      ;;
  esac
done < <(find "$WORK/nuget" -type f -name '*.dll' | grep '/lib/netstandard2.0/' || true)

# Some packages only ship netstandard2.1.
while IFS= read -r dll; do
  base="$(basename "$dll")"
  if [[ ! -f "$DEST/$base" ]]; then
    case "$base" in
      System.Text.Json.dll|System.Text.Encodings.Web.dll)
        cp -f "$dll" "$DEST/"
        ;;
    esac
  fi
done < <(find "$WORK/nuget" -type f -name '*.dll' | grep '/lib/netstandard2.1/' || true)

echo "Wrote DLLs to $DEST"
ls -1 "$DEST"/*.dll
echo "Open the Unity project and press Play."
