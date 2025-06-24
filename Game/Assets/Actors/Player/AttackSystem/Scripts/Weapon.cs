using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Player.AttackSystem.Data;
using Player.Inventory;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using EventBus = EventBusNamespace.EventBus;

namespace Actors.Player.AttackSystem.Scripts
{
    public class Weapon : AttackEnemyAbstract, IDisposable
    {
        [Header("AttackConfig")]
        [SerializeField] private WeaponAttackSettings baseAttackSettings;

        [Header("MessageSettings")] 
        [SerializeField] private string text = "Cooldown attack";
        
        [SerializeField] private GameObject hitObject;

        private WeaponAttackSettings _currentAttackSettings;

#if  UNITY_EDITOR
        [Header("debug")] 
        [SerializeField] private float angle;
        [SerializeField] private Vector2 hitSize;
        [SerializeField] private bool sceneTest;
#endif
        
        protected override void Awake()
        {
            base.Awake();

            if (baseAttackSettings == null)
                baseAttackSettings = Resources.Load<WeaponAttackSettings>("BaseWeaponConfig");

            if (_currentAttackSettings == null)
            {
                _currentAttackSettings = baseAttackSettings;
            }

            MaxComboAttack = _currentAttackSettings.MaxCountAttack;
            EquipWeaponAction = e => UpdateCurrentAttackList(e.WeaponConfig);
            EventBus.Subscribe(EquipWeaponAction);

            UpdateQueueAttack();
        }

        private void Update()
        {
            if (Time.time - LastClick > _currentAttackSettings.AttackSettings.comboWindow &&
                CurrentCountAttack > 0)
            {
                TryStartEndCombo();
            }

            InputLogic();

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnimationName) &&
                PlayerStates.IsAttacking)
            {
                PlayerStates.UpdateAttackState(false);
            }

            if (CurrentCountAttack >= MaxComboAttack &&
                !animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnimationName))
            {
                TryStartEndCombo();
            }
        }

        private void InputLogic()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0) && Cooldown > 0)
            {
                EventBus.Publish(new LogText(text, CustomLogType.Error));
            }
            
            if (CanAttack)
            {
                Attack();
                LastClick = Time.time;
                Cooldown += _currentAttackSettings.AttackSettings.delayBetweenAttacks;
            }
            else if (Cooldown > 0)
            {
                Cooldown -= Time.deltaTime;
            }
        }

        private void Attack()
        {
            if (CurrentCountAttack <= _currentAttackSettings.MaxCountAttack)
            {
                if (_queueAttackDictionary.TryGetValue(CurrentCountAttack, out var attackData))
                {
                    if (AttackCoroutine == null &&
                        !animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnimationName))
                    {
                        PlayerStates.UpdateAttackState(true);
                        AttackCoroutine = StartCoroutine(SetAnimation(attackData.triggerName, hitObject.transform,
                            attackData.weaponHitColliderSettings));
                        CurrentAnimationName = attackData.triggerName;
                        CurrentCountAttack++;
                    }
                }
            }
        }

        protected override IEnumerator SetAnimation(string animationName, Transform hitPositon,
            WeaponHitColliderSettings hitColliderSettings)
        {
            yield return base.SetAnimation(animationName, hitPositon, hitColliderSettings);

            AttackCoroutine = null;
        }

        private void UpdateCurrentAttackList(WeaponAttackSettings weaponAttackSettings)
        {
            _currentAttackSettings = weaponAttackSettings;
            MaxComboAttack = _currentAttackSettings.MaxCountAttack;
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
            if (ExitFromComboCoroutine == null)
            {
                ExitFromComboCoroutine = StartCoroutine(EndComboCoroutine());
            }
        }

        private IEnumerator EndComboCoroutine() 
        {
            AttackCoroutine = null;

            Cooldown = _currentAttackSettings.AttackSettings.attackCooldown;

            yield return new WaitForSeconds(Cooldown);

            foreach (var anim in _queueAttackDictionary)
            {
                animator.ResetTrigger(anim.Value.triggerName);
            }

            BaseExit();
        }


        public void Dispose()
        {
            EventBus.Unsubscribe(EquipWeaponAction);
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