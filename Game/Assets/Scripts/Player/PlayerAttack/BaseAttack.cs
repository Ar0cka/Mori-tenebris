using System;
using UnityEngine;

namespace Player.PlayerAttack
{
    public class BaseAttack : AbstractAttackClass
    {
        private void Update()
        {
            UpdateLogic();
        }
    }
}