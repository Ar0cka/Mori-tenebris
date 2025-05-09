using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Pathfinder;
using Actors.Enemy.Stats.Scripts;
using PlayerNameSpace;
using UnityEngine;

namespace Actors.Enemy.Movement
{
    [RequireComponent(typeof(Rigidbody2D), typeof(EnemyData))]
    public class EnemyMove : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private PathfinderSystem pathfinder;
        [SerializeField] private Rigidbody2D rb2D;

        [SerializeField] private float switchNodeDistance;
        [SerializeField] private float delayForRequestPath;

        private EnemyScrObj enemyScrObj;

        private List<Node> _path = new List<Node>();

        private bool _canMove;
        private bool _canRequestPath;

        private int _nodeCounter = 0;

        private void Awake()
        {
            if (enemyData == null || pathfinder == null)
            {
                enabled = false;
                return;
            }

            enemyScrObj = enemyData.GetEnemyScrObj();
        }

        private void Update()
        {
            var path = pathfinder.GetPath();

            if (path?.Count > 0)
            {
                Move();
            }
        }

        private IEnumerator UpdatePath()
        {
            yield return new WaitForSeconds(delayForRequestPath);
            //pathfinder.FindPath(transform.position, GetPlayerPosition.PlayerPosition().position);
            _path = pathfinder.GetPath();
            _nodeCounter = 0;
            _canRequestPath = true;
        }

        private void Move()
        {
            rb2D.MovePosition(rb2D.position + _path[0].Waypoint * enemyScrObj.Speed * Time.deltaTime);

            if (CanSwitchMoveNode(_path[0].Waypoint))
            {
                _path.RemoveAt(0);
            }
        }

        private bool CanSwitchMoveNode(Vector2 nextPoint) =>
            Vector2.Distance(nextPoint, transform.position) <= switchNodeDistance;
    }
}