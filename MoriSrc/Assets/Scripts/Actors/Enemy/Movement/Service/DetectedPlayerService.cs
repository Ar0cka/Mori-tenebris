using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Actors.Enemy.Movement.Base.Service
{
    public class DetectedPlayerService
    {
        private const float ObstacleRadius = 1f;
        private const string ObstacleTag = "Obstacle";
        
        public Vector2 DetectedTarget(float radius, LayerMask layerMask, Vector2 currentPosition)
        {
            var hit = Physics2D.OverlapCircle(currentPosition, radius, layerMask);

            if (hit != null)
            {
                return hit.transform.position;
            }
            
            return Vector2.zero;
        }
        public bool IdleDetection(float radius, float fieldOfViewAngle, LayerMask layerMask, SpriteRenderer spriteRenderer, Vector2 currentPosition)
        {
            var hits =
                Physics2D.OverlapCircle(currentPosition, radius, layerMask);

            if (hits is null)
            {
                Debug.Log("Not find player");
                return false;
            }
            
            Vector2 lookDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
            Vector2 targetDirection = ((Vector2)hits.transform.position - currentPosition).normalized;
            
            float angle = Vector2.Angle(lookDirection, targetDirection);

            if (angle > fieldOfViewAngle / 2)
            {
                Debug.Log("Not find player");
                return false;
            }
            
            Debug.Log(angle + " player detection");
            return true;
        }

        public bool CheckTargetPositionOnObstacle(Vector2 targetPosition)
        {
            var hit = Physics2D.OverlapCircle(targetPosition, ObstacleRadius, LayerMask.GetMask(ObstacleTag));

            if (hit != null)
            {
                return false;
            }

            return true;
        }
        
        public Vector2 DetectedGroupObject(float radius, List<LayerMask> layerMasks, Vector2 currentPosition)
        {
            float minDistance = float.MaxValue;
            Vector2 nearestVector = Vector2.zero;
            
            foreach (var layerMask in layerMasks)
            {
                var hit = Physics2D.OverlapCircle(currentPosition, radius, layerMask);

                float distance = ((Vector2)hit.transform.position - currentPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestVector = hit.transform.position;
                }
            }

            return nearestVector;
        }
    }
}