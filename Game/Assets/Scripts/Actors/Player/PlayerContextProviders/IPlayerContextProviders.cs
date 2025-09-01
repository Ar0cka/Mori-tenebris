using UnityEngine;

namespace PlayerContextProviders
{
    public interface IPlayerContextProviders<TContext>
    {
        TContext GetPlayerContext();
    }
}