# Disclaimer

**This repository is a Unity host for game and simulation software. It is not a psychological test, not a medical device, and not professional advice.**

This notice does not replace the [MIT License](LICENSE). The MIT grant and the MIT “AS IS” / no-liability terms remain the license.

This host does not implement psychology. It ticks and saves two other libraries:

- [Personality Engine](https://github.com/RossSim/personality-engine) — events in, named numbers out. Read its [Disclaimer](https://github.com/RossSim/personality-engine/blob/main/DISCLAIMER.md).
- [Archetypes](https://github.com/RossSim/archetypes) — starting-mind catalogs and a builder. Read its [Disclaimer](https://github.com/RossSim/archetypes/blob/main/DISCLAIMER.md).

## What this project is

A Unity package (`Packages/com.rosssim.npc-host`) plus a playable demo (`Assets/NpcDemo`). The package owns one mind per GameObject, idle ticks, host-event helpers, and JSON persist. The demo shows two catalog NPCs on screen. Numbers labeled **project convention** in those libraries are game-feel knobs, not psychometric scoring keys.

There is no language model in this repository. A game that uses a model can still host Personality Engine; that wiring stays in the game. See [Language models as a host](https://github.com/RossSim/personality-engine/blob/main/docs/LANGUAGE_MODELS.md).

## What this project is not

Do not represent this host, Personality Engine snapshots, Archetypes presets, or the demo UI as any of the following:

- Psychological, psychiatric, counseling, coaching, or medical services
- A diagnostic, prognostic, or treatment instrument
- A validated psychometric test, personnel-selection tool, education high-stakes assessment, or clinical scale
- A substitute for a licensed professional’s judgment
- An FDA-cleared or CE-marked medical device, Software as a Medical Device, or general-wellness clinical claim

A product that wraps these libraries and makes those claims does so **at that product’s own risk** and must obtain its own legal, regulatory, and professional review. The copyright holders do not authorize such representations.

## Third-party libraries and marks

This repository does not re-license Personality Engine or Archetypes. Those projects are MIT; you must keep their copyright notices and license terms when you copy their binaries or source. Cited authors, publishers, and institutions named in those projects have **not** endorsed this host.

Unity is a trademark of Unity Technologies. This project is not affiliated with, endorsed by, or sponsored by Unity Technologies. You need your own Unity license to open the editor.

## Saves and untrusted data

`NpcPersist` serializes a host-owned affect bag. If you ever load persist JSON from mods or other players, cap size before deserialize and treat the blob as untrusted input. See [Hosting](https://github.com/RossSim/personality-engine/blob/main/docs/HOSTING.md) in Personality Engine.

## No warranty

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, TITLE, AND NONINFRINGEMENT.

TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES, OR OTHER LIABILITY ARISING FROM, OUT OF, OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

If you do not agree to the MIT License and this notice, do not download, use, copy, or distribute the Software.
