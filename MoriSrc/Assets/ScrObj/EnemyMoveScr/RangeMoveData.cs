using System;
using System.Collections.Generic;
using Actors.Enemy.Movement.Enums;
using Scripts.Systems;
using UnityEngine;

namespace ScrObj.EnemyMoveScr
{
    [CreateAssetMenu(fileName = "RangeMoveData", menuName = "Enemy/RangeMove", order = 0)]
    public class RangeMoveData : MoveData
    {
        [field:SerializeField] public RadiusSettings RadiusSettings { get; private set; }
    }

    [Serializable]
    public class RadiusSettings
    {
        public float largeStopDistance;
        
        [SerializeField] private SerializableDictionary<AiRadiusEnum, float> radiusDictionary;
        public Dictionary<AiRadiusEnum, float> RadiusDictionary => radiusDictionary.ToDictionary();
    }
}