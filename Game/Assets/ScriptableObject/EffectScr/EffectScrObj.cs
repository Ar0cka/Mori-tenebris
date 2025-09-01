using UnityEngine;

namespace NegativeEffects
{
    public abstract class EffectScrObj : ScriptableObject
    {
        public abstract EffectData EffectData();
        public abstract AbstractActionEffect ActionEffect();
    }
}