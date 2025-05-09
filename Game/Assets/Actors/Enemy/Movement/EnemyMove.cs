using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Pathfinder;
using Actors.Enemy.Stats.Scripts;
using PlayerNameSpace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

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

        [Inject] private GridCreater gridCreater;

        private EnemyScrObj enemyScrObj;

        private List<Node> _path = new List<Node>();

        [SerializeField] private bool canMove; //=> Vector2.Distance(transform.position, GetPlayerPosition.PlayerPosition().position) <= enemyScrObj.AgressionDistance;
        private bool _canRequestPath;

        private int _nodeCounter;

        private void Awake()
        {
            if (enemyData == null || pathfinder == null)
            {
                enabled = false;
                return;
            }

            enemyScrObj = enemyData.GetEnemyScrObj();
            _canRequestPath = true;
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                if (_canRequestPath)
                {
                    _canRequestPath = false;
                    StartCoroutine(UpdatePath());
                }

                if (_path.Count > 0)
                {
                    Move();
                }
            }
        }

        private IEnumerator UpdatePath()
        {
            yield return new WaitForSeconds(delayForRequestPath);
            pathfinder.FindPath(transform.position, GetPlayerPosition.PlayerPosition().position);
            _path = pathfinder.GetPath();
            _nodeCounter = 0;
            _canRequestPath = true;
        }

        private void Move()
        {
            Debug.Log("path count = " + _path.Count);

            Vector2 targetPosition = _path[_nodeCounter].Waypoint;
            Vector2 moveDirection = (_path[_nodeCounter].Waypoint - rb2D.position).normalized;

            rb2D.MovePosition(rb2D.position + moveDirection * enemyScrObj.Speed * Time.fixedDeltaTime);

            if (CanSwitchMoveNode(targetPosition))
            {
                _nodeCounter++;
            }
        }

        private bool CanSwitchMoveNode(Vector2 nextPoint) =>
            Vector2.Distance(nextPoint, rb2D.position) <= switchNodeDistance;
    }
}