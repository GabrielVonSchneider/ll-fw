using LibreLancer.Data.Ini;
using LibreLancer.Ini;
using System.Collections.Generic;

namespace LibreLancer.Data.Arena
{
    [InheritSection]
    public class ArenaMap
    {
        [Entry("nickname")]
        public string Nickname;
        [Entry("title")]
        public string Title;
        [Entry("mode")]
        public string Mode;

        [Entry("faction")]
        public List<ArenaFaction> Factions = new List<ArenaFaction>();

        [Entry("capture_point")]
        public List<ArenaCapturePoint> CapturePoints = new List<ArenaCapturePoint>();
    }
}
