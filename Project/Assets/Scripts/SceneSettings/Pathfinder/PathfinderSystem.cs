using System.Collections.Generic;
using System.Linq;
using Actors.Enemy.Pathfinder.Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Pathfinder
{
    public class PathfinderSystem : MonoBehaviour, IPathFind
    {
        [SerializeField] private GridCreater gridInitialize;

        private List<Node> _path;

        public void FindPath(Vector2 startPos, Vector2 goalPos)
        {
            if (startPos == goalPos || startPos == Vector2.zero || goalPos == Vector2.zero) return;
            
            Node startNode = gridInitialize.NodeFromWorldPosition(startPos);
            Node goalNode = gridInitialize.NodeFromWorldPosition(goalPos);
            
            List<Node> openList = new List<Node>();
            HashSet<Node> closeList = new HashSet<Node>();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost &&
                        openList[i].HCost < currentNode.HCost)
                    {
                        currentNode = openList[i];
                    }
                }
                
                openList.Remove(currentNode);
                closeList.Add(currentNode);

                if (currentNode == goalNode) 
                {
                    RequestPath(startNode, goalNode);
                    return;
                }

                foreach (var neighbours in gridInitialize.GetNeighbour(currentNode))
                {
                    if (!neighbours.IsWalkable || closeList.Contains(neighbours))
                    { 
                        continue;
                    }
                    
                    
                    int newMovementCost = currentNode.GCost + GetDistance(currentNode, neighbours);

                    if (newMovementCost < neighbours.GCost || !openList.Contains(neighbours))
                    {
                        neighbours.CalculateGCost(newMovementCost);
                        neighbours.CalculateHCost(GetDistance(neighbours, goalNode));
                        neighbours.SetParent(currentNode);
                        
                        if (!openList.Contains(neighbours))
                        {
                            openList.Add(neighbours);
                        }
                    }
                }
            }
        }

        private void RequestPath(Node startNode, Node endNode)
        {
            Debug.Log("requestPath");
            List<Node> currentPath = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                currentPath.Add(currentNode);
                currentNode = currentNode.ParentNode;
            }
            
            currentPath.Reverse();
            _path = currentPath;
        }

        private int GetDistance(Node nodeA, Node NodeB)
        {
            int distX = Mathf.Abs(nodeA.X - NodeB.X);
            int distY = Mathf.Abs(nodeA.Y - NodeB.Y);

            if (distX > distY) return 14 * distY + 10 * (distX - distY);  
  
            return 14 * distX + 10 * (distY - distX);  
        }

        public List<Node> GetPath() => _path;
        
        public bool CheckPathCount() => _path?.Count > 0;
    }
}