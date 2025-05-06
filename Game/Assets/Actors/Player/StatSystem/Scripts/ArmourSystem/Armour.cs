using System;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using Data;
using EventBusNamespace;
using PlayerNameSpace;
using Zenject;

namespace DefaultNamespace.PlayerStatsOperation.StatSystem.ArmourSystem
{
    public class Armour : IEquipAndUnEquipItem, IDisposable
    {
        public float PhysicArmour { get; private set; }
        public float MagicArmour { get; private set; }
        
        [Inject] private readonly IGetPlayerStat _getPlayerStat;

        private Dictionary<string, EquipItem> _equipItems = new Dictionary<string, EquipItem>();

        private int _armourFromAgility;

        public void Initialize()
        {
            EventBus.Subscribe<SendUpdateStatEvent>(e => UpdateArmour());
            AddArmourFromAgility();
        }

        private void UpdateArmour()
        {
            PhysicArmour -= _armourFromAgility;
            AddArmourFromAgility();
        }

        private void AddArmourFromAgility()
        {
            _armourFromAgility = (int)(_getPlayerStat.GetPlayerDataStats().Agility * 0.5f);
            PhysicArmour += _armourFromAgility;
        }
        
        public void EquipArmourItem(ItemData itemData)
        {
            if (itemData is EquipItem equipItem)
            {
                _equipItems.Add(itemData.nameItem, equipItem);

                PhysicArmour += equipItem.physicArmour;
                MagicArmour += equipItem.magicArmour;
            }
            
        }

        public void DeleteArmour(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (_equipItems.TryGetValue(name, out EquipItem item))
                {
                    PhysicArmour -= item.physicArmour;
                    MagicArmour -= item.magicArmour;
                }
                _equipItems.Remove(name);
            }
        }

        public float GetArmour(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physic:
                    return PhysicArmour;
                case DamageType.Magic:
                    return MagicArmour;
            }

            return 0;
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<SendUpdateStatEvent>(e => UpdateArmour());
        }
    }
}