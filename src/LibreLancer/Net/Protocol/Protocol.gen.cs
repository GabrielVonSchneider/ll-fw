// AUTOGENERATED CODE
// Generated: 20210912 11:50:26 UTC
// Method count: 26

using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using LiteNetLib;
using LiteNetLib.Utils;

namespace LibreLancer.Net
{
    public interface IClientPlayer
    {
        void UpdateBaselinePrices(BaselinePrice[] prices);
        void CallThorn(string script);
        void SpawnObject(int id, string name, Vector3 position, Quaternion orientation, NetShipLoadout loadout);
        void SpawnPlayer(string system, Vector3 position, Quaternion orientation, long credits, NetShipLoadout ship);
        void UpdateInventory(long credits, NetShipLoadout ship);
        void SpawnSolar(SolarInfo[] solars);
        void OnConsoleMessage(string text);
        void SpawnDebris(int id, string archetype, string part, Vector3 position, Quaternion orientation, float mass);
        void BaseEnter(string _base, long credits, NetShipLoadout ship, string[] rtcs, NewsArticle[] news, SoldGood[] goods);
        void UpdateRTCs(string[] rtcs);
        void DespawnObject(int id);
        void PlaySound(string sound);
        void PlayMusic(string music);
        void DestroyPart(byte idtype, int id, string part);
        void RunMissionDialog(NetDlgLine[] lines);
    }

    public interface IServerPlayer
    {
        void Launch();
        void RTCComplete(string rtc);
        void LineSpoken(uint hash);
        void OnLocationEnter(string _base, string room);
        void ConsoleCommand(string command);
        void RequestCharacterDB();
        Task<bool> SelectCharacter(int index);
        Task<bool> DeleteCharacter(int index);
        Task<bool> CreateNewCharacter(string name, int index);
        Task<bool> PurchaseGood(string item, int count);
        Task<bool> SellGood(int id, int count);
    }

    public partial class RemoteServerPlayer : IServerPlayer
    {
        int retSeq;

        public void Launch()
        {
            SendPacket(new ServerPacket_Launch() {
            });
        }

        public void RTCComplete(string rtc)
        {
            SendPacket(new ServerPacket_RTCComplete() {
                rtc = rtc,
            });
        }

        public void LineSpoken(uint hash)
        {
            SendPacket(new ServerPacket_LineSpoken() {
                hash = hash,
            });
        }

        public void OnLocationEnter(string _base, string room)
        {
            SendPacket(new ServerPacket_OnLocationEnter() {
                _base = _base,
                room = room,
            });
        }

        public void ConsoleCommand(string command)
        {
            SendPacket(new ServerPacket_ConsoleCommand() {
                command = command,
            });
        }

        public void RequestCharacterDB()
        {
            SendPacket(new ServerPacket_RequestCharacterDB() {
            });
        }

        public Task<bool> SelectCharacter(int index)
        {
            var complete = ResponseHandler.GetCompletionSource_bool(retSeq);
            SendPacket(new ServerPacket_SelectCharacter() {
                Sequence = Interlocked.Increment(ref retSeq),
                index = index,
            });
            return complete.Task;
        }

        public Task<bool> DeleteCharacter(int index)
        {
            var complete = ResponseHandler.GetCompletionSource_bool(retSeq);
            SendPacket(new ServerPacket_DeleteCharacter() {
                Sequence = Interlocked.Increment(ref retSeq),
                index = index,
            });
            return complete.Task;
        }

        public Task<bool> CreateNewCharacter(string name, int index)
        {
            var complete = ResponseHandler.GetCompletionSource_bool(retSeq);
            SendPacket(new ServerPacket_CreateNewCharacter() {
                Sequence = Interlocked.Increment(ref retSeq),
                name = name,
                index = index,
            });
            return complete.Task;
        }

