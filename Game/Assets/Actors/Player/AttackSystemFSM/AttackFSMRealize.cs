using System;
using System.Collections.Generic;
using Actors.Player.AbstractFSM;
using UnityEngine;

namespace Actors.Player.AttackSystemFSM
{
    public class AttackFSMRealize : AbstractStateMachineRealize
    {
        [SerializeField] private List<string> baseAttackList;
        [SerializeField] private List<string> strengtheningSkills = new List<string>(2);
        [SerializeField] private List<string> attackSkills = new List<string>(3);

        private FStateMachine _fStateMachine;
        
        private void Awake()
        {
            _fStateMachine = new FStateMachine();
            
            
        }
    }
}