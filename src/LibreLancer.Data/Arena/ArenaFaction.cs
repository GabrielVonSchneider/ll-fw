using LibreLancer.Data.Ini;
using LibreLancer.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreLancer.Data.Arena
{
    [InheritSection]
    public class ArenaFaction
    {
        [Entry("nickname")]
        public string Nickname; //rep group e.g. li_n_grp
        [Entry("base")]
        public string Base;
        [Entry("color")]
        public string Color;
        [Entry("starter_ship")]
        public string StarterShip;
    }
}
