using System.Collections.Generic;
using UnityEngine;

namespace Actors.Enemy.Pathfinder.Interface
{
    public interface IPathFind
    {
        public void FindPath(Vector2 startPos, Vector2 goalPos);
        bool CheckPathCount();
        List<Node> GetPath();
    }
}