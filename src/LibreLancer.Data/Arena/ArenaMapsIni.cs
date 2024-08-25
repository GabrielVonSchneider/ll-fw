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

        public void ParseAndFill(string path, FreelancerData fldata)
		{
            ParseAndFill(path, fldata.VFS);
            foreach (var map in Maps)
            {
                map.ParseAndFill(fldata.VFS.RemovePathComponent(path), fldata);
            }
        }
    }
}
