using System;
using DefaultNamespace.PlayerStatsOperation.StatSystem;

namespace DefaultNamespace.Events
{
    public interface IGetSubscribeInDieEvent
    {
        void SubscribeInDieEvent(Action action);
        void UnsubscribeFromDieEvent(Action callback);
    }
}