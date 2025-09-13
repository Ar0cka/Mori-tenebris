using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Project.Service.Context;

namespace Project.Service.RendererRealize
{
    public class CraftInventoryRenderer : AbstractInventoryRenderer<CraftRendInitContext, CraftContext>
    {
        private SlotContainer _playerInventoryContainer;

        public override void Init(CraftRendInitContext ctx, ISpawnProjectObject factory,
            IDestroyService destroyService)
        {
            _playerInventoryContainer = new SlotContainer(ctx.LeftInventory, ctx.InventoryConfig.InventoryData.SlotPrefab,
                ctx.InventoryConfig.InventoryData.CountSlots,
                factory, destroyService);
        }

        public override void Redraw(CraftContext ctx)
        {
            _playerInventoryContainer.Render(ctx.PlayerInventory);
        }

        public List<ItemUI> RedrawItems(CraftContext ctx)
        {
            var list = _playerInventoryContainer.Render(ctx.PlayerInventory);
            return list;
        }
    }
}