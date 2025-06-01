using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Enemy.Data.Scripts;
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
    [RequireComponent(typeof(Rigidbody2D), typeof(EnemyData))]
    public class EnemyMove : MonoBehaviour
    {
        #region param

        [Header("Components")]
        [SerializeField] private EnemyData enemyData;
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private CapsuleCollider2D capsuleCollider2D;
        [SerializeField] private MovementOffsetScr colliderOffset;

        [Header("MoveSettings")]
        [SerializeField] private float switchNodeDistance;
        [SerializeField] private float delayForRequestPath;
        
        [Inject] private IPathFind _pathfinder;

        private EnemyScrObj enemyScrObj;

        private List<Node> _path = new List<Node>();
        private bool _canMove => Vector2.Distance(transform.position, _playerPosition.position) <= enemyScrObj.AgressionDistance;
        private bool _move;
        private bool _canRequestPath;

        private int _nodeCounter;

        private Transform _playerPosition;
        
        #endregion
        
        private void Awake()
        {
            if (ValidateComponents())
            {
                enabled = false;
                return;
            }

            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
            
            enemyScrObj = enemyData.GetEnemyScrObj();
            _canRequestPath = true;
        }

        private void FixedUpdate()
        {
            if (_canMove)
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
            _pathfinder.FindPath(transform.position, _playerPosition.position);
            _path = _pathfinder.GetPath();
            _nodeCounter = 0;
            _canRequestPath = true;
        }

        private void Move()
        {
            Debug.Log("path count = " + _path.Count);

            Vector2 targetPosition = _path[_nodeCounter].Waypoint;
            Vector2 moveDirection = (_path[_nodeCounter].Waypoint - rb2D.position).normalized;

            SetModelSettings(moveDirection);
            
            rb2D.MovePosition(rb2D.position + moveDirection * enemyScrObj.Speed * Time.time);

            if (CanSwitchMoveNode(targetPosition))
            {
                _nodeCounter++;
            }
        }

        private void SetModelSettings(Vector2 moveDirection)
        {
            _move = moveDirection.magnitude > 0.01f;
            
            animator.SetBool("Walk", _move);
            spriteRenderer.flipX = moveDirection.x > 0;
            capsuleCollider2D.offset = moveDirection.x < 0 ? colliderOffset.MoveLeftOffset : colliderOffset.MoveRightOffset;
        }
        private bool CanSwitchMoveNode(Vector2 nextPoint) =>
            Vector2.Distance(nextPoint, rb2D.position) <= switchNodeDistance;
        private bool ValidateComponents()
        {
            return enemyData == null || _pathfinder == null || animator == null || rb2D == null || spriteRenderer == null;
        }
        
        
    }
}