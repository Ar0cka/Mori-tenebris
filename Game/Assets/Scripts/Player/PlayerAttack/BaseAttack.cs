using System;
using EventBusNamespace;
using UnityEngine;

namespace Player.PlayerAttack
{
    public class BaseAttack : AbstractAttackClass
    {
        private void Update()
        {
            UpdateLogic();
        }

        public override void SendEffectsEvent()
        {
            EventBus.Publish(new SendBaseAttackEvent(_currentCountAttack));
        }
    }
}