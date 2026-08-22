using RossSim.NpcHost;

namespace RossSim.NpcDemo
{
    /// <summary>
    /// Pre-authored host-event captions. The demo does not call a language model.
    /// Same OCC kind can be framed as theft, a slight, or harm to a friend.
    /// </summary>
    public sealed class NpcDemoBeat
    {
        public string Title;
        public string Anger;
        public string Gratitude;
        public string Threat;
        public string ThreatPassed;
        public string NeedMet;
        public string AngerNote;
        public string GratitudeNote;
        public string ThreatNote;
        public string ThreatPassedNote;
        public string NeedMetNote;
    }

    public static class NpcDemoBeats
    {
        public static readonly NpcDemoBeat[] All =
        {
            new NpcDemoBeat
            {
                Title = "The yard",
                Anger = "Insulted me",
                AngerNote = "They hear it as yours.",
                Gratitude = "Brought a gift",
                GratitudeNote = "Tagged as gratitude — the engine did not infer that.",
                Threat = "Danger in the room",
                ThreatNote = "Something is still a threat.",
                ThreatPassed = "Danger passed",
                ThreatPassedNote = "The danger is gone. Mood still has to catch up.",
                NeedMet = "A need was met",
                NeedMetNote = "A need was satisfied."
            },
            new NpcDemoBeat
            {
                Title = "The purse",
                Anger = "Stole from me",
                AngerNote = "Theft, tagged as anger at you.",
                Gratitude = "Returned my purse",
                GratitudeNote = "A return, tagged as gratitude.",
                Threat = "Cutpurse in the square",
                ThreatNote = "A thief is still nearby.",
                ThreatPassed = "The thief ran",
                ThreatPassedNote = "The cutpurse is gone.",
                NeedMet = "Found my coin",
                NeedMetNote = "The missing coin is back in hand."
            },
            new NpcDemoBeat
            {
                Title = "Kin",
                Anger = "Hurt my friend",
                AngerNote = "Harm to someone they like, tagged as anger at you.",
                Gratitude = "Helped my kin",
                GratitudeNote = "Aid to family, tagged as gratitude.",
                Threat = "Knife at their throat",
                ThreatNote = "A friend is still in danger.",
                ThreatPassed = "They're safe",
                ThreatPassedNote = "The knife is down.",
                NeedMet = "They ate",
                NeedMetNote = "Hunger for a friend was met."
            },
            new NpcDemoBeat
            {
                Title = "The stall",
                Anger = "Shortchanged me",
                AngerNote = "A bad count, tagged as anger at you.",
                Gratitude = "Paid a fair price",
                GratitudeNote = "Honest coin, tagged as gratitude.",
                Threat = "Taxman at the door",
                ThreatNote = "The levy is still coming.",
                ThreatPassed = "Taxman left",
                ThreatPassedNote = "The collector moved on.",
                NeedMet = "Sale closed",
                NeedMetNote = "The stall made its sale."
            },
            new NpcDemoBeat
            {
                Title = "The road",
                Anger = "Spit on my name",
                AngerNote = "A public slight, tagged as anger at you.",
                Gratitude = "Shared the fire",
                GratitudeNote = "A camp share, tagged as gratitude.",
                Threat = "Wolves on the ridge",
                ThreatNote = "The pack is still out.",
                ThreatPassed = "Wolves moved on",
                ThreatPassedNote = "The ridge is quiet.",
                NeedMet = "Reached camp",
                NeedMetNote = "The day's march is done."
            },
            new NpcDemoBeat
            {
                Title = "The guild",
                Anger = "Blamed me for the loss",
                AngerNote = "A false charge, tagged as anger at you.",
                Gratitude = "Spoke for me",
                GratitudeNote = "A defense, tagged as gratitude.",
                Threat = "Audit at dawn",
                ThreatNote = "The books are still under review.",
                ThreatPassed = "Audit closed",
                ThreatPassedNote = "The clerks packed up.",
                NeedMet = "Quota met",
                NeedMetNote = "The hall's number was hit."
            },
            new NpcDemoBeat
            {
                Title = "The inn",
                Anger = "Started a brawl",
                AngerNote = "A fight in their house, tagged as anger at you.",
                Gratitude = "Bought a round",
                GratitudeNote = "Drinks on you, tagged as gratitude.",
                Threat = "Roof's on fire",
                ThreatNote = "The rafters are still burning.",
                ThreatPassed = "Fire's out",
                ThreatPassedNote = "The buckets won.",
                NeedMet = "Beds are full",
                NeedMetNote = "The house made its night."
            },
            new NpcDemoBeat
            {
                Title = "The watch",
                Anger = "Lied to the watch",
                AngerNote = "A false report, tagged as anger at you.",
                Gratitude = "Turned in the thief",
                GratitudeNote = "A catch, tagged as gratitude.",
                Threat = "Riot on the quay",
                ThreatNote = "The crowd is still up.",
                ThreatPassed = "Crowd broke",
                ThreatPassedNote = "The quay is clear.",
                NeedMet = "Gate locked",
                NeedMetNote = "The wall is shut for the night."
            },
            new NpcDemoBeat
            {
                Title = "The field",
                Anger = "Trampled the crop",
                AngerNote = "Ruined work, tagged as anger at you.",
                Gratitude = "Mended the fence",
                GratitudeNote = "A repair, tagged as gratitude.",
                Threat = "Storm on the field",
                ThreatNote = "The weather is still a threat.",
                ThreatPassed = "Storm passed",
                ThreatPassedNote = "The sky opened.",
                NeedMet = "Harvest in",
                NeedMetNote = "The crop is under roof."
            },
            new NpcDemoBeat
            {
                Title = "The sickbed",
                Anger = "Mocked the sick",
                AngerNote = "A cruel joke, tagged as anger at you.",
                Gratitude = "Brought a tonic",
                GratitudeNote = "A bottle, tagged as gratitude.",
                Threat = "Fever spiked",
                ThreatNote = "The illness is still rising.",
                ThreatPassed = "Fever broke",
                ThreatPassedNote = "The worst is over.",
                NeedMet = "They slept",
                NeedMetNote = "Rest, at last."
            },
            new NpcDemoBeat
            {
                Title = "The rival",
                Anger = "Mocked my craft",
                AngerNote = "A slight on the work, tagged as anger at you.",
                Gratitude = "Praised the work",
                GratitudeNote = "A public compliment, tagged as gratitude.",
                Threat = "Challenge in the yard",
                ThreatNote = "A fight is still offered.",
                ThreatPassed = "They stood down",
                ThreatPassedNote = "The challenge was withdrawn.",
                NeedMet = "Job done",
                NeedMetNote = "The piece is finished."
            },
            new NpcDemoBeat
            {
                Title = "The camp",
                Anger = "Stole the watch",
                AngerNote = "Left them unguarded, tagged as anger at you.",
                Gratitude = "Took my watch for me",
                GratitudeNote = "A shift covered, tagged as gratitude.",
                Threat = "Horn in the dark",
                ThreatNote = "An alarm is still up.",
                ThreatPassed = "All clear",
                ThreatPassedNote = "The horn stopped.",
                NeedMet = "Rations issued",
                NeedMetNote = "The line was fed."
            },
            new NpcDemoBeat
            {
                Title = "The hall",
                Anger = "Called me oathbreaker",
                AngerNote = "A public charge, tagged as anger at you.",
                Gratitude = "Swore I told truth",
                GratitudeNote = "A witness, tagged as gratitude.",
                Threat = "Sentence at noon",
                ThreatNote = "Judgment is still coming.",
                ThreatPassed = "Reprieve",
                ThreatPassedNote = "The sentence was stayed.",
                NeedMet = "Fine paid",
                NeedMetNote = "The hall's due was met."
            }
        };
    }
}
