using System;
using Archetypes;

namespace RossSim.NpcHost
{
    /// <summary>
    /// Resolves a public catalog id from compile-time rows first, then embedded JSON.
    /// Compile-time hits avoid CatalogJson reflection on IL2CPP.
    /// </summary>
    public static class NpcPreset
    {
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
