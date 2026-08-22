using System.Collections.Generic;
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

        static readonly Color32 Extraversion = new Color32(64, 191, 242, 255);
        static readonly Color32 Conscientiousness = new Color32(191, 191, 199, 255);
        static readonly Color32 Pleasure = new Color32(89, 209, 102, 255);
        static readonly Color32 Arousal = new Color32(242, 158, 51, 255);
        static readonly Color32 Anger = new Color32(235, 71, 71, 255);
        static readonly Color32 Grid = new Color32(255, 255, 255, 46);
        static readonly Color32 Background = new Color32(28, 28, 28, 255);

        static readonly Dictionary<NpcDemoTrace, PlotBuf> Buffers = new Dictionary<NpcDemoTrace, PlotBuf>();

        sealed class PlotBuf
        {
            public Texture2D Tex;
            public Color32[] Pixels;
            public int W;
            public int H;
        }

        public static void DrawPageLegend(GUIStyle label)
        {
            GUILayout.BeginHorizontal();
            LegendItem(Extraversion, "E Extraversion", label);
            LegendItem(Conscientiousness, "C Conscientiousness", label);
            LegendItem(Pleasure, "P Pleasure", label);
            LegendItem(Arousal, "A Arousal", label);
            LegendItem(Anger, "Anger", label);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        static void LegendItem(Color32 color, string name, GUIStyle label)
        {
            var swatch = GUILayoutUtility.GetRect(14f, 14f, GUILayout.Width(14f), GUILayout.Height(14f));
            if (Event.current.type == EventType.Repaint)
            {
                var old = GUI.color;
                GUI.color = color;
                GUI.DrawTexture(swatch, Texture2D.whiteTexture);
                GUI.color = old;
            }

            GUILayout.Label(name, label, GUILayout.ExpandWidth(false));
            GUILayout.Space(16f);
        }

        public static void Draw(Rect rect, NpcDemoTrace trace, float elapsedSeconds)
        {
            GUI.Box(rect, GUIContent.none);
            var time = new Rect(rect.xMax - 110f, rect.y + 2f, 104f, 18f);
            GUI.Label(time, elapsedSeconds.ToString("0.0") + "s / 60s");

            var plot = new Rect(rect.x + 4f, rect.y + 22f, rect.width - 8f, rect.height - 26f);
            if (plot.width < 8f || plot.height < 8f)
                return;
            if (Event.current.type != EventType.Repaint)
                return;

            var buf = Rasterize(trace, Mathf.Max(8, (int)plot.width), Mathf.Max(8, (int)plot.height));
            GUI.BeginClip(plot);
            GUI.DrawTexture(new Rect(0f, 0f, plot.width, plot.height), buf.Tex, ScaleMode.StretchToFill, false);
            GUI.EndClip();
        }

        static PlotBuf Rasterize(NpcDemoTrace trace, int w, int h)
        {
            var key = trace ?? DummyKey;
            if (!Buffers.TryGetValue(key, out var buf))
            {
                buf = new PlotBuf();
                Buffers[key] = buf;
            }

            if (buf.Tex == null || buf.W != w || buf.H != h)
            {
                if (buf.Tex != null)
                    Object.Destroy(buf.Tex);
                buf.W = w;
                buf.H = h;
                buf.Pixels = new Color32[w * h];
                buf.Tex = new Texture2D(w, h, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp,
                    hideFlags = HideFlags.HideAndDontSave
                };
            }

            var px = buf.Pixels;
            var bg = Background;
            for (var i = 0; i < px.Length; i++)
                px[i] = bg;

            DrawHLine(px, w, h, YToPixel(h, 0f), Grid);
            DrawHLine(px, w, h, YToPixel(h, 0.5f), Grid);
            DrawHLine(px, w, h, YToPixel(h, 1f), Grid);

            if (trace != null && trace.Count >= 2)
            {
                DrawSeries(px, w, h, trace.Extraversion, trace.Count, Extraversion);
                DrawSeries(px, w, h, trace.Conscientiousness, trace.Count, Conscientiousness);
                DrawSeries(px, w, h, trace.Pleasure, trace.Count, Pleasure);
                DrawSeries(px, w, h, trace.Arousal, trace.Count, Arousal);
                DrawSeries(px, w, h, trace.Anger, trace.Count, Anger);
            }

            buf.Tex.SetPixels32(px);
            buf.Tex.Apply(false, false);
            return buf;
        }

        static readonly NpcDemoTrace DummyKey = new NpcDemoTrace();

        static void DrawSeries(Color32[] px, int w, int h, float[] data, int count, Color32 color)
        {
            var lastX = 0;
            var lastY = YToPixel(h, data[0]);
            var denom = (float)(w - 1);
            for (var x = 1; x < w; x++)
            {
                var i = (int)(x / denom * (count - 1));
                if (i >= count)
                    i = count - 1;
                var y = YToPixel(h, data[i]);
                DrawClippedLine(px, w, h, lastX, lastY, x, y, color);
                lastX = x;
                lastY = y;
            }
        }

        static int YToPixel(int h, float value)
        {
            var y01 = Mathf.Clamp01(Mathf.InverseLerp(YMin, YMax, value));
            return Mathf.Clamp((int)(y01 * (h - 1)), 0, h - 1);
        }

        static void DrawHLine(Color32[] px, int w, int h, int y, Color32 color)
        {
            if ((uint)y >= (uint)h)
                return;
            var row = y * w;
            for (var x = 0; x < w; x++)
                px[row + x] = color;
        }

        static void DrawClippedLine(Color32[] px, int w, int h, int x0, int y0, int x1, int y1, Color32 color)
        {
            var dx = Mathf.Abs(x1 - x0);
            var dy = Mathf.Abs(y1 - y0);
            var sx = x0 < x1 ? 1 : -1;
            var sy = y0 < y1 ? 1 : -1;
            var err = dx - dy;
            var x = x0;
            var y = y0;
            while (true)
            {
                Put(px, w, h, x, y, color);
                Put(px, w, h, x, y + 1, color);
                if (x == x1 && y == y1)
                    break;
                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }
        }

        static void Put(Color32[] px, int w, int h, int x, int y, Color32 color)
        {
            if ((uint)x >= (uint)w || (uint)y >= (uint)h)
                return;
            px[y * w + x] = color;
        }
    }
}
