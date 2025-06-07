using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Player.AttackSystem.Data;
using Player.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using EventBus = EventBusNamespace.EventBus;

namespace Actors.Player.AttackSystem.Scripts
{
    public class WeaponAttack : AbstractAttack, IDisposable
    {
        [Header("AttackConfig")] [SerializeField]
        private WeaponAttackSettings baseAttackSettings;

        [SerializeField] private GameObject hitObject;

        private WeaponAttackSettings _currentAttackSettings;

        private int _maxComboAttack;
        private int _currentCountAttack;
        private string _currentAnimationName;
        private float _lastClick;
        private bool _isEndCombo;
        private Coroutine _endComboCoroutine;
        private Coroutine _attackCorutine;

#if  UNITY_EDITOR
        [Header("debug")] 
        [SerializeField] private float angle;
        [SerializeField] private Vector2 hitSize;
        [SerializeField] private bool sceneTest;
#endif
        

        private Action<SendEquipWeaponEvent> _equipWeaponAction;

        private bool CanAttack =>
            Input.GetMouseButtonDown(0) && Cooldown <= 0 && !_isEndCombo && !GlobalAttackStates.IsBusy;

        protected override void Awake()
        {
            base.Awake();

            if (baseAttackSettings == null)
                baseAttackSettings = Resources.Load<WeaponAttackSettings>("BaseWeaponConfig");

            if (_currentAttackSettings == null)
            {
                _currentAttackSettings = baseAttackSettings;
            }

            _maxComboAttack = _currentAttackSettings.MaxCountAttack;
            _equipWeaponAction = e => UpdateCurrentAttackList(e.WeaponConfig);
            EventBus.Subscribe(_equipWeaponAction);

            UpdateQueueAttack();
        }

        private void Update()
        {
            if (Time.time - _lastClick > _currentAttackSettings.AttackSettings.comboWindow && !_isEndCombo &&
                _currentCountAttack > 0)
            {
                TryStartEndCombo();
            }

            InputLogic();

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(_currentAnimationName) &&
                GlobalAttackStates.IsAttacking)
            {
                GlobalAttackStates.UpdateAttackState(false);
            }

            if (!_isEndCombo && _currentCountAttack >= _maxComboAttack &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName(_currentAnimationName))
            {
                TryStartEndCombo();
            }
        }

        private void InputLogic()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (CanAttack)
            {
                Attack();
                _lastClick = Time.time;
                Cooldown += _currentAttackSettings.AttackSettings.delayBetweenAttacks;
            }
            else if (Cooldown > 0)
            {
                Cooldown -= Time.deltaTime;
            }
        }

        private void Attack()
        {
            if (_currentCountAttack <= _currentAttackSettings.MaxCountAttack)
            {
                if (_queueAttackDictionary.TryGetValue(_currentCountAttack, out var attackData))
                {
                    if (_attackCorutine == null &&
                        !animator.GetCurrentAnimatorStateInfo(0).IsName(_currentAnimationName))
                    {
                        _attackCorutine = StartCoroutine(SetAnimation(attackData.triggerName, hitObject.transform,
                            attackData.weaponHitColliderSettings));
                        _currentAnimationName = attackData.triggerName;
                        GlobalAttackStates.UpdateAttackState(true);
                        _currentCountAttack++;
                    }
                }
            }
        }

        protected override IEnumerator SetAnimation(string animationName, Transform hitPositon,
            WeaponHitColliderSettings hitColliderSettings)
        {
            yield return base.SetAnimation(animationName, hitPositon, hitColliderSettings);

            _attackCorutine = null;
        }

        private void UpdateCurrentAttackList(WeaponAttackSettings weaponAttackSettings)
        {
            _currentAttackSettings = weaponAttackSettings;
            _maxComboAttack = _currentAttackSettings.MaxCountAttack;
        }

        private void UpdateQueueAttack()
        {
            List<AttackData> sortingList = _currentAttackSettings.AttackData.OrderBy(a => a.countQueue).ToList();

            foreach (var attack in sortingList)
            {
                _queueAttackDictionary.TryAdd(attack.countQueue, attack);
            }
        }

        private void TryStartEndCombo()
        {
            if (_endComboCoroutine == null)
            {
                _endComboCoroutine = StartCoroutine(EndComboCoroutine());
            }
        }

        private IEnumerator EndComboCoroutine()
        {
            _isEndCombo = true;
            _attackCorutine = null;

            Cooldown = _currentAttackSettings.AttackSettings.attackCooldown;

            yield return new WaitForSeconds(Cooldown);

            foreach (var anim in _queueAttackDictionary)
            {
                animator.ResetTrigger(anim.Value.triggerName);
            }

            _currentCountAttack = 0;
            _currentAnimationName = "";
            _isEndCombo = false;
            _endComboCoroutine = null;
        }


        public void Dispose()
        {
            EventBus.Unsubscribe(_equipWeaponAction);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;

            Matrix4x4 rotationMatrix = Matrix4x4.zero;
            
            if (sceneTest)
            {
                rotationMatrix = Matrix4x4.TRS(hitObject.transform.position, Quaternion.Euler(0, 0, angle),
                    Vector3.one);
            }
            else
            {
                rotationMatrix = Matrix4x4.TRS(hitObject.transform.position, Quaternion.Euler(0, 0, CurrentAngle),
                    Vector3.one);
            }
           
            Gizmos.matrix = rotationMatrix;

            if (sceneTest)
            {
                Gizmos.DrawWireCube(Vector3.zero, hitSize);
            }
            else
            {
                Gizmos.DrawWireCube(Vector3.zero, CurrentSize);
            }
        }
#endif
    }
}