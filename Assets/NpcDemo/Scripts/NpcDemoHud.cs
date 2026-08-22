using RossSim.NpcHost;
using UnityEngine;

namespace RossSim.NpcDemo
{
    /// <summary>
    /// Playable HUD. Demo-only. Spawns two catalog minds and sends host-tagged events.
    /// </summary>
    public sealed class NpcDemoHud : MonoBehaviour
    {
        NpcMind smith;
        NpcMind scout;
        string lastJson;
        string status = "Same buttons. Two starting minds. No language model.";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void AutoSpawn()
        {
            if (HasHud())
                return;
            var root = new GameObject("NpcDemo");
            root.AddComponent<NpcDemoHud>();
        }

        static bool HasHud()
        {
#if UNITY_6000_0_OR_NEWER
            return FindAnyObjectByType<NpcDemoHud>() != null;
#elif UNITY_2023_1_OR_NEWER
            return FindFirstObjectByType<NpcDemoHud>() != null;
#else
            return FindObjectOfType<NpcDemoHud>() != null;
#endif
        }

        void Awake()
        {
            smith = CreateMind("Smith", "village-smith", new Vector3(-1.5f, 0f, 0f));
            scout = CreateMind("Scout", "wilderness-scout", new Vector3(1.5f, 0f, 0f));
        }

        NpcMind CreateMind(string name, string presetId, Vector3 pos)
        {
            var go = new GameObject(name);
            go.transform.SetParent(transform, false);
            go.transform.position = pos;
            var mind = go.AddComponent<NpcMind>();
            mind.Configure(presetId);
            return mind;
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, 16, Screen.width - 32, Screen.height - 32));
            GUILayout.Label("NPC-demo — Personality Engine + Archetypes host");
            GUILayout.Label(status);
            GUILayout.Space(8);

            GUILayout.BeginHorizontal();
            DrawColumn(smith, "Village smith");
            GUILayout.Space(24);
            DrawColumn(scout, "Wilderness scout");
            GUILayout.EndHorizontal();

            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Insult both (anger)"))
                Pulse(HostEventKind.Anger, "They hear it as yours.");
            if (GUILayout.Button("Gift both (gratitude)"))
                Pulse(HostEventKind.Gratitude, "A gift, tagged as gratitude — the engine did not infer that.");
            if (GUILayout.Button("Threat"))
                Pulse(HostEventKind.Threat, "Danger in the room.");
            if (GUILayout.Button("Threat passed"))
                Pulse(HostEventKind.ThreatPassed, "The danger is gone. Mood still has to catch up.");
            if (GUILayout.Button("Need met"))
                Pulse(HostEventKind.NeedMet, "A need was satisfied.");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save smith JSON"))
            {
                lastJson = smith.SaveToJson();
                status = "Saved smith persist blob (" + lastJson.Length + " chars). Load applies to the smith after rebuild.";
            }
            GUI.enabled = lastJson != null;
            if (GUILayout.Button("Load into smith"))
            {
                smith.LoadFromJson(lastJson);
                status = "Imported. Composition was rebuilt first.";
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.Label("Game numbers, not a test. See DISCLAIMER.md.");
            GUILayout.EndArea();
        }

        void Pulse(HostEventKind kind, string note)
        {
            smith.Notify(kind, 0.8f, "player");
            scout.Notify(kind, 0.8f, "player");
            status = note;
        }

        void DrawColumn(NpcMind mind, string title)
        {
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(360));
            GUILayout.Label(title + "  (" + mind.PresetId + ")");
            GUILayout.Label(string.Format(
                "E {0:0.00}   C {1:0.00}   P {2:0.00}   A {3:0.00}   anger {4:0.00}",
                mind.Extraversion, mind.Conscientiousness, mind.Pleasure, mind.Arousal, mind.Anger));
            var weights = mind.Weight("stay", "leave", "haggle");
            var move = NpcDemoLines.TopMove(weights);
            GUILayout.Label("Ranked move: " + move);
            GUILayout.Label(NpcDemoLines.Pick(mind.PresetId, mind.Arousal, mind.Anger, move));
            GUILayout.EndVertical();
        }
    }
}
