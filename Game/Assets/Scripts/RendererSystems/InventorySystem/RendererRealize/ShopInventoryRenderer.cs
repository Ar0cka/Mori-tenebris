using System.Collections.Generic;
using Actors.Player.Inventory;
using DefaultNamespace;
using DefaultNamespace.ShopPanel;
using DefaultNamespace.Zenject;
using Player.Inventory;
using Project.Service.Context;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Project.Service
{
    #region RendererClasess

    public class ShopInventoryRenderer : AbstractInventoryRenderer<InventoryRendererInitContext, ShopContext>
    {
        private SlotContainer _leftPanel;
        private SlotContainer _rightPanel;

        public override void Init(InventoryRendererInitContext shopCtx, ISpawnProjectObject factory, IDestroyService destroyService,  IItemUIFactory itemUIFactory)
        {
            _leftPanel = new SlotContainer(shopCtx.LeftInventory, shopCtx.ShopInventoryConfig.InventoryData.SlotPrefab,
                shopCtx.ShopInventoryConfig.InventoryData.CountSlots, factory, destroyService, itemUIFactory);
            _rightPanel = new SlotContainer(shopCtx.RightInventory,
                shopCtx.ShopInventoryConfig.InventoryData.SlotPrefab,
                shopCtx.ShopInventoryConfig.InventoryData.CountSlots, factory, destroyService, itemUIFactory);
        }

        public override void Redraw(ShopContext ctx)
        {
            _leftPanel.Render(ctx.PrimaryInventory);
            _rightPanel.Render(ctx.SecondaryInventory);
        }
    }

    #endregion
}