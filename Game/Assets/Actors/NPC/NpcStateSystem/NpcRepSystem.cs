using System;
using System.Collections.Generic;
using Actors.NPC.DialogSystem.DataScripts;
using Actors.NPC.NpcStateSystem;
using Systems;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Actors.NPC.NpcStateSystem
{
    public class NpcRepSystem : IReputationOperations
    {
        private NpcReputationEnum _npcReputationEnum;
        private ReputationData _reputationData;
        private ReactiveValue<int> _valueReputation; //Реативный класс, который добавляем в себя event для 

        public void InitializeStats(ReputationData startReputationData)
        {
            _reputationData = startReputationData;
            
            _valueReputation = new ReactiveValue<int>(_reputationData.reputation);
            _valueReputation.OnChange += UpdateReputationState;
            
            UpdateReputationState(_valueReputation.Value);
        }

        private void UpdateReputationState(int value)
        {
            if (value >= _reputationData.friendlyValues)
            {
                _npcReputationEnum = NpcReputationEnum.Friendly;
            }
            else if (value <= _reputationData.agressivValues)
            {
                _npcReputationEnum = NpcReputationEnum.Aggressive;
            }
            else
            {
                _npcReputationEnum = NpcReputationEnum.Neutral;
            }
            
            Debug.Log($"Current reputation state: {_npcReputationEnum}");
        }

        public NpcReputationEnum GetCurrentNpcReputationState()
        {
            return _npcReputationEnum;
        }
        
        public void MinesReputation(int value)
        {
            _valueReputation.Value -= value;
        }

        public void PlusReputation(int value)
        {
            _valueReputation.Value += value;
        }
    }
}

public interface IReputationOperations
{
    public void MinesReputation(int value);
    public void PlusReputation(int value);
    public NpcReputationEnum GetCurrentNpcReputationState();
}