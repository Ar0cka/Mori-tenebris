using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Enemy.Pathfinder
{
    public class PathfinderSystem : MonoBehaviour
    {
        [SerializeField] private GridCreater gridInitialize;

        [SerializeField] private Transform currentPos;
        [SerializeField] private Transform seeker;

        private List<Node> _path;

        private void Update()
        {
            if(Input.GetButtonDown("Jump"))
                FindPath(currentPos.position, seeker.position);
        }

        public void FindPath(Vector2 startPos, Vector2 goalPos)
        {
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

            foreach (var pathNode in _path)
            {
                Debug.Log($"{pathNode.X},{pathNode.Y}");
            }
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
        
        private void OnDrawGizmos()
        {
            if (_path != null)
            {
                foreach (var p in _path)
                {
                    Gizmos.color = Color.black;
                    Debug.Log("Drawing path");
                    Gizmos.DrawSphere(p.Waypoint, 0.2f);
                }
            }
        }
    }
}