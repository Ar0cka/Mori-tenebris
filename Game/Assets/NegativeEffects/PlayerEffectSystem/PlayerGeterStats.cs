using System.Collections.Generic;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace NegativeEffects
{
    public class PlayerGeterStats : MonoBehaviour
    {
        [SerializeField] private PlayerTakeDamage playerTakeDamage;

        private Dictionary<EffectType, IEffectStat> _stats = new();

        public void Init()
        {
            _stats.Add(EffectType.Poison, new PoisonStat(playerTakeDamage));
        }

        public TStat GetStat<TStat>(EffectType effectType) where TStat : class, IEffectStat
        {
            if (_stats.TryGetValue(effectType, out var stat))
                return stat as TStat;
            
            Debug.LogError("Not find stat");
            return null;
        }
    }
    
    #region StatConverter

    public interface IEffectStat
    {
        
    }
    
    public interface IPoisonStats : IEffectStat
    {
        public ITakeDamage Damage { get; }
    }
    
    public class PoisonStat: IPoisonStats
    {
        public ITakeDamage Damage { get; }

        public PoisonStat(ITakeDamage damage)
        {
            Damage = damage;
        }
    }

    #endregion
}