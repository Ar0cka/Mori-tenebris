using Player.Inventory;
using UnityEngine;

namespace DefaultNamespace
{
    public class PanelController
    {
        private IPanelOpen _currentPanel;

        public void UpdatePanel(IPanelOpen currentPanel)
        {
            _currentPanel = currentPanel;
            Debug.Log("PanelController:UpdatePanel");
        }

        public void OpenPanel(ItemUI itemUI)
        {
            if (_currentPanel != null)
            {
                _currentPanel.Open(itemUI);
            }
        }
    }
}