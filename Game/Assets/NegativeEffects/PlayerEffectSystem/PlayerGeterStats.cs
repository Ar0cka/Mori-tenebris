using System.Collections.Generic;
using PlayerNameSpace;
using UnityEngine;
using Zenject;

namespace NegativeEffects
{
    public class PlayerGeterStats : MonoBehaviour
    {
        [Inject] private Health _health;
        [Inject] private IHitPlayer _hitPlayer;

        private Dictionary<EffectType, IEffectStat> _stats = new();

        public void Init()
        {
            _stats.Add(EffectType.Poison, new PoisonStat(_health, _hitPlayer));
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
        public Health Health { get; }
        public IHitPlayer Damage { get; }
    }
    
    public class PoisonStat: IPoisonStats
    {
        public Health Health { get; }
        public IHitPlayer Damage { get; }

        public PoisonStat(Health health, IHitPlayer damage)
        {
            Health = health;
            Damage = damage;
        }
    }

    #endregion
}