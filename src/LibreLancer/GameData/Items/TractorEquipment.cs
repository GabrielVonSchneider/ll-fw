using LibreLancer.Sounds;
using LibreLancer.World;

namespace LibreLancer.GameData.Items
{
    public class TractorEquipment : Equipment
    {
        public Data.Equipment.Tractor Def;
        static TractorEquipment() => EquipmentObjectManager.RegisterType<TractorEquipment>(AddEquipment);
        static GameObject AddEquipment(GameObject parent, ResourceManager res, SoundManager snd, EquipmentType type, string hardpoint, Equipment equip)
        {
            //just a dummy to prevent an error here.
            return new GameObject();
        }
    }
}
