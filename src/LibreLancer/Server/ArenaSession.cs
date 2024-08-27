using LibreLancer.Data.Arena;
using LibreLancer.Data.Universe;
using LibreLancer.GameData;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.IO;
using System.Linq;

namespace LibreLancer.Server
{
    public class ArenaSession
    {
        public GameServer Server;
        ArenaMap CurrentMap;
        Random random = new Random();
        double startTime;
        CapturePoint[] CapPoints = Array.Empty<CapturePoint>();
        int SecondsToCapture = 30;

        public ArenaSession(GameServer server)
        {
            this.Server = server;
        }

        void NextMap()
        {
            foreach (var capPoint in this.CapPoints)
            {
                if (capPoint.World is ServerWorld world)
                {
                    world.ContainsCapturePoint = false;
                }
            }

            var maps = Server.GameData.Ini.ArenaMaps.Maps;
            this.CurrentMap = maps[random.Next(maps.Count)];
            this.CapPoints = new CapturePoint[this.CurrentMap.CapturePoints.Count];

            foreach (int i in Enumerable.Range(0, this.CapPoints.Length))
            {
                var system = Server.GameData.Systems.Get(this.CurrentMap.CapturePoints[i].System);
                this.CapPoints[i] = new CapturePoint
                {
                    FactionShips = new byte[this.CurrentMap.Factions.Count],
                    OwningFaction = -1,
                    Progress = 0,
                    FactionWithProgress = -1,
                    Template = this.CurrentMap.CapturePoints[i],
                };
                this.Server.Worlds.RequestWorld(system,
                    w =>
                    {
                        this.CapPoints[i].World = w;
                        w.ContainsCapturePoint = true;
                    },
                    Array.Empty<PreloadObject>()
                );
            }

            foreach (var player in Server.AllPlayers)
            {
                player.ArenaFaction = -1;
                player.RpcClient.UpdateArenaMap(this.CurrentMap.Nickname);
                //todo: reinitialize characters.
                player.RpcClient.UpdateArenaFaction(-1);
            }

            this.startTime = Server.TotalTime;
        }

        public void Update(TimeSpan timeDelta) //called from the main loop
        {
            if (this.CurrentMap is null)
            {
                this.NextMap();
            }

            var progress = timeDelta.TotalSeconds / SecondsToCapture;
            foreach (var capPoint in this.CapPoints)
            {
                if (capPoint.World is null)
                {
                    continue;
                }

                int cappingFaction = -1;
                foreach (int iFaction in Enumerable.Range(0, capPoint.FactionShips.Length))
                {
                    if (capPoint.FactionShips[iFaction] > 0)
                    {
                        if (cappingFaction > 0)
                        {
                            //multiple factions capping, so nobody's capping.
                            cappingFaction = -1;
                            break;
                        }
                        else
                        {
                            cappingFaction = iFaction;
                        }
                    }
                }

                if (cappingFaction >= 0 && cappingFaction == capPoint.OwningFaction) //faction capping its own point
                {
                    capPoint.Progress -= progress * 2;
                }
                else if (cappingFaction >= 0) //faction capping another's point.
                {
                    if (cappingFaction == capPoint.FactionWithProgress || cappingFaction == -1) //making own progress
                    {
                        capPoint.FactionWithProgress = (sbyte)cappingFaction;
                        capPoint.Progress += progress;
                    }
                    else //resetting somebody else's progress
                    {
                        capPoint.Progress -= progress * 2;
                        if (capPoint.Progress < 0)
                        {
                            capPoint.Progress = -capPoint.Progress / 2;
                            capPoint.FactionWithProgress = (sbyte)cappingFaction;
                        }
                    }

                    if (capPoint.Progress >= 1)
                    {
                        capPoint.OwningFaction = capPoint.FactionWithProgress;
                        capPoint.Progress = 0;
                    }
                }
                else //nobody's capping
                {
                    capPoint.Progress -= progress;
                }

                capPoint.Progress = MathHelper.Clamp(capPoint.Progress, 0, 1);
                if (capPoint.Progress == 0)
                {
                    capPoint.FactionWithProgress = 0;
                }

                Console.WriteLine(capPoint.ToString());

                //count the players close enough to the object.
                var capObject = capPoint.World.GameWorld.GetObject(capPoint.Template.Object);
                for (int i = 0; i < capPoint.FactionShips.Length; i++)
                {
                    capPoint.FactionShips[i] = 0;
                }
                foreach (var player in capPoint.World.Players)
                {
                    if (player.Key.ArenaFaction < 0)
                    {
                        continue;
                    }

                    var delta = player.Value.LocalTransform.Position - capObject.LocalTransform.Position;
                    if (Math.Abs(delta.Length()) < 1000)
                    {
                        Console.WriteLine($"capping!");
                        capPoint.FactionShips[player.Key.ArenaFaction]++;
                    }
                }
            }
        }

        public void BeginGame(Player p)
        {
            p.RpcClient.UpdateArenaMap(CurrentMap.Nickname);
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

            if (CurrentMap.Factions[index].StarterShip is string starterShip && starterShip.Length > 0)
            {
                player.ApplyShipPackage(starterShip);
            }
            if (CurrentMap.Factions[index].Base is string startBase && startBase.Length > 0)
            {
                player.ForceLand(startBase);
            }

            player.RpcClient.UpdateArenaFaction(index);
            //todo: actually set the affiliation
        }
    }

    public class CapturePoint
    {
        public sbyte OwningFaction;
        public byte[] FactionShips;
        public sbyte FactionWithProgress;
        public double Progress;
        public ServerWorld World;
        public ArenaCapturePoint Template;

        public override string ToString()
        {
            return $@"Owning Faction: {OwningFaction}, Ships: {FactionShips}, Faction with progress: {FactionWithProgress}, Progress: {Progress}";
        }
    }
}
