using LibreLancer.Data.Arena;
using LibreLancer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WattleScript.Interpreter;

namespace LibreLancer.Client
{
    [WattleScriptUserData]
    public class ArenaClient
    {
        CGameSession session;

        public ArenaClient(CGameSession session)
        {
            this.session = session;
        }

        public PickableFaction[] GetAvailableFactions()
        {
            var map = this.session.Game.GameData.Ini.ArenaMaps.Maps.First(m => m.Nickname == this.session.ArenaMap);
            return map.Factions.Select(f =>
            {
                var faction = this.session.Game.GameData.Factions.Get(f.Nickname);
                return new PickableFaction
                {
                    IdsName = faction.IdsName,
                    Color = f.Color,
                };
            }).ToArray();
        }

        public bool GetAttentionRequired() => this.session.ArenaFaction < 0 && this.session.ArenaMap != null;
        public string GetArenaText()
        {
            if (this.session.ArenaFaction < 0)
            {
                return "Pick your faction!";
            }
            else
            {
                return "Set up your ship!";
            }
        }

        public void PickFaction(int i)
        {
            this.session.RpcServer.PickArenaFaction(i);
        }
    }

    [WattleScriptUserData]
    public class PickableFaction
    {
        public int IdsName;
        public string Color;
    }
}
