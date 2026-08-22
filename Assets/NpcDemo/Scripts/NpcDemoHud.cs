using RossSim.NpcHost;
using UnityEngine;

namespace RossSim.NpcDemo
{
    /// <summary>
    /// Playable HUD. Demo-only. Spawns two catalog minds and sends host-tagged events.
    /// </summary>
    [DefaultExecutionOrder(100)]
    public sealed class NpcDemoHud : MonoBehaviour
    {
        const float ZoomMin = 1f;
        const float ZoomMax = 3f;
        const float ZoomDefault = 2f;
        const float SpeedMin = 0.2f;
        const float SpeedMax = 1f;
        const float PersonaWidth = 320f;
        const float RowHeight = 200f;

        NpcMind left;
        NpcMind right;
        readonly NpcDemoTrace leftTrace = new NpcDemoTrace();
        readonly NpcDemoTrace rightTrace = new NpcDemoTrace();
        string lastJson;
        bool lastJsonWasLeft;
        string status = "Same buttons. Two starting minds. No language model.";
        NpcDemoBeat beat = NpcDemoBeats.All[0];
        float zoom = ZoomDefault;
        float speed = SpeedMax;
        float elapsed;
        bool capped;
        float sampleAcc;
        Vector2 scroll;
        GUIStyle wrapButton;
        GUIStyle wrapLabel;
        GUIStyle wrapBox;

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
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            left = CreateMind("Left", "village-smith", new Vector3(-1.5f, 0f, 0f));
            right = CreateMind("Right", "wilderness-scout", new Vector3(1.5f, 0f, 0f));
        }

