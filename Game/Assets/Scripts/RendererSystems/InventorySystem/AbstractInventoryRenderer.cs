using DefaultNamespace.Zenject;

namespace Project.Service
{
    public abstract class AbstractInventoryRenderer <TInitConfig, TRendererContext>
    {
        public abstract void Init(TInitConfig ctx, ISpawnProjectObject factory,
            IDestroyService destroyService);

        public abstract void Redraw(TRendererContext ctx);
    }
}