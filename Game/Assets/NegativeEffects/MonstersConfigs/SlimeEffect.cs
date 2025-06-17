using UnityEngine;

namespace NegativeEffects
{
    [CreateAssetMenu(fileName = "SlimeEffect", menuName = "Monsters/Slime/Effect", order = 0)]
    public class SlimeEffect : EffectScrObj
    {
        [SerializeField] private EffectData effectData;
        [SerializeField] private PoisonEffect poisonEffect;
        
        public override EffectData EffectData() => effectData;
        public override AbstractActionEffect ActionEffect() => poisonEffect;
    }
}