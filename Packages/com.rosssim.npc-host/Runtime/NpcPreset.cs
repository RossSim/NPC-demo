using System;
using System.Collections.Generic;
using Archetypes;

namespace RossSim.NpcHost
{
    /// <summary>
    /// Resolves a public catalog id from compile-time rows first, then embedded JSON.
    /// Compile-time hits avoid CatalogJson reflection on IL2CPP.
    /// </summary>
    public static class NpcPreset
    {
        static string[] ids;

        public static IReadOnlyList<string> AllIds
        {
            get
            {
                if (ids != null)
                    return ids;
                var seeds = Catalog.Seeds;
                ids = new string[seeds.Count];
                for (var i = 0; i < seeds.Count; i++)
                    ids[i] = seeds[i].Id;
                return ids;
            }
        }

        public static string DisplayName(string presetId)
        {
            if (string.IsNullOrWhiteSpace(presetId))
                return "";
            var parts = presetId.Split('-');
            for (var i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length == 0)
                    continue;
                parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1);
            }

            return string.Join(" ", parts);
        }

        public static MindPreset Resolve(string presetId)
        {
            if (string.IsNullOrWhiteSpace(presetId))
                throw new ArgumentException("Preset id is required.", nameof(presetId));

            foreach (var seed in Catalog.Seeds)
            {
                if (string.Equals(seed.Id, presetId, StringComparison.Ordinal))
                    return seed;
            }

            return CatalogJson.Load(presetId);
        }
    }
}
