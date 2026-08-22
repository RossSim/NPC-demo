using System;
using System.Collections.Generic;
using PersonalityEngine;
using UnityEngine;

namespace RossSim.NpcHost
{
    /// <summary>
    /// Designer table: a world verb your game already has ("damage", "gift") onto a host event.
    /// Combat code should call <see cref="TryCreate"/> rather than naming OCC kinds.
    /// </summary>
    [CreateAssetMenu(fileName = "HostEventMap", menuName = "NPC Host/Host Event Map", order = 0)]
    public sealed class HostEventMap : ScriptableObject
    {
        [SerializeField] List<Entry> entries = new List<Entry>();

        public IReadOnlyList<Entry> Entries => entries;

        public bool TryCreate(string worldVerb, out WorldEvent ev, float intensity = 1f, string otherId = null)
        {
            ev = default;
            if (string.IsNullOrWhiteSpace(worldVerb) || entries == null)
                return false;

            for (var i = 0; i < entries.Count; i++)
            {
                var row = entries[i];
                if (row == null || !string.Equals(row.WorldVerb, worldVerb, StringComparison.OrdinalIgnoreCase))
                    continue;

                var other = string.IsNullOrWhiteSpace(otherId) ? row.DefaultOtherId : otherId;
                ev = HostEventFactory.Create(row.Kind, intensity, other);
                return true;
            }

            return false;
        }

        [Serializable]
        public sealed class Entry
        {
            [SerializeField] string worldVerb = "damage";
            [SerializeField] HostEventKind kind = HostEventKind.Harm;
            [SerializeField] string defaultOtherId = HostEventFactory.DefaultOtherId;

            public string WorldVerb => worldVerb;
            public HostEventKind Kind => kind;
            public string DefaultOtherId => defaultOtherId;
        }
    }
}
