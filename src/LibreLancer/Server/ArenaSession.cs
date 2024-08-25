using LibreLancer.Data.Arena;
using System;

namespace LibreLancer.Server
{
    public class ArenaSession
    {
        public GameServer Server;
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
                player.ArenaFaction = -1;
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
                CurrentMap.Nickname,
                p.ArenaFaction
            );
        }

        public void PickArenaFaction(Player player, int index)
        {
            if (CurrentMap == null)
            {
                return;
            }
            if (index < 0 || index >= CurrentMap.Factions.Count)
            {
                return;
            }

            player.ArenaFaction = index;

            if (CurrentMap.Factions[index].Base is string startBase)
            {
                player.ForceLand(startBase);
            }

            SendArenaInfo(player);
        }
    }
}
