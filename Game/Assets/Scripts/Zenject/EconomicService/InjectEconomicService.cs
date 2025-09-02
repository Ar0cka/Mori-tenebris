using Project.Service.EconomicService;

namespace Zenject.EconomicService
{
    public class InjectEconomicService : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MoneyService>().AsSingle();
            Container.Bind<PriceCalculatingService>().AsSingle();
            Container.Bind<TradeService>().AsSingle();
        }
    }
}