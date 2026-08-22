using System.Collections.Generic;
using RossSim.NpcHost;

namespace RossSim.NpcDemo
{
    /// <summary>
    /// Pre-authored stems. The demo does not call a language model.
    /// </summary>
    public static class NpcDemoLines
    {
        public static string Pick(string presetId, float arousal, float anger, string rankedMove)
        {
            var smith = presetId == "village-smith";
            if (anger >= 0.45f)
                return smith
                    ? "The hammer stays in his hand. \"Say that again.\""
                    : "She does not look at you. \"Keep walking.\"";
            if (arousal >= 0.55f)
                return smith
                    ? "He talks fast over the anvil. \"Make it quick.\""
                    : "Weight on the balls of her feet. \"What.\"";
            if (rankedMove == "leave")
                return smith
                    ? "He nods at the door. \"Forge is closed.\""
                    : "Already half turned. \"I have a ridge to walk.\"";
            if (rankedMove == "haggle")
                return smith
                    ? "\"Price is the price. You heard it.\""
                    : "\"I don't sell. I scout.\"";
            return smith
                ? "Steady hands. \"Need a shoe, or just talk?\""
                : "Quiet. \"Tracks are south. That's all.\"";
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
    }
}
