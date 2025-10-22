using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Service
{
    public static class VectorMathService
    {
        public static Vector2 GetBackstepVector(Vector2 targetPosition, Vector2 currentPosition, BackStepSettings backStepSettings = null)
        {
            Vector2 direction = currentPosition - targetPosition;

            if (backStepSettings != null && backStepSettings.backStepOffset != Vector2.zero)
            {
                Vector2 backstepOffset = backStepSettings.backStepOffset;
                direction += new Vector2(backstepOffset.x, backstepOffset.y);
            }

            return direction.normalized;
        }

        public static Vector2 GetForwardVector(Vector2 targetPosition, Vector2 currentPosition, Vector2 offset = default)
        {
            Vector2 direction = targetPosition - currentPosition;
            
            if (offset != default || offset != Vector2.zero)
            {
                direction += new Vector2(offset.x, offset.y);
            }
            
            return direction.normalized;
        }
    }
}