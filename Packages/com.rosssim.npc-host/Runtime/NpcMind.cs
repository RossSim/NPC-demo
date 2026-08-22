using System.Collections.Generic;
using Archetypes;
using PersonalityEngine;
using PersonalityEngine.Providers.Occ;
using PersonalityEngine.Providers.Ocean;
using PersonalityEngine.Providers.Pad;
using UnityEngine;

namespace RossSim.NpcHost
{
    /// <summary>
    /// One Personality Engine mind on a GameObject. Seeded from an Archetypes catalog id.
    /// Idle decay runs in <see cref="Update"/>. Host-tagged events go through <see cref="Notify(HostEventKind,float,string)"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-100)]
    public sealed class NpcMind : MonoBehaviour
    {
        [SerializeField] string presetId = "village-smith";
        [SerializeField] JitterTier tier = JitterTier.Named;
        [Tooltip("0 = catalog midpoints (deterministic). Any other value jittered inside the band.")]
        [SerializeField] int jitterSeed;
        [SerializeField] HostEventMap eventMap;

        AffectEngine engine;

        public string PresetId => presetId;
        public AffectEngine Engine => engine;
        public AffectSnapshot Snapshot => engine?.Snapshot;

        public float Pleasure => Read(PadMood.PleasureKey);
        public float Arousal => Read(PadMood.ArousalKey);
        public float Dominance => Read(PadMood.DominanceKey);
        public float Anger => Read(OccEmotion.AngerKey);
        public float Extraversion => Read(OceanPersonality.ExtraversionKey);
        public float Conscientiousness => Read(OceanPersonality.ConscientiousnessKey);

        void Awake() => Rebuild();

        void Update()
        {
            engine?.Tick(Time.deltaTime);
        }

        public void Configure(string id, JitterTier jitterTier = JitterTier.Named, int seed = 0)
        {
            presetId = id;
            tier = jitterTier;
            jitterSeed = seed;
            Rebuild();
        }

        public void Rebuild()
        {
            var preset = NpcPreset.Resolve(presetId);
            var options = new BuildOptions
            {
                Tier = tier,
                Seed = jitterSeed == 0 ? (int?)null : jitterSeed
            };
            engine = PresetBuilder.Build(preset, options);
            engine.Tick(0f);
        }

        public void Notify(HostEventKind kind, float intensity = 1f, string otherId = null)
        {
            if (engine == null)
                Rebuild();
            engine.Tick(HostEventFactory.Create(kind, intensity, otherId), 0f);
        }

        public void Notify(WorldEvent ev)
        {
            if (ev == null)
                throw new System.ArgumentNullException(nameof(ev));
            if (engine == null)
                Rebuild();
            engine.Tick(ev, 0f);
        }

        public bool NotifyWorld(string worldVerb, float intensity = 1f, string otherId = null)
        {
            if (eventMap == null)
                return false;
            if (!eventMap.TryCreate(worldVerb, out var ev, intensity, otherId))
                return false;
            Notify(ev);
            return true;
        }

        public IReadOnlyDictionary<string, float> Weight(params string[] actionIds)
        {
            if (engine == null)
                Rebuild();
            return engine.WeightActions(actionIds);
        }

        public string SaveToJson()
        {
            if (engine == null)
                Rebuild();
            return NpcPersist.ToJson(engine);
        }

        public void LoadFromJson(string json)
        {
            Rebuild();
            NpcPersist.Apply(engine, json);
        }

        float Read(string key) => engine?.Snapshot.GetOrDefault(key) ?? 0f;
    }
}
