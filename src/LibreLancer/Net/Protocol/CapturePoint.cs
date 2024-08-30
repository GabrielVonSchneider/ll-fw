using WattleScript.Interpreter;

namespace LibreLancer.Net.Protocol
{
    [WattleScriptUserData]
    public class NetCapturePoint
    {
        public sbyte OwningFaction;
        public byte[] FactionShips;
        public sbyte FactionWithProgress;
        public float Progress;

        public static NetCapturePoint Read(PacketReader reader)
        {
            var point = new NetCapturePoint
            {
                OwningFaction = (sbyte)reader.GetByte(),
                FactionWithProgress = (sbyte)reader.GetByte(),
                Progress = reader.GetFloat(),
            };

            //kinda lame since client already knows the faction count, but easier for now
            var nFactions = reader.GetByte();
            point.FactionShips = reader.GetBytes(nFactions);

            return point;
        }

        public void Put(PacketWriter writer)
        {
            writer.Put((byte)OwningFaction);
            writer.Put((byte)FactionWithProgress);
            writer.Put(Progress);
            writer.Put((byte)FactionShips.Length);
            writer.Put(FactionShips, 0, FactionShips.Length);
        }
    }
}
