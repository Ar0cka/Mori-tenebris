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

        private Dictionary<PositionSettings, ObstaclesSettings> _placedNodes = new();

        private int _tryCount;
        private int _maxTryCount;
        
        public void SpawnScene()
        {
            if (_placedNodes.Count > 0) 
                ClearObstacleDictionary();
            
            float sizeSpriteX = backgroundSprite.bounds.size.x;
            float sizeSpriteY = backgroundSprite.bounds.size.y;

            Debug.Log($"SizeSpriteX = {sizeSpriteX}, SizeSpriteY = {sizeSpriteY}");
            
            Vector3 result = CalculateSizeSprite(sizeSpriteX, sizeSpriteY);
            
            backSpriteRenderer.sprite = backgroundSprite;
            
            if (result != Vector3.zero)
                backSpriteRenderer.transform.localScale = result;
            
            if (obstacles.Count > 0) SpawnObstacles();
        }

        private Vector2 CalculateSizeSprite(float sizeX, float sizeY)
        {
            float sizeWorldX = worldSize.x * 1f / sizeX;
            float sizeWorldY = worldSize.y * 1f / sizeY;

            if (sizeWorldY <= 0 || sizeWorldX <= 0) return Vector2.zero;
            
            return new Vector2(sizeWorldX, sizeWorldY);
        }

        
        #region Obstacles

        private void ClearObstacleDictionary()
        {
            foreach (var obstacle in _placedNodes)
            {
                var item = obstacle.Value.referencePrefab;
                if (item != null && !Application.isPlaying)
                    DestroyImmediate(item);
                else if (item != null)
                    Destroy(item);
            }
            
            _placedNodes.Clear();
        }
        private void SpawnObstacles()
        {
            _maxTryCount = worldSize.x * worldSize.y * 2;
            
            foreach (ObstaclesSettings obstacle in obstacles)
            {
                var cloneObstacle = obstacle.Clone();
                
                var obj = Instantiate(cloneObstacle.prefab, cloneObstacle.parent);
                
                if (obj == null) continue;
                
                var position = GetWorldPosition(cloneObstacle.size.x, cloneObstacle.size.y);

                if (position == null)
                {
                    Debug.Log("Not have free nodes");
                    break;
                }
                
                if (position.WorldPosition == Vector3.zero) 
                    continue;
                
                obj.transform.position = position.WorldPosition;
                
                cloneObstacle.referencePrefab = obj;

                if (!_placedNodes.TryAdd(position, cloneObstacle))
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
        private PositionSettings GetWorldPosition(float sizeX, float sizeY)
        {
            _tryCount = 0;
            
            int gridWidth = Mathf.FloorToInt(worldSize.x / sizeX);
            int gridHeight = Mathf.FloorToInt(worldSize.y / sizeY);
            
            while (_tryCount++ < _maxTryCount)
            {
                int randomNodeX = Random.Range(0, gridWidth);
                int randomNodeY = Random.Range(0, gridHeight);
                
                Vector2 bottomLeft = (Vector2) transform.position - Vector2.right * worldSize.x / 2 - Vector2.up * worldSize.y / 2;
                
                Vector2 waypoint = bottomLeft + Vector2.right * (randomNodeX * sizeX) + Vector2.up * (randomNodeY * sizeY);
                
                if (!CheckObstacles(waypoint))
                {
                    continue;
                }
            
                PositionSettings positionSettings = new PositionSettings(randomNodeX, randomNodeY, waypoint);

                return positionSettings;
            }

            return null;
        }

        private bool CheckObstacles(Vector2 waypoint)
        {
            foreach (var item in _placedNodes)
            {
                Vector2 itemPos = item.Key.WorldPosition;

                if (!Distance(itemPos, waypoint))
                {
                    return false;
                }
            }

            return true;
        }

        private bool Distance(Vector2 distance1, Vector2 distance2)
        {
            if (Vector2.Distance(distance1, distance2) < maxDistance) return false;
            
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

        public GameObject referencePrefab;

        public ObstaclesSettings Clone()
        {
            return (ObstaclesSettings)MemberwiseClone();
        }
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