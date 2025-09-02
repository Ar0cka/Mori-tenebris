using System.Collections.Generic;
using Actors.NPC.NpcStateSystem;
using Items.Data.Scripts;
using Scripts.Systems;
using UnityEngine;

namespace ScrObj.Economic
{
    [CreateAssetMenu(fileName = "EconomicCoefficientForPlayer", menuName = "Economic/CoefficientConfig", order = 0)]
    public class EconomicCoefficient : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<NpcReputationEnum, float> reputationCoefficient;
        [SerializeField] private SerializableDictionary<ItemRarity, float> rarityCoefficient;
        
        public Dictionary<NpcReputationEnum, float> ReputationCoefficient => reputationCoefficient.ToDictionary();
        public Dictionary<ItemRarity, float>  ItemRarityCoefficient => rarityCoefficient.ToDictionary();
    }
}