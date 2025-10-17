using System;
using System.Collections.Generic;
using Scripts.Systems;
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
        public SerializableDictionary<MoveType, string> movementAnimationList;
        
        public float speed;
        public float speedMin;
        public float acceleration;
        public float deceleration;

        public bool hasPatrol = false;
    }

    [Serializable]
    public class AggressiveSettings
    {
        public float detectionRadius;
        public float lingerTime = 2f;
        public float lingerDistance = 0.3f;
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
        public float switchNodeDistance;
        
        public void SetPatrolPoints(List<Vector2> points)
        {
            patrolPoints = points.ToArray();
        }
    }
}

public enum MoveType
{
    Idle,
    Move,
    Backstep,
}