using UnityEngine;

namespace Actors.Enemy.Data.Scripts
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "MonsterData/AttackConfig", order = 0)]
    public class AttackScrConf : ScriptableObject
    {
        [SerializeField] private AttackConfig attackConfig;

        public AttackConfig GetAttackConfig() => attackConfig;
    }
}