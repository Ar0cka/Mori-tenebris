using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SceneSettings.LevelGenerator
{
    #if UNITY_EDITOR
    public class SceneSettings : MonoBehaviour
    {
        [SerializeField] private Vector2Int worldSize;
        
        [SerializeField] private SpriteRenderer backSpriteRenderer;

        [Header("Scene assets")]
        [SerializeField] private Sprite backgroundSprite;
        [SerializeField] private List<ObstaclesSettings> obstacles;
        [SerializeField] private int indexObstacle;

        [Header("Spawn settings")] 
        [SerializeField] private int maxDistance;
        
        [SerializeField] private List<GameObject> spawnedObstacles;

        private Dictionary<PositionSettings, ObstaclesSettings> _placedNodes = new();

        private int _tryCount;
        private int _maxTryCount;
        
        public void SpawnScene()
        {
            float sizeSpriteX = backgroundSprite.bounds.size.x;
            float sizeSpriteY = backgroundSprite.bounds.size.y;

            Debug.Log($"SizeSpriteX = {sizeSpriteX}, SizeSpriteY = {sizeSpriteY}");
            
            Vector3 result = CalculateSizeSprite(sizeSpriteX, sizeSpriteY);
            
            backSpriteRenderer.sprite = backgroundSprite;
            
            if (result != Vector3.zero)
                backSpriteRenderer.transform.localScale = result;
            
            if (obstacles.Count > 0) SpawnObstacles();
        }

        #region Obstacles
        private void SpawnObstacles()
        {
            _maxTryCount = worldSize.x * worldSize.y * 2;
            
            foreach (ObstaclesSettings obstacle in obstacles)
            {
                var obj = Instantiate(obstacle.prefab, obstacle.parent);
                
                if (obj == null) continue;
                
                var position = GetWorldPosition(obstacle.size.x, obstacle.size.y);

                if (position == null)
                {
                    Debug.Log("Not have free nodes");
                    break;
                }
                
                if (position.WorldPosition == Vector3.zero) 
                    continue;
                
                obj.transform.position = position.WorldPosition;

                if (!_placedNodes.TryAdd(position, obstacle))
                {
                    Debug.LogError("Can't add obstacle");
                }
            }
        }

        public void SpawnIndexObstacles()
        {
            if (indexObstacle >= obstacles.Count)
            {
                Debug.LogError($"{indexObstacle} incorrect. Current count items = {obstacles.Count}");
                return;
            }
            
            var obj = Instantiate(obstacles[indexObstacle].prefab, obstacles[indexObstacle].parent);
        }
        #endregion
        
        private Vector2 CalculateSizeSprite(float sizeX, float sizeY)
        {
            float sizeWorldX = worldSize.x * 1f / sizeX;
            float sizeWorldY = worldSize.y * 1f / sizeY;

            if (sizeWorldY <= 0 || sizeWorldX <= 0) return Vector2.zero;
            
            return new Vector2(sizeWorldX, sizeWorldY);
        }

        private PositionSettings GetWorldPosition(float sizeX, float sizeY)
        {
            _tryCount = 0;
            
            while (_tryCount++ < _maxTryCount)
            {
                int gridWidth = worldSize.x;
                int gridHeight = worldSize.y;
            
                int randomNodeX = Random.Range(0, gridWidth);
                int randomNodeY = Random.Range(0, gridHeight);

                if (!CheckObstacles(randomNodeX, randomNodeY))
                {
                    continue;
                }

                float offsetX = -worldSize.x / 2f * sizeX;
                float offsetY = -worldSize.y / 2f * sizeY;

                Vector2 waypoint = (Vector2)transform.position 
                                   + new Vector2(offsetX + randomNodeX * sizeX, offsetY + randomNodeY * sizeY);
            
                PositionSettings positionSettings = new PositionSettings(randomNodeX, randomNodeY, waypoint);

                return positionSettings;
            }

            return null;
        }

        private bool CheckObstacles(int x, int y)
        {
            foreach (var item in _placedNodes)
            {
                int distanceX = item.Key.X - x;
                int distanceY = item.Key.Y - y;

                if (!Distance(distanceX, distanceY))
                {
                    return false;
                }
            }
            
            return true;
        }

        private bool Distance(int dX, int dY)
        {
            dX = Mathf.Abs(dX);
            dY = Mathf.Abs(dY);
            
            if (dX <= 0 && dY <= 0) return false;
            
            if (dX <= maxDistance || dY <= maxDistance) return false;
            
            return true;
        }
        
        #region Gizmos

        public void OnDrawGizmos()
        {
            if (worldSize != Vector2.zero)
                Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, worldSize.y, 0f));
        }
        #endregion
    }

    #region Obstacles

    [Serializable]
    public class ObstaclesSettings
    {
        public string obstacleName;
        
        public GameObject prefab;
        public Transform parent;
        public Vector2 size;
    }
    public class PositionSettings
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Vector3 WorldPosition { get; private set; }

        public PositionSettings(int x, int y, Vector3 worldPosition)
        {
            X = x;
            Y = y;
            WorldPosition = worldPosition;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is PositionSettings other)
            {
                return X == other.X && Y == other.Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return X * 397 ^ Y;
        }
}
    
    #endregion
    
    #region Editor
    
    [CustomEditor(typeof(SceneSettings))]
    public class SceneGenerationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var sceneSettings = (SceneSettings)target;

            if (GUILayout.Button("Generate obstacle"))
            {
                sceneSettings.SpawnIndexObstacles();
            }
            
            if (GUILayout.Button("Generate Scene"))
            {
                sceneSettings.SpawnScene();
            }
        }
    }

    #endregion

    #region Enums

    public enum TypeGeneration
    {
        Manual,
        Procedure
    }

    #endregion
    
    #endif
}