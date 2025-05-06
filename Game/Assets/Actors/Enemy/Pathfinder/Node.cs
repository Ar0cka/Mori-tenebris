using UnityEngine;

namespace Actors.Enemy.Pathfinder
{
    public class Node
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        
        public Vector2 Waypoint { get; private set; }
        public bool IsWalkable { get; private set; }

        public int GCost { get; private set; }
        public int HCost { get; private set; }
        public int FCost => GCost + HCost;
        
        public Node ParentNode { get; private set; }

        public Node(int x, int y, bool isWalkable, Vector2 waypoint)
        {
            X = x;
            Y = y;
            
            IsWalkable = isWalkable;
            Waypoint = waypoint;
            
            GCost = 0;
            HCost = 0;
            
            ParentNode = null;
        }

        public void SetParent(Node parent)
        {
            ParentNode = parent;
        }

        public void CalculateGCost(int gCost)
        {
            GCost = gCost;
        }

        public void CalculateHCost(int hCost)
        {
            HCost = hCost;
        }
    }
}