using UnityEngine;

namespace Project.Service
{
    public class DestroyService : IDestroyService
    {
        public void DestroyItem(Object obj)
        {
            Object.Destroy(obj);
        }
    }

    public interface IDestroyService
    {
        void DestroyItem(Object obj);
    }
}