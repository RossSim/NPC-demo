using RossSim.NpcHost;
using UnityEngine;

namespace RossSim.NpcDemo
{
    /// <summary>
    /// Wall-clock samples at 60 Hz. The x-axis always starts at t=0 on the left
    /// and compresses as more samples arrive, up to 60 seconds.
    /// </summary>
    public sealed class NpcDemoTrace
    {
        public const int MaxSamples = 60 * 60;
        public const float MaxSeconds = 60f;
        public const float SampleHz = 60f;

        public readonly float[] Extraversion = new float[MaxSamples];
        public readonly float[] Conscientiousness = new float[MaxSamples];
        public readonly float[] Pleasure = new float[MaxSamples];
        public readonly float[] Arousal = new float[MaxSamples];
        public readonly float[] Anger = new float[MaxSamples];
        public int Count { get; private set; }

        public void Clear() => Count = 0;

        public void Push(NpcMind mind)
        {
            if (mind == null || Count >= MaxSamples)
                return;
            Extraversion[Count] = mind.Extraversion;
            Conscientiousness[Count] = mind.Conscientiousness;
            Pleasure[Count] = mind.Pleasure;
            Arousal[Count] = mind.Arousal;
            Anger[Count] = mind.Anger;
            Count++;
        }
    }

    public static class NpcDemoChart
    {
        const float YMin = -0.2f;
        const float YMax = 1.2f;
        const float LineWidth = 2f;

        static readonly Color Extraversion = new Color(0.25f, 0.75f, 0.95f);
        static readonly Color Conscientiousness = new Color(0.75f, 0.75f, 0.78f);
        static readonly Color Pleasure = new Color(0.35f, 0.82f, 0.40f);
        static readonly Color Arousal = new Color(0.95f, 0.62f, 0.20f);
        static readonly Color Anger = new Color(0.92f, 0.28f, 0.28f);
        static readonly Color Grid = new Color(1f, 1f, 1f, 0.18f);

        public static void Draw(Rect rect, NpcDemoTrace trace, float elapsedSeconds)
        {
            GUI.Box(rect, GUIContent.none);
            var legend = new Rect(rect.x + 100f, rect.y + 2f, rect.width - 220f, 18f);
            GUI.Label(legend, "E   C   P   A   anger");
            var time = new Rect(rect.xMax - 110f, rect.y + 2f, 104f, 18f);
            GUI.Label(time, elapsedSeconds.ToString("0.0") + "s / 60s");
            PaintSwatches(new Rect(rect.x + 6f, rect.y + 4f, 90f, 16f));

            var plot = new Rect(rect.x + 4f, rect.y + 22f, rect.width - 8f, rect.height - 26f);
            if (Event.current.type != EventType.Repaint)
                return;

            GUI.BeginGroup(plot);
            var local = new Rect(0f, 0f, plot.width, plot.height);
            DrawGrid(local);
            if (trace != null && trace.Count >= 2)
            {
                DrawSeries(local, trace.Extraversion, trace.Count, Extraversion);
                DrawSeries(local, trace.Conscientiousness, trace.Count, Conscientiousness);
                DrawSeries(local, trace.Pleasure, trace.Count, Pleasure);
                DrawSeries(local, trace.Arousal, trace.Count, Arousal);
                DrawSeries(local, trace.Anger, trace.Count, Anger);
            }

            GUI.EndGroup();
        }

        static void PaintSwatches(Rect r)
        {
            var x = r.x;
            Swatch(ref x, r.y, Extraversion);
            Swatch(ref x, r.y, Conscientiousness);
            Swatch(ref x, r.y, Pleasure);
            Swatch(ref x, r.y, Arousal);
            Swatch(ref x, r.y, Anger);
        }

        static void Swatch(ref float x, float y, Color color)
        {
            var old = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(x, y + 4f, 10f, 10f), Texture2D.whiteTexture);
            GUI.color = old;
            x += 18f;
        }

        static void DrawGrid(Rect rect)
        {
            DrawLine(XPoint(rect, 0f), XPoint(rect, 1f), Grid, 1f);
            DrawLine(YPoint(rect, 0.5f, true), YPoint(rect, 0.5f, false), Grid, 1f);
            DrawLine(YPoint(rect, 0f, true), YPoint(rect, 0f, false), Grid, 1f);
        }

        static Vector2 XPoint(Rect rect, float t) =>
            new Vector2(rect.x + t * rect.width, rect.yMax);

        static Vector2 YPoint(Rect rect, float value, bool left)
        {
            var y01 = Mathf.InverseLerp(YMin, YMax, value);
            var y = rect.yMax - y01 * rect.height;
            return new Vector2(left ? rect.x : rect.xMax, y);
        }

        static void DrawSeries(Rect rect, float[] data, int count, Color color)
        {
            var segments = Mathf.Min(count - 1, Mathf.Max(1, (int)rect.width));
            var denom = (float)segments;
            var last = Point(rect, 0, count, data[0]);
            for (var s = 1; s <= segments; s++)
            {
                var i = (int)(s / denom * (count - 1));
                var next = Point(rect, i, count, data[i]);
                DrawLine(last, next, color, LineWidth);
                last = next;
            }
        }

        static Vector2 Point(Rect rect, int i, int count, float value)
        {
            var t = count <= 1 ? 0f : i / (float)(count - 1);
            var y01 = Mathf.InverseLerp(YMin, YMax, value);
            return new Vector2(rect.x + t * rect.width, rect.yMax - y01 * rect.height);
        }

        static void DrawLine(Vector2 a, Vector2 b, Color color, float thickness)
        {
            var delta = b - a;
            var length = delta.magnitude;
            if (length < 0.25f)
                return;
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            var matrix = GUI.matrix;
            var saved = GUI.color;
            GUI.color = color;
            GUIUtility.RotateAroundPivot(angle, a);
            GUI.DrawTexture(new Rect(a.x, a.y - thickness * 0.5f, length, thickness), Texture2D.whiteTexture);
            GUI.matrix = matrix;
            GUI.color = saved;
        }
    }
}
