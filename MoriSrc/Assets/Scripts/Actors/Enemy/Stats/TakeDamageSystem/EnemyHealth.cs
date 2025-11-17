using System;
using System.Collections.Generic;
using Unity.AppUI.Redux;

namespace Actors.Enemy.Stats.Scripts.TakeDamageSystem
{
    public class EnemyHealth : IObservable<HealthStates>, IEnemyHealth
    {
        private int _maxHealth;
        private int _currentHealth;
        private List<IObserver<HealthStates>> _observables = new();

        public int CurrentHealth => _currentHealth;

        public EnemyHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
        }
        
        public void TakeHit(int amount)
        {
            _currentHealth -= amount;

            if (_currentHealth <= 0) Die();
            else ReportObservers(HealthStates.Damage);
        }
        
        public void Heal(int amount)
        {
            _currentHealth += Math.Clamp(_currentHealth, 0, _maxHealth);
            
            ReportObservers(HealthStates.Heal);
        }
        
        private void Die()
        {
            ReportObservers(HealthStates.Died);
            //Логика смерти
        }
        
        public IDisposable Subscribe(IObserver<HealthStates> observer)
        {
            _observables.Add(observer);
            return new Unsubscriber(_observables, observer);
        } 
        private void ReportObservers(HealthStates currentState)
        {
            foreach (var observer in _observables)
            {
                observer.OnNext(currentState);
            }
        }
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<HealthStates>> _observers;
            private IObserver<HealthStates> _observer;

            public Unsubscriber(List<IObserver<HealthStates>> observers, IObserver<HealthStates> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }

    public enum HealthStates
    {
        Heal,
        Damage,
        Died
    }
}