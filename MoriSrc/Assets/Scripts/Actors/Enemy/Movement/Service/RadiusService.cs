using Actors.Enemy.Movement.Enums;
using ScrObj.EnemyMoveScr;
using UnityEngine;

namespace Actors.Enemy.Movement.Service
{
    public class RadiusService<TRadiusConfig> where TRadiusConfig : RadiusSettings
    {
        public AiRadiusEnum CurrentAiRadius { get; private set; } = AiRadiusEnum.Idle;
        
        private readonly TRadiusConfig _radiusConfig;

        public RadiusService(TRadiusConfig radiusConfig)
        {
            _radiusConfig = radiusConfig;
            
        }
        
        public AiRadiusEnum CheckCirclePosition(Vector2 targetPosition, Vector2 currentPosition)
        {
            float distance = Vector2.Distance(targetPosition, currentPosition);
            
            foreach (var item in _radiusConfig.RadiusDictionary)
            {
                if (CheckDistance(distance, item.Value))
                {
                    CurrentAiRadius = item.Key;
                    return CurrentAiRadius;
                }
            }

            return AiRadiusEnum.Idle;
        }

        private bool CheckDistance(float distance, float radius)
        {
            return distance >= radius;
        }
    }
}