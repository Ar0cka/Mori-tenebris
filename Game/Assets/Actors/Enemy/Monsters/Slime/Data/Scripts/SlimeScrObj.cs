using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
using UnityEngine;

namespace Actors.Enemy.Monsters.Slime.Data.Scripts
{
    [CreateAssetMenu(fileName = "slime", menuName = "Monsters/slime", order = 0)]
    public class SlimeScrObj : MonsterScrObj
    {
        [SerializeField] private SlimeConfig slimeConfig;

        public override EnemyConfig GetConfig() => slimeConfig;
    }
}