using LibreLancer.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreLancer.Data.Arena
{
    public class ArenaMapsIni : IniFile
    {
        [Section("map")]
        public List<ArenaMap> Maps = new List<ArenaMap>();
        public void ParseAllInis(IEnumerable<string> paths, FreelancerData fldata)
		{
            ParseAndFill(paths, fldata.VFS);
        }
    }
}
