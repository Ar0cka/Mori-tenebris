using EventBusNamespace;
using Zenject;

namespace PlayerNameSpace
{
    public class PlayerLevelSystem : IAddExpInLevel
    {
        [Inject] private IGetPlayerStat _getPlayerStat;
        private PlayerUIManager _playerUIManager;

        public void Initialize(PlayerUIManager playerUIManager)
        {
            _playerUIManager = playerUIManager;
        }

        public void AddExpInLevel(int exp)
        {
            _getPlayerStat.GetPlayerDataStats().UpLevel(exp);
            EventBus.Publish(new SendUIUpdateExpSystem());
        }
    }

    public class SendUIUpdateExpSystem
    {
    }
}