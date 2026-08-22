using System;
using System.Text.Json;
using PersonalityEngine;

namespace RossSim.NpcHost
{
    /// <summary>
    /// JSON round-trip for <see cref="AffectPersist"/>. Rebuild the same composition before <see cref="Apply"/>.
    /// Cap <paramref name="json"/> size yourself if the blob came from mods or other players.
    /// </summary>
    public static class NpcPersist
    {
        public const int UntrustedMaxChars = 256 * 1024;

        static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static string ToJson(AffectEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            return JsonSerializer.Serialize(engine.Export(), Options);
        }

        public static void Apply(AffectEngine engine, string json, int maxChars = UntrustedMaxChars)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Persist JSON is empty.", nameof(json));
            if (json.Length > maxChars)
                throw new ArgumentException("Persist JSON exceeds the size cap.", nameof(json));

            var blob = JsonSerializer.Deserialize<AffectPersist>(json, Options)
                ?? throw new ArgumentException("Persist JSON deserialized to null.", nameof(json));
            engine.Import(blob);
            engine.Tick(0f);
        }
    }
}
