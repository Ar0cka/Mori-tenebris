using System;

namespace Actors.Player.AttackSystem.Scripts
{
    [Serializable]
    public class AttackSettings
    {
        public float comboWindow;
        public float exitFromComboDelay;
        public float attackCooldown;
    }

    [Serializable]
    public class AttackData
    {
        public string triggerName;
        public int countQueue;
    }
}