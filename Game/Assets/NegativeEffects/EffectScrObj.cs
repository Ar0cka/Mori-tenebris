using UnityEngine;

namespace NegativeEffects
{
    public abstract class EffectScrObj : ScriptableObject, IEffectData
    {
        public abstract EffectData EffectData();
        public abstract AbstractActionEffect ActionEffect();
    }

    public interface IEffectData
    {
        EffectData EffectData();
        AbstractActionEffect ActionEffect();
    }
}