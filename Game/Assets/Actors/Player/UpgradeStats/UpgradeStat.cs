using EventBusNamespace;
using DefaultNamespace.Enums;
using Zenject;

namespace PlayerNameSpace
{
    public class UpgradeStat : IUpgradeStat
    {
        [Inject] private IGetPlayerStat _playerStat;

        public void Upgrade(StatType statType)
        {
            _playerStat.GetPlayerDataStats().UpgradeStat(statType);
            EventBus.Publish(new SendUpdateStatEvent());
        }
    }
}