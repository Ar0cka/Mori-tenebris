using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
using Actors.Enemy.Monsters.AbstractEnemy;
using Actors.Enemy.Pathfinder;
using Actors.Enemy.Pathfinder.Interface;
using Actors.Enemy.Stats.Scripts;
using Actors.Player.Movement.Scripts;
using PlayerNameSpace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Actors.Enemy.Movement
{
    public class EnemyMove : MonoBehaviour
    {
        #region param

        [Header("Components")]
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private SpriteController spriteController;

        [Header("MoveSettings")]
        [SerializeField] private float switchNodeDistance;
        [SerializeField] private float delayForRequestPath;
        [SerializeField] private float stopDistance;
        
        [Inject] private IPathFind _pathfinder;

        private MonsterScrObj _monsterScrObj;
        private EnemyConfig MonsterConfig => _monsterScrObj?.GetConfig();

        private List<Node> _path = new List<Node>();
        
        private bool _seePlayer;
        private bool _canMove;
        private StateController _stateController;
        
        private bool _canRequestPath;

        private int _nodeCounter;
        
        #endregion
        
        public void Initialize()
        {
            if (ValidateComponents())
            {
                enabled = false;
                return;
            }
            
            _monsterScrObj = enemyData.GetEnemyScrObj();
            _stateController = enemyData.GetStateController();
            
            _canRequestPath = true;
        }

        private void Update()
        {
            if (_monsterScrObj != null)
            {
               _seePlayer = CheckDistanceWithPlayer(MonsterConfig.agressionDistance);
               _canMove = !CheckDistanceWithPlayer(stopDistance);
            }
        }

        private void FixedUpdate()
        {
            if (CanMove())
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
            else
            {
                spriteController.SetModelSettings(Vector2.zero);
            }
        }

        private IEnumerator UpdatePath()
        {
            yield return new WaitForSeconds(delayForRequestPath);
            _pathfinder.FindPath(transform.position, enemyData.PlayerPosition.position);
            _path = _pathfinder.GetPath();
            _nodeCounter = 0;
            _canRequestPath = true;
        }

        private void Move()
        {
            Vector2 moveDirection = Vector2.zero;
            
            if (_nodeCounter < _path.Count)
            {
                Vector2 targetPosition = _path[_nodeCounter].Waypoint;
                moveDirection = Vector2.zero;

                if (_seePlayer && _canMove)
                {
                    moveDirection = (targetPosition - rb2D.position).normalized;
            
                    rb2D.MovePosition(rb2D.position + moveDirection * MonsterConfig.speed * Time.fixedDeltaTime);

                    if (CanSwitchMoveNode(targetPosition))
                    {
                        _nodeCounter++;
                    }
                }
            }
            spriteController.SetModelSettings(moveDirection);
        }
        private bool CanSwitchMoveNode(Vector2 nextPoint) =>
            Vector2.Distance(nextPoint, rb2D.position) <= switchNodeDistance;

        private bool CheckDistanceWithPlayer(float distance) =>
            Vector2.Distance(transform.position, enemyData.PlayerPosition.position) <= distance;
        
        private bool ValidateComponents()
        {
            return enemyData == null || _pathfinder == null;
        }

        private bool CanMove()
        {
            return _canMove && _seePlayer && _stateController.CanAttack();
        } 
    }
}