        void Update()
        {
            if (capped)
            {
                if (left != null)
                    left.TickScale = 0f;
                if (right != null)
                    right.TickScale = 0f;
                return;
            }

            if (left != null)
                left.TickScale = speed;
            if (right != null)
                right.TickScale = speed;

            var step = 1f / NpcDemoTrace.SampleHz;
            sampleAcc += Time.deltaTime;
            while (sampleAcc >= step && leftTrace.Count < NpcDemoTrace.MaxSamples)
            {
                sampleAcc -= step;
                leftTrace.Push(left);
                rightTrace.Push(right);
            }

            elapsed = leftTrace.Count / NpcDemoTrace.SampleHz;
            if (leftTrace.Count >= NpcDemoTrace.MaxSamples)
            {
                elapsed = NpcDemoTrace.MaxSeconds;
                capped = true;
                status = "60s cap. Randomize personas to start a new run.";
            }
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

        void EnsureStyles()
        {
            if (wrapButton != null)
                return;
            wrapButton = new GUIStyle(GUI.skin.button) { wordWrap = true };
            wrapLabel = new GUIStyle(GUI.skin.label) { wordWrap = true };
            wrapBox = new GUIStyle(GUI.skin.box) { wordWrap = true };
        }

        void OnGUI()
        {
            EnsureStyles();
            var old = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(zoom, zoom, 1f));
            var w = Screen.width / zoom;
            var h = Screen.height / zoom;
            GUILayout.BeginArea(new Rect(12f, 12f, w - 24f, h - 24f));
            scroll = GUILayout.BeginScrollView(scroll);

            GUILayout.Label("NPC-demo — Personality Engine + Archetypes host", wrapLabel);
            GUILayout.Label(status, wrapLabel);
            GUILayout.Space(6);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Zoom " + zoom.ToString("0.0") + "×", GUILayout.Width(110));
            zoom = GUILayout.HorizontalSlider(zoom, ZoomMin, ZoomMax);
            if (GUILayout.Button("Reset", GUILayout.Width(70)))
                zoom = ZoomDefault;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Speed (decay)", GUILayout.Width(110));
            GUILayout.Label("slow", GUILayout.Width(40));
            speed = GUILayout.HorizontalSlider(speed, SpeedMin, SpeedMax);
            GUILayout.Label("fast", GUILayout.Width(40));
            GUILayout.EndHorizontal();
            GUILayout.Label(
                speed >= 0.99f
                    ? "Decay is realtime (fastest)."
                    : "Decay is about " + (1f / speed).ToString("0.0") + "× slower than realtime.",
                wrapLabel);

            GUILayout.Space(6);
            if (GUILayout.Button("Randomize personas", GUILayout.Height(36)))
                RandomizePair();

            GUILayout.Space(8);
            DrawPersonaRow(left, leftTrace);
            GUILayout.Space(8);
            DrawPersonaRow(right, rightTrace);

            GUILayout.Space(10);
            GUILayout.Label("This beat: " + beat.Title + " — same OCC kinds, different captions. You still choose the tag.", wrapLabel);

            GUI.enabled = !capped;
            DrawBeatButton(beat.Anger, beat.AngerNote, HostEventKind.Anger);
            DrawBeatButton(beat.Gratitude, beat.GratitudeNote, HostEventKind.Gratitude);
            DrawBeatButton(beat.Threat, beat.ThreatNote, HostEventKind.Threat);
            DrawBeatButton(beat.ThreatPassed, beat.ThreatPassedNote, HostEventKind.ThreatPassed);
            DrawBeatButton(beat.NeedMet, beat.NeedMetNote, HostEventKind.NeedMet);
            GUI.enabled = true;

            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save " + NpcPreset.DisplayName(left.PresetId) + " JSON", GUILayout.Height(32)))
            {
                lastJson = left.SaveToJson();
                lastJsonWasLeft = true;
                status = "Saved persist blob (" + lastJson.Length + " chars). Load rebuilds that mind first.";
            }

            GUI.enabled = lastJson != null;
            var loadTarget = lastJsonWasLeft ? left : right;
            var loadName = loadTarget == null ? "mind" : NpcPreset.DisplayName(loadTarget.PresetId);
            if (GUILayout.Button("Load into " + loadName, GUILayout.Height(32)))
            {
                loadTarget.LoadFromJson(lastJson);
                status = "Imported. Composition was rebuilt first.";
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.Label("Game numbers, not a test. See DISCLAIMER.md.", wrapLabel);
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            GUI.matrix = old;
        }

        void DrawPersonaRow(NpcMind mind, NpcDemoTrace trace)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(RowHeight));
            DrawColumn(mind);
            var chart = GUILayoutUtility.GetRect(
                80f,
                RowHeight,
                GUILayout.ExpandWidth(true),
                GUILayout.Height(RowHeight));
            NpcDemoChart.Draw(chart, trace, elapsed);
            GUILayout.EndHorizontal();
        }

        void DrawBeatButton(string caption, string note, HostEventKind kind)
        {
            if (GUILayout.Button(caption, wrapButton, GUILayout.Height(40), GUILayout.ExpandWidth(true)))
                Pulse(kind, note);
        }

        void Pulse(HostEventKind kind, string note)
        {
            if (capped)
                return;
            left.Notify(kind, 0.8f, "player");
            right.Notify(kind, 0.8f, "player");
            status = note;
        }

        void DrawColumn(NpcMind mind)
        {
            GUILayout.BeginVertical(
                wrapBox,
                GUILayout.Width(PersonaWidth),
                GUILayout.Height(RowHeight),
                GUILayout.MaxHeight(RowHeight));
            GUILayout.Label(NpcPreset.DisplayName(mind.PresetId) + "  (" + mind.PresetId + ")", wrapLabel);
            GUILayout.Label(string.Format(
                "E {0:0.00}   C {1:0.00}   P {2:0.00}   A {3:0.00}   anger {4:0.00}",
                mind.Extraversion, mind.Conscientiousness, mind.Pleasure, mind.Arousal, mind.Anger), wrapLabel);
            var weights = mind.Weight("stay", "leave", "haggle");
            var move = NpcDemoLines.TopMove(weights);
            GUILayout.Label("Ranked move: " + move, wrapLabel);
            GUILayout.Label(NpcDemoLines.Pick(mind.PresetId, mind.Arousal, mind.Anger, move), wrapLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        void RandomizePair()
        {
            var ids = NpcPreset.AllIds;
            if (ids == null || ids.Count < 2)
                return;

            var a = Random.Range(0, ids.Count);
            var b = Random.Range(0, ids.Count);
            if (b == a)
                b = (b + 1) % ids.Count;

            var seedA = Random.Range(1, int.MaxValue);
            var seedB = Random.Range(1, int.MaxValue);
            left.Configure(ids[a], default, seedA);
            right.Configure(ids[b], default, seedB);

            var next = Random.Range(0, NpcDemoBeats.All.Length);
            if (NpcDemoBeats.All.Length > 1 && ReferenceEquals(NpcDemoBeats.All[next], beat))
                next = (next + 1) % NpcDemoBeats.All.Length;
            beat = NpcDemoBeats.All[next];
            lastJson = null;
            ResetRun();

            status = NpcPreset.DisplayName(ids[a]) + " and " + NpcPreset.DisplayName(ids[b])
                     + ". Beat: " + beat.Title + ". Button captions changed; OCC kinds did not.";
        }

        void ResetRun()
        {
            elapsed = 0f;
            capped = false;
            leftTrace.Clear();
            rightTrace.Clear();
            sampleAcc = 0f;
        }
    }
}
