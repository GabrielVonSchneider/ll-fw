using LibreLancer.Data.Arena;
using LibreLancer.Net.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreLancer.Server
{
    public enum ArenaState
    {
        TeamPick,
        Setup,
        Gameplay,
    }

    public class ArenaSession
    {
        public GameServer Server;

        ArenaState State;
        ArenaMap CurrentMap;
        Random random = new Random();

        public ArenaSession(GameServer server)
        {
            this.Server = server;
            this.NextMap();
        }

        void NextMap()
        {
            var maps = Server.GameData.Ini.ArenaMaps.Maps;
            this.CurrentMap = maps[random.Next(maps.Count)];

            foreach (var player in Server.AllPlayers)
            {
                player.ArenaFaction = null;
                SendArenaInfo(player);
            }
        }

        public void BeginGame(Player p)
        {
            SendArenaInfo(p);
        }

        private void SendArenaInfo(Player p)
        {
            p.RpcClient.UpdateArena(
                CurrentMap.Title,
                CurrentMap.Factions
                    .Select(f => this.Server.GameData.GetString(this.Server.GameData.Factions.Get(f.Nickname).IdsName))
                    .ToArray(),
                this.State
            );
        }

        void BeamPlayersToBases()
        {
            foreach (var player in Server.AllPlayers)
            {
                var faction = this.CurrentMap.Factions.FirstOrDefault(f => f.Nickname == player.ArenaFaction);
            }
        }
    }
}
