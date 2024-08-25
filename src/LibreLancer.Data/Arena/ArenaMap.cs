using LibreLancer.Data.Ini;
using LibreLancer.Ini;
using System.Collections.Generic;

namespace LibreLancer.Data.Arena
{
    [InheritSection]
    [SelfSection("map")]
    public class ArenaMap : IniFile
    {
        [Entry("nickname")]
        public string Nickname;
        [Entry("title")]
        public string Title;
        [Entry("mode")]
        public string Mode;

        [Entry("file")]
        public string File;

        [Section("faction")]
        public List<ArenaFaction> Factions = new List<ArenaFaction>();

        [Section("capture_point")]
        public List<ArenaCapturePoint> CapturePoints = new List<ArenaCapturePoint>();

        public void ParseAndFill(string path, FreelancerData fldata)
		{
            ParseAndFill(path + File, fldata.VFS);
        }
    }
}
