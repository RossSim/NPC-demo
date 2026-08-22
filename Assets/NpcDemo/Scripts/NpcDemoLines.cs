using System.Collections.Generic;
using RossSim.NpcHost;

namespace RossSim.NpcDemo
{
    /// <summary>
    /// Pre-authored stems. The demo does not call a language model.
    /// Variants are keyed by preset id so two randomized jobs do not share a mouth.
    /// </summary>
    public static class NpcDemoLines
    {
        public static string Pick(string presetId, float arousal, float anger, string rankedMove)
        {
            var name = NpcPreset.DisplayName(presetId);
            var slot = Slot(presetId);
            if (anger >= 0.45f)
                return AngerLine(name, slot);
            if (arousal >= 0.55f)
                return ArousalLine(name, slot);
            if (rankedMove == "leave")
                return LeaveLine(name, slot);
            if (rankedMove == "haggle")
                return HaggleLine(name, slot);
            return CalmLine(name, slot);
        }

        public static string TopMove(IReadOnlyDictionary<string, float> weights)
        {
            var best = "stay";
            var score = float.NegativeInfinity;
            if (weights == null)
                return best;
            foreach (var pair in weights)
            {
                if (pair.Value > score)
                {
                    score = pair.Value;
                    best = pair.Key;
                }
            }

            return best;
        }

        static int Slot(string presetId)
        {
            unchecked
            {
                var h = 17;
                if (presetId != null)
                {
                    for (var i = 0; i < presetId.Length; i++)
                        h = h * 31 + presetId[i];
                }

                if (h < 0)
                    h = -h;
                return h % 3;
            }
        }

        static string AngerLine(string name, int slot)
        {
            switch (slot)
            {
                case 0: return name + " doesn't look at you. \"Don't.\"";
                case 1: return name + " goes still. \"Say that again.\"";
                default: return name + " cuts you off. \"We're done.\"";
            }
        }

        static string ArousalLine(string name, int slot)
        {
            switch (slot)
            {
                case 0: return name + " talks fast. \"Make it quick.\"";
                case 1: return name + " shifts their weight. \"What.\"";
                default: return name + " keeps their hands busy. \"Not now.\"";
            }
        }

        static string LeaveLine(string name, int slot)
        {
            switch (slot)
            {
                case 0: return name + " nods at the door. \"Closed.\"";
                case 1: return name + " is already half turned. \"I have work.\"";
                default: return name + " steps back. \"Later.\"";
            }
        }

        static string HaggleLine(string name, int slot)
        {
            switch (slot)
            {
                case 0: return name + ": \"Price is the price.\"";
                case 1: return name + ": \"That's the offer. Take it.\"";
                default: return name + ": \"I don't owe you a bargain.\"";
            }
        }

        static string CalmLine(string name, int slot)
        {
            switch (slot)
            {
                case 0: return name + " waits. \"Talk, or work?\"";
                case 1: return name + " is quiet. \"I'm listening.\"";
                default: return name + " glances up. \"Go on.\"";
            }
        }
    }
}
