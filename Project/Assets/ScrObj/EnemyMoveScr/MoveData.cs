using System;
using UnityEngine;

namespace ScrObj.EnemyMoveScr
{
    [CreateAssetMenu(fileName = "MoveData", menuName = "Enemy/Movement", order = 0)]
    public class MoveData : ScriptableObject
    {
        [field:SerializeField] public MoveSettings MoveSettings { get; private set; }
        [field:SerializeField] public PatrolSettings PatrolSettings { get; private set; }
        [field:SerializeField] public AggressiveSettings AggressiveSettings { get; private set; }
        [field:SerializeField] public IdleDetectionSettings IdleDetectionSettings { get; private set; }
    }

    [Serializable]
    public class MoveSettings
    {
        public float speed;
        public float speedMin;
        public float acceleration;
        public float deceleration;
    }

    [Serializable]
    public class AggressiveSettings
    {
        public float detectionRadius;
        public float lingerTime = 2f;
        public float stopDistance;
    }

    [Serializable]
    public class IdleDetectionSettings
    {
        public float fieldOfViewAngle;
        public float idleDetectionRadius;
        public float alertRadius;
    }

    [Serializable]
    public class PatrolSettings
    {
        public Vector2[] patrolPoints;
        public float patrolSpeed;
    }
}