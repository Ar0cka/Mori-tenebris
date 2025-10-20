using System;
using UnityEngine;

namespace ScrObj.EnemyMoveScr
{
    [CreateAssetMenu(fileName = "RangeMoveData", menuName = "Enemy/RangeMove", order = 0)]
    public class RangeMoveData : MoveData
    {
        [field:SerializeField] public RangeRadiusSettings RangeRadiusSettings { get; private set; }
    }

    [Serializable]
    public class RangeRadiusSettings
    {
        public float smallRadius; //Радиус для перехода в мили
        public float mediumRadius; //Радиус когда моб пытается держать дистанцию
        public float largeRadius; //Дистанция на которой моб подстраивается под край медиум радиуса
    }
}