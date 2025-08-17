using Player.Inventory;

namespace DefaultNamespace
{
    public class PanelController
    {
        private IPanelOpen _currentPanel;

        public void UpdatePanel(IPanelOpen currentPanel)
        {
            _currentPanel = currentPanel;
        }

        public void OpenPanel(ItemSettings itemSettings)
        {
            if (_currentPanel != null)
            {
                _currentPanel.Open(itemSettings);
            }
        }
    }
}