        public Task<bool> PurchaseGood(string item, int count)
        {
            var complete = ResponseHandler.GetCompletionSource_bool(retSeq);
            SendPacket(new ServerPacket_PurchaseGood() {
                Sequence = Interlocked.Increment(ref retSeq),
                item = item,
                count = count,
            });
            return complete.Task;
        }

        public Task<bool> SellGood(int id, int count)
        {
            var complete = ResponseHandler.GetCompletionSource_bool(retSeq);
            SendPacket(new ServerPacket_SellGood() {
                Sequence = Interlocked.Increment(ref retSeq),
                id = id,
                count = count,
            });
            return complete.Task;
        }

    }
    public partial class RemoteClientPlayer : IClientPlayer
    {
        int retSeq;

        public void UpdateBaselinePrices(BaselinePrice[] prices)
        {
            SendPacket(new ClientPacket_UpdateBaselinePrices() {
                prices = prices,
            });
        }

        public void CallThorn(string script)
        {
            SendPacket(new ClientPacket_CallThorn() {
                script = script,
            });
        }

        public void SpawnObject(int id, string name, Vector3 position, Quaternion orientation, NetShipLoadout loadout)
        {
            SendPacket(new ClientPacket_SpawnObject() {
                id = id,
                name = name,
                position = position,
                orientation = orientation,
                loadout = loadout,
            });
        }

        public void SpawnPlayer(string system, Vector3 position, Quaternion orientation, long credits, NetShipLoadout ship)
        {
            SendPacket(new ClientPacket_SpawnPlayer() {
                system = system,
                position = position,
                orientation = orientation,
                credits = credits,
                ship = ship,
            });
        }

        public void UpdateInventory(long credits, NetShipLoadout ship)
        {
            SendPacket(new ClientPacket_UpdateInventory() {
                credits = credits,
                ship = ship,
            });
        }

        public void SpawnSolar(SolarInfo[] solars)
        {
            SendPacket(new ClientPacket_SpawnSolar() {
                solars = solars,
            });
        }

        public void OnConsoleMessage(string text)
        {
            SendPacket(new ClientPacket_OnConsoleMessage() {
                text = text,
            });
        }

        public void SpawnDebris(int id, string archetype, string part, Vector3 position, Quaternion orientation, float mass)
        {
            SendPacket(new ClientPacket_SpawnDebris() {
                id = id,
                archetype = archetype,
                part = part,
                position = position,
                orientation = orientation,
                mass = mass,
            });
        }

        public void BaseEnter(string _base, long credits, NetShipLoadout ship, string[] rtcs, NewsArticle[] news, SoldGood[] goods)
        {
            SendPacket(new ClientPacket_BaseEnter() {
                _base = _base,
                credits = credits,
                ship = ship,
                rtcs = rtcs,
                news = news,
                goods = goods,
            });
        }

        public void UpdateRTCs(string[] rtcs)
        {
            SendPacket(new ClientPacket_UpdateRTCs() {
                rtcs = rtcs,
            });
        }

        public void DespawnObject(int id)
        {
            SendPacket(new ClientPacket_DespawnObject() {
                id = id,
            });
        }

        public void PlaySound(string sound)
        {
            SendPacket(new ClientPacket_PlaySound() {
                sound = sound,
            });
        }

        public void PlayMusic(string music)
        {
            SendPacket(new ClientPacket_PlayMusic() {
                music = music,
            });
        }

        public void DestroyPart(byte idtype, int id, string part)
        {
            SendPacket(new ClientPacket_DestroyPart() {
                idtype = idtype,
                id = id,
                part = part,
            });
        }

        public void RunMissionDialog(NetDlgLine[] lines)
        {
            SendPacket(new ClientPacket_RunMissionDialog() {
                lines = lines,
            });
        }

    }

    public class ServerPacket_Launch : IPacket
    {
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_Launch();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
        }
    }

    public class ServerPacket_RTCComplete : IPacket
    {
        public string rtc;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_RTCComplete();
            _packet.rtc = message.GetStringPacked();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(rtc);
        }
    }

    public class ServerPacket_LineSpoken : IPacket
    {
        public uint hash;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_LineSpoken();
            _packet.hash = message.GetUInt();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(hash);
        }
    }

    public class ServerPacket_OnLocationEnter : IPacket
    {
        public string _base;
        public string room;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_OnLocationEnter();
            _packet._base = message.GetStringPacked();
            _packet.room = message.GetStringPacked();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(_base);
            message.PutStringPacked(room);
        }
    }

    public class ServerPacket_ConsoleCommand : IPacket
    {
        public string command;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_ConsoleCommand();
            _packet.command = message.GetStringPacked();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(command);
        }
    }

    public class ServerPacket_RequestCharacterDB : IPacket
    {
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_RequestCharacterDB();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
        }
    }

    public class ServerPacket_SelectCharacter : IPacket
    {
        public int Sequence;
        public int index;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_SelectCharacter();
            _packet.Sequence = message.GetInt();
            _packet.index = message.GetInt();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(Sequence);
            message.Put(index);
        }
    }

    public class ServerPacket_DeleteCharacter : IPacket
    {
        public int Sequence;
        public int index;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_DeleteCharacter();
            _packet.Sequence = message.GetInt();
            _packet.index = message.GetInt();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(Sequence);
            message.Put(index);
        }
    }

    public class ServerPacket_CreateNewCharacter : IPacket
    {
        public int Sequence;
        public string name;
        public int index;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_CreateNewCharacter();
            _packet.Sequence = message.GetInt();
            _packet.name = message.GetStringPacked();
            _packet.index = message.GetInt();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(Sequence);
            message.PutStringPacked(name);
            message.Put(index);
        }
    }

    public class ServerPacket_PurchaseGood : IPacket
    {
        public int Sequence;
        public string item;
        public int count;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_PurchaseGood();
            _packet.Sequence = message.GetInt();
            _packet.item = message.GetStringPacked();
            _packet.count = message.GetInt();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(Sequence);
            message.PutStringPacked(item);
            message.Put(count);
        }
    }

    public class ServerPacket_SellGood : IPacket
    {
        public int Sequence;
        public int id;
        public int count;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ServerPacket_SellGood();
            _packet.Sequence = message.GetInt();
            _packet.id = message.GetInt();
            _packet.count = message.GetInt();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(Sequence);
            message.Put(id);
            message.Put(count);
        }
    }

    public class ClientPacket_UpdateBaselinePrices : IPacket
    {
        public BaselinePrice[] prices;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_UpdateBaselinePrices();
            uint __len_prices = message.GetVariableUInt32();
            if (__len_prices > 0) {
                _packet.prices = new BaselinePrice[(int)(__len_prices - 1)];
                for(int _ARRIDX = 0; _ARRIDX < _packet.prices.Length; _ARRIDX++)
                    _packet.prices[_ARRIDX] = BaselinePrice.Read(message);
            }
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            if (prices != null) {
                message.PutVariableUInt32((uint)(prices.Length + 1));
                foreach(var _element in prices)
                    _element.Put(message);
            } else {
                message.PutVariableUInt32(0);
            }
        }
    }

    public class ClientPacket_CallThorn : IPacket
    {
        public string script;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_CallThorn();
            _packet.script = message.GetStringPacked();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(script);
        }
    }

    public class ClientPacket_SpawnObject : IPacket
    {
        public int id;
        public string name;
        public Vector3 position;
        public Quaternion orientation;
        public NetShipLoadout loadout;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_SpawnObject();
            _packet.id = message.GetInt();
            _packet.name = message.GetStringPacked();
            _packet.position = message.GetVector3();
            _packet.orientation = message.GetQuaternion();
            _packet.loadout = NetShipLoadout.Read(message);
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(id);
            message.PutStringPacked(name);
            message.Put(position);
            message.Put(orientation);
            loadout.Put(message);
        }
    }

    public class ClientPacket_SpawnPlayer : IPacket
    {
        public string system;
        public Vector3 position;
        public Quaternion orientation;
        public long credits;
        public NetShipLoadout ship;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_SpawnPlayer();
            _packet.system = message.GetStringPacked();
            _packet.position = message.GetVector3();
            _packet.orientation = message.GetQuaternion();
            _packet.credits = message.GetLong();
            _packet.ship = NetShipLoadout.Read(message);
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(system);
            message.Put(position);
            message.Put(orientation);
            message.Put(credits);
            ship.Put(message);
        }
    }

    public class ClientPacket_UpdateInventory : IPacket
    {
        public long credits;
        public NetShipLoadout ship;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_UpdateInventory();
            _packet.credits = message.GetLong();
            _packet.ship = NetShipLoadout.Read(message);
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(credits);
            ship.Put(message);
        }
    }

    public class ClientPacket_SpawnSolar : IPacket
    {
        public SolarInfo[] solars;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_SpawnSolar();
            uint __len_solars = message.GetVariableUInt32();
            if (__len_solars > 0) {
                _packet.solars = new SolarInfo[(int)(__len_solars - 1)];
                for(int _ARRIDX = 0; _ARRIDX < _packet.solars.Length; _ARRIDX++)
                    _packet.solars[_ARRIDX] = SolarInfo.Read(message);
            }
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            if (solars != null) {
                message.PutVariableUInt32((uint)(solars.Length + 1));
                foreach(var _element in solars)
                    _element.Put(message);
            } else {
                message.PutVariableUInt32(0);
            }
        }
    }

    public class ClientPacket_OnConsoleMessage : IPacket
    {
        public string text;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_OnConsoleMessage();
            _packet.text = message.GetStringPacked();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(text);
        }
    }

    public class ClientPacket_SpawnDebris : IPacket
    {
        public int id;
        public string archetype;
        public string part;
        public Vector3 position;
        public Quaternion orientation;
        public float mass;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_SpawnDebris();
            _packet.id = message.GetInt();
            _packet.archetype = message.GetStringPacked();
            _packet.part = message.GetStringPacked();
            _packet.position = message.GetVector3();
            _packet.orientation = message.GetQuaternion();
            _packet.mass = message.GetFloat();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(id);
            message.PutStringPacked(archetype);
            message.PutStringPacked(part);
            message.Put(position);
            message.Put(orientation);
            message.Put(mass);
        }
    }

    public class ClientPacket_BaseEnter : IPacket
    {
        public string _base;
        public long credits;
        public NetShipLoadout ship;
        public string[] rtcs;
        public NewsArticle[] news;
        public SoldGood[] goods;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_BaseEnter();
            _packet._base = message.GetStringPacked();
            _packet.credits = message.GetLong();
            _packet.ship = NetShipLoadout.Read(message);
            uint __len_rtcs = message.GetVariableUInt32();
            if (__len_rtcs > 0) {
                _packet.rtcs = new string[(int)(__len_rtcs - 1)];
                for(int _ARRIDX = 0; _ARRIDX < _packet.rtcs.Length; _ARRIDX++)
                    _packet.rtcs[_ARRIDX] = message.GetStringPacked();
            }
            uint __len_news = message.GetVariableUInt32();
            if (__len_news > 0) {
                _packet.news = new NewsArticle[(int)(__len_news - 1)];
                for(int _ARRIDX = 0; _ARRIDX < _packet.news.Length; _ARRIDX++)
                    _packet.news[_ARRIDX] = NewsArticle.Read(message);
            }
            uint __len_goods = message.GetVariableUInt32();
            if (__len_goods > 0) {
                _packet.goods = new SoldGood[(int)(__len_goods - 1)];
                for(int _ARRIDX = 0; _ARRIDX < _packet.goods.Length; _ARRIDX++)
                    _packet.goods[_ARRIDX] = SoldGood.Read(message);
            }
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(_base);
            message.Put(credits);
            ship.Put(message);
            if (rtcs != null) {
                message.PutVariableUInt32((uint)(rtcs.Length + 1));
                foreach(var _element in rtcs)
                    message.PutStringPacked(_element);
            } else {
                message.PutVariableUInt32(0);
            }
            if (news != null) {
                message.PutVariableUInt32((uint)(news.Length + 1));
                foreach(var _element in news)
                    _element.Put(message);
            } else {
                message.PutVariableUInt32(0);
            }
            if (goods != null) {
                message.PutVariableUInt32((uint)(goods.Length + 1));
                foreach(var _element in goods)
                    _element.Put(message);
            } else {
                message.PutVariableUInt32(0);
            }
        }
    }

    public class ClientPacket_UpdateRTCs : IPacket
    {
        public string[] rtcs;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_UpdateRTCs();
            uint __len_rtcs = message.GetVariableUInt32();
            if (__len_rtcs > 0) {
                _packet.rtcs = new string[(int)(__len_rtcs - 1)];
                for(int _ARRIDX = 0; _ARRIDX < _packet.rtcs.Length; _ARRIDX++)
                    _packet.rtcs[_ARRIDX] = message.GetStringPacked();
            }
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            if (rtcs != null) {
                message.PutVariableUInt32((uint)(rtcs.Length + 1));
                foreach(var _element in rtcs)
                    message.PutStringPacked(_element);
            } else {
                message.PutVariableUInt32(0);
            }
        }
    }

    public class ClientPacket_DespawnObject : IPacket
    {
        public int id;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_DespawnObject();
            _packet.id = message.GetInt();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(id);
        }
    }

    public class ClientPacket_PlaySound : IPacket
    {
        public string sound;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_PlaySound();
            _packet.sound = message.GetStringPacked();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(sound);
        }
    }

    public class ClientPacket_PlayMusic : IPacket
    {
        public string music;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_PlayMusic();
            _packet.music = message.GetStringPacked();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.PutStringPacked(music);
        }
    }

    public class ClientPacket_DestroyPart : IPacket
    {
        public byte idtype;
        public int id;
        public string part;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_DestroyPart();
            _packet.idtype = message.GetByte();
            _packet.id = message.GetInt();
            _packet.part = message.GetStringPacked();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(idtype);
            message.Put(id);
            message.PutStringPacked(part);
        }
    }

    public class ClientPacket_RunMissionDialog : IPacket
    {
        public NetDlgLine[] lines;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ClientPacket_RunMissionDialog();
            uint __len_lines = message.GetVariableUInt32();
            if (__len_lines > 0) {
                _packet.lines = new NetDlgLine[(int)(__len_lines - 1)];
                for(int _ARRIDX = 0; _ARRIDX < _packet.lines.Length; _ARRIDX++)
                    _packet.lines[_ARRIDX] = NetDlgLine.Read(message);
            }
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            if (lines != null) {
                message.PutVariableUInt32((uint)(lines.Length + 1));
                foreach(var _element in lines)
                    _element.Put(message);
            } else {
                message.PutVariableUInt32(0);
            }
        }
    }

    public class ResponsePacket_bool : IPacket
    {
        public int Sequence;
        public bool Value;
        public static object Read(NetPacketReader message)
        {
            var _packet = new ResponsePacket_bool();
            _packet.Sequence = message.GetInt();
            _packet.Value = message.GetBool();
            return _packet;
        }
        public void WriteContents(NetDataWriter message)
        {
            message.Put(Sequence);
            message.Put(Value);
        }
    }
    public partial class NetResponseHandler
    {
        public TaskCompletionSource<bool> GetCompletionSource_bool(int sequence)
        {
            var src = new TaskCompletionSource<bool>();
            completionSources.Add(sequence, src);
            return src;
        }

        public bool HandlePacket(IPacket pkt)
        {
            switch (pkt)
            {
                case ResponsePacket_bool _1: {
                    if (completionSources.TryGetValue(_1.Sequence, out object k)) {
                        completionSources.Remove(_1.Sequence);
                        if (k is TaskCompletionSource<bool> i) i.SetResult(_1.Value);
                    }
                    return true;
                }
            }
            return false;
        }
    }
    static class GeneratedProtocol
    {
        public static void RegisterPackets()
        {
            Packets.Register<ResponsePacket_bool>(ResponsePacket_bool.Read);
            Packets.Register<ServerPacket_Launch>(ServerPacket_Launch.Read);
            Packets.Register<ServerPacket_RTCComplete>(ServerPacket_RTCComplete.Read);
            Packets.Register<ServerPacket_LineSpoken>(ServerPacket_LineSpoken.Read);
            Packets.Register<ServerPacket_OnLocationEnter>(ServerPacket_OnLocationEnter.Read);
            Packets.Register<ServerPacket_ConsoleCommand>(ServerPacket_ConsoleCommand.Read);
            Packets.Register<ServerPacket_RequestCharacterDB>(ServerPacket_RequestCharacterDB.Read);
            Packets.Register<ServerPacket_SelectCharacter>(ServerPacket_SelectCharacter.Read);
            Packets.Register<ServerPacket_DeleteCharacter>(ServerPacket_DeleteCharacter.Read);
            Packets.Register<ServerPacket_CreateNewCharacter>(ServerPacket_CreateNewCharacter.Read);
            Packets.Register<ServerPacket_PurchaseGood>(ServerPacket_PurchaseGood.Read);
            Packets.Register<ServerPacket_SellGood>(ServerPacket_SellGood.Read);
            Packets.Register<ClientPacket_UpdateBaselinePrices>(ClientPacket_UpdateBaselinePrices.Read);
            Packets.Register<ClientPacket_CallThorn>(ClientPacket_CallThorn.Read);
            Packets.Register<ClientPacket_SpawnObject>(ClientPacket_SpawnObject.Read);
            Packets.Register<ClientPacket_SpawnPlayer>(ClientPacket_SpawnPlayer.Read);
            Packets.Register<ClientPacket_UpdateInventory>(ClientPacket_UpdateInventory.Read);
            Packets.Register<ClientPacket_SpawnSolar>(ClientPacket_SpawnSolar.Read);
            Packets.Register<ClientPacket_OnConsoleMessage>(ClientPacket_OnConsoleMessage.Read);
            Packets.Register<ClientPacket_SpawnDebris>(ClientPacket_SpawnDebris.Read);
            Packets.Register<ClientPacket_BaseEnter>(ClientPacket_BaseEnter.Read);
            Packets.Register<ClientPacket_UpdateRTCs>(ClientPacket_UpdateRTCs.Read);
            Packets.Register<ClientPacket_DespawnObject>(ClientPacket_DespawnObject.Read);
            Packets.Register<ClientPacket_PlaySound>(ClientPacket_PlaySound.Read);
            Packets.Register<ClientPacket_PlayMusic>(ClientPacket_PlayMusic.Read);
            Packets.Register<ClientPacket_DestroyPart>(ClientPacket_DestroyPart.Read);
            Packets.Register<ClientPacket_RunMissionDialog>(ClientPacket_RunMissionDialog.Read);
        }

        public static async Task<bool> HandleServerPacket(IPacket pkt, IServerPlayer player, INetResponder res)
        {
            switch (pkt)
            {
                case ServerPacket_Launch _1: {
                    player.Launch();
                    return true;
                }
                case ServerPacket_RTCComplete _2: {
                    player.RTCComplete(_2.rtc);
                    return true;
                }
                case ServerPacket_LineSpoken _3: {
                    player.LineSpoken(_3.hash);
                    return true;
                }
                case ServerPacket_OnLocationEnter _4: {
                    player.OnLocationEnter(_4._base,_4.room);
                    return true;
                }
                case ServerPacket_ConsoleCommand _5: {
                    player.ConsoleCommand(_5.command);
                    return true;
                }
                case ServerPacket_RequestCharacterDB _6: {
                    player.RequestCharacterDB();
                    return true;
                }
                case ServerPacket_SelectCharacter _7: {
                    var retval = await player.SelectCharacter(_7.index);
                    res.SendResponse(new ResponsePacket_bool() { Sequence = _7.Sequence, Value = retval });
                    return true;
                }
                case ServerPacket_DeleteCharacter _8: {
                    var retval = await player.DeleteCharacter(_8.index);
                    res.SendResponse(new ResponsePacket_bool() { Sequence = _8.Sequence, Value = retval });
                    return true;
                }
                case ServerPacket_CreateNewCharacter _9: {
                    var retval = await player.CreateNewCharacter(_9.name,_9.index);
                    res.SendResponse(new ResponsePacket_bool() { Sequence = _9.Sequence, Value = retval });
                    return true;
                }
                case ServerPacket_PurchaseGood _10: {
                    var retval = await player.PurchaseGood(_10.item,_10.count);
                    res.SendResponse(new ResponsePacket_bool() { Sequence = _10.Sequence, Value = retval });
                    return true;
                }
                case ServerPacket_SellGood _11: {
                    var retval = await player.SellGood(_11.id,_11.count);
                    res.SendResponse(new ResponsePacket_bool() { Sequence = _11.Sequence, Value = retval });
                    return true;
                }
            }
            return false;
        }

        public static async Task<bool> HandleClientPacket(IPacket pkt, IClientPlayer player, INetResponder res)
        {
            switch (pkt)
            {
                case ClientPacket_UpdateBaselinePrices _1: {
                    player.UpdateBaselinePrices(_1.prices);
                    return true;
                }
                case ClientPacket_CallThorn _2: {
                    player.CallThorn(_2.script);
                    return true;
                }
                case ClientPacket_SpawnObject _3: {
                    player.SpawnObject(_3.id,_3.name,_3.position,_3.orientation,_3.loadout);
                    return true;
                }
                case ClientPacket_SpawnPlayer _4: {
                    player.SpawnPlayer(_4.system,_4.position,_4.orientation,_4.credits,_4.ship);
                    return true;
                }
                case ClientPacket_UpdateInventory _5: {
                    player.UpdateInventory(_5.credits,_5.ship);
                    return true;
                }
                case ClientPacket_SpawnSolar _6: {
                    player.SpawnSolar(_6.solars);
                    return true;
                }
                case ClientPacket_OnConsoleMessage _7: {
                    player.OnConsoleMessage(_7.text);
                    return true;
                }
                case ClientPacket_SpawnDebris _8: {
                    player.SpawnDebris(_8.id,_8.archetype,_8.part,_8.position,_8.orientation,_8.mass);
                    return true;
                }
                case ClientPacket_BaseEnter _9: {
                    player.BaseEnter(_9._base,_9.credits,_9.ship,_9.rtcs,_9.news,_9.goods);
                    return true;
                }
                case ClientPacket_UpdateRTCs _10: {
                    player.UpdateRTCs(_10.rtcs);
                    return true;
                }
                case ClientPacket_DespawnObject _11: {
                    player.DespawnObject(_11.id);
                    return true;
                }
                case ClientPacket_PlaySound _12: {
                    player.PlaySound(_12.sound);
                    return true;
                }
                case ClientPacket_PlayMusic _13: {
                    player.PlayMusic(_13.music);
                    return true;
                }
                case ClientPacket_DestroyPart _14: {
                    player.DestroyPart(_14.idtype,_14.id,_14.part);
                    return true;
                }
                case ClientPacket_RunMissionDialog _15: {
                    player.RunMissionDialog(_15.lines);
                    return true;
                }
            }
            return false;
        }

    }
}
