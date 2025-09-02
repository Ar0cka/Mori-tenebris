using System.Collections.Generic;
using Actors.Player.AttackSystem.Scripts;
using UnityEngine;

namespace Actors.Player.AttackSystem.Data
{
    [CreateAssetMenu(fileName = "weaponConfig", menuName = "Weapon/config")]
    public class WeaponAttackSettings : ScriptableObject
    {
        [SerializeField] private AttackSettings attackSettingsSettings;
        [SerializeField] private List<AttackData> attackData;

        public AttackSettings AttackSettings => attackSettingsSettings;
        public List<AttackData> AttackData => attackData;

        public int MaxCountAttack => attackData.Count;
    }
}