using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Pathfinder
{
    public class GridCreater : MonoBehaviour
    {
        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private LayerMask unwalkableMask;
        [SerializeField] private float nodeRadius;
        [SerializeField] private float overlapRadius;

        private float nodeDiameter;

        private int gridWidth;
        private int gridHeight;

        private Node[,] grid;

        private void Start()
        {
            nodeDiameter = nodeRadius * 2;
            gridWidth = Mathf.FloorToInt(gridWorldSize.x / nodeDiameter);
            gridHeight = Mathf.FloorToInt(gridWorldSize.y / nodeDiameter);

            InitializeGrid();
            
#if UNITY_EDITOR
             Debug.Log("InitializeGrid :)");
#endif
        }

        private void InitializeGrid()
        {
            grid = new Node[gridWidth, gridHeight];
            Vector2 bottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 -
                                 Vector2.up * gridWorldSize.y / 2;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector2 waypoint = bottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) +
                                       Vector2.up * (y * nodeDiameter + nodeRadius);

                    bool isWalkable = !Physics2D.OverlapCircle(waypoint, overlapRadius, unwalkableMask);
                    
                    grid[x, y] = new Node(x, y, isWalkable, waypoint);
                }
            }
        }

        public Node NodeFromWorldPosition(Vector2 currentPosition)
        {
            float perX = (currentPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float perY = (currentPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

            perX = Mathf.Clamp01(perX);
            perY = Mathf.Clamp01(perY);

            int x = Mathf.RoundToInt((gridWidth - 1) * perX);
            int y = Mathf.RoundToInt((gridHeight - 1) * perY);

            return grid[x, y];
        }

        public List<Node> GetNeighbour(Node node)
        {
            List<Node> neighbours = new List<Node>();

            int[] dx = { 1, -1, 0, 0, -1, -1, 1, 1 };
            int[] dy = { 0, 0, -1, 1, 1, -1, -1, 1 };

            for (int i = 0; i < dx.Length; i++)
            {
                int checkX = node.X + dx[i];
                int checkY = node.Y + dy[i];

                if (checkX < gridWidth && checkY < gridHeight && checkX >= 0 && checkY >= 0)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }

            return neighbours;
        }
    }
}