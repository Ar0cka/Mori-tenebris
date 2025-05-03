using EventBusNamespace;
using Zenject;

namespace PlayerNameSpace
{
    public class PlayerLevelSystem : IAddExpInLevel
    {
        [Inject] private IGetPlayerStat _getPlayerStat;
        private PlayerStatsUI _playerStatsUI;

        public void Initialize(PlayerStatsUI playerStatsUI)
        {
            _playerStatsUI = playerStatsUI;
        }

        public void AddExpInLevel(int exp)
        {
            _getPlayerStat.GetPlayerDataStats().UpLevel(exp);
            EventBus.Publish(new SendUIUpdateExpSystem());
        }
    }
}