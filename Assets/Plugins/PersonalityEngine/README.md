Drop restored DLLs here (gitignored):

- `PersonalityEngine.Core.dll` (0.6.1)
- `Archetypes.Core.dll` (1.0.0)
- System.Text.Json 8.0.5 and its netstandard2.0 graph

From the repo root:

```bash
bash scripts/restore-unity-libs.sh
```

Do not commit these binaries. Do not drop both a PE zip extract and the nupkg DLL (duplicate types).
