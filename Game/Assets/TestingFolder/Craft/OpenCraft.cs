using DefaultNamespace;
using UnityEngine;

namespace TestingFolder.Craft
{
    public class OpenCraft : MonoBehaviour
    {
        [SerializeField] private CraftPanel craftPanel;

        public void Open()
        {
            craftPanel.OpenPanel();
        }
    }
}