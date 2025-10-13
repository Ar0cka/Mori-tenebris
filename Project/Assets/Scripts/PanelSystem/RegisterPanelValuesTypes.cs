using Player.Inventory;

namespace DefaultNamespace
{
    public class RegisterPanelValuesTypes
    {
        public void InitPanelsValues(PanelController panelController)
        {
            panelController.RegisterNewPanelType(typeof(ItemUI));
            panelController.RegisterNewPanelType(typeof(RecipesConfig));
        }
    }
}