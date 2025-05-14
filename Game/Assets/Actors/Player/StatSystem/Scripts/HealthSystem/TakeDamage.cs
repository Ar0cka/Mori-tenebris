using DefaultNamespace.Enums;
using UnityEngine;
using Zenject;

namespace PlayerNameSpace
{
    public class TakeDamage : MonoBehaviour
    {
        [Inject] private ITakeDamage _takeDamage;

        public void TakeHit(int damage, DamageType damageType)
        {
            _takeDamage.TakeDamage(damage, damageType);
        }
    }
}