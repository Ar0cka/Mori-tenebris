using UnityEngine;

namespace HitChecker
{
    public static class HitSystem
    {
        public static GameObject CircleHit(Vector2 position, float radius, string hitTag)
        {
            var hit = Physics2D.OverlapCircle(position, radius, LayerMask.GetMask("Player"));

            if (hit != null && hit.CompareTag(hitTag))
            {
                return hit.gameObject;
            }
            
            return null;
        }

        public static GameObject BoxHit(Vector2 position, Vector2 size, float angle, string hitTag)
        {
            var hit = Physics2D.OverlapBox(position, size, angle);

            if (hit != null && hit.CompareTag(hitTag))
            {
                return hit.gameObject;
            }
            
            return null;
        }
    }
}