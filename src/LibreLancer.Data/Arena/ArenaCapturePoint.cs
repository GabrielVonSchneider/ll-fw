using LibreLancer.Data.Ini;
using LibreLancer.Ini;
using System.Collections.Generic;

namespace LibreLancer.Data.Arena
{
    [InheritSection]
    public class ArenaCapturePoint
    {
        [Entry("object")]
        public string Object;
    }
}
