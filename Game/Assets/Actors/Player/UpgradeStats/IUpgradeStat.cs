using System;
using DefaultNamespace.Enums;

namespace PlayerNameSpace
{
    public interface IUpgradeStat
    {
        void Upgrade(StatType statType);
    }
}