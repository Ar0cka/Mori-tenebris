using System;
using UnityEngine;

namespace Actors.Enemy.Stats.Scripts.TakeDamageSystem
{
    public class HealthController : MonoBehaviour
    {
        private EnemyHealth _health;

        public void Initialize(int maxHealth)
        {
            _health = new EnemyHealth(maxHealth);
        }

        public IDisposable SubscribeNewObserver(IObserver<HealthStates> observer)
        {
            if (observer == null)
                throw new NullReferenceException();
            
            return _health.Subscribe(observer);
        }

        public IEnemyHealth GetHealth() =>  _health;
    }
}