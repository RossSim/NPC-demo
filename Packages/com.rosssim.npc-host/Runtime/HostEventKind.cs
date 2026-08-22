using PersonalityEngine;

namespace RossSim.NpcHost
{
    /// <summary>
    /// Unity-facing names for Personality Engine <see cref="HostEvents"/>.
    /// The mapping is a project convention. The host still chooses the kind;
    /// the engine does not infer that a hit was anger.
    /// </summary>
    public enum HostEventKind
    {
        NeedMet,
        Harm,
        Threat,
        ThreatPassed,
        SelfCredit,
        SelfBlame,
        HappyFor,
        Pity,
        Resent,
        Gloat,
        Like,
        Dislike,
        Anger,
        Gratitude,
        Gratification,
        Remorse
    }

    public static class HostEventFactory
    {
        public const string DefaultOtherId = "player";

        public static WorldEvent Create(HostEventKind kind, float intensity = 1f, string otherId = null)
        {
            var other = string.IsNullOrWhiteSpace(otherId) ? DefaultOtherId : otherId;
            switch (kind)
            {
                case HostEventKind.NeedMet: return HostEvents.NeedMet(intensity);
                case HostEventKind.Harm: return HostEvents.Harm(intensity);
                case HostEventKind.Threat: return HostEvents.Threat(intensity);
                case HostEventKind.ThreatPassed: return HostEvents.ThreatPassed(intensity);
                case HostEventKind.SelfCredit: return HostEvents.SelfCredit(intensity);
                case HostEventKind.SelfBlame: return HostEvents.SelfBlame(intensity);
                case HostEventKind.HappyFor: return HostEvents.HappyFor(other, intensity);
                case HostEventKind.Pity: return HostEvents.Pity(other, intensity);
                case HostEventKind.Resent: return HostEvents.Resent(other, intensity);
                case HostEventKind.Gloat: return HostEvents.Gloat(other, intensity);
                case HostEventKind.Like: return HostEvents.Like(other, intensity);
                case HostEventKind.Dislike: return HostEvents.Dislike(other, intensity);
                case HostEventKind.Anger: return HostEvents.Anger(other, intensity);
                case HostEventKind.Gratitude: return HostEvents.Gratitude(other, intensity);
                case HostEventKind.Gratification: return HostEvents.Gratification(intensity);
                case HostEventKind.Remorse: return HostEvents.Remorse(intensity);
                default: throw new System.ArgumentOutOfRangeException(nameof(kind), kind, "Unknown host event.");
            }
        }
    }
}
