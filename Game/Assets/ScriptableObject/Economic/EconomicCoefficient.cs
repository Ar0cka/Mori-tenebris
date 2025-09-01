using System;
using Actors.NPC.NpcStateSystem;
using Items.Data.Scripts;
using UnityEngine;
using UnityEngine.Rendering;

namespace EconomicSystem
{
    [CreateAssetMenu(fileName = "EconomicCoefficientForPlayer", menuName = "EconomicSystem/CoefficientConfig", order = 0)]
    public class EconomicCoefficient : ScriptableObject
    {
        [SerializeField] private ReputationCoefficient reputationCoefficient;
        [SerializeField] private ItemRarityCoefficient rarityCoefficient;
        
        public ReputationCoefficient ReputationCoefficient => reputationCoefficient;
        public ItemRarityCoefficient ItemRarityCoefficient => rarityCoefficient;
    }

    [Serializable]
    public class ReputationCoefficient : SerializedDictionary<NpcReputationEnum, int>
    {
        
    }

    [Serializable]
    public class ItemRarityCoefficient : SerializedDictionary<ItemRarity, int>
    {
        
    }
}