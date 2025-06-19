using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using NegativeEffects;
using PlayerNameSpace;
using UnityEngine;

public class HitAndAddEffect : MonoBehaviour
{
    [SerializeField] private PlayerTakeDamage playerTakeDamage;
    [SerializeField] private PlayerEffectController effectController;
    [SerializeField] private EffectScrObj effectScrObj;

    public void HitPlayer()
    {
        playerTakeDamage.TakeHit(10, DamageType.Physic);
    }

    public void EffectHitPlayer()
    {
        effectController.AddEffect(effectScrObj);
    }
}
