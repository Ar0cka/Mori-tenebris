using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Player.AttackSystem.Data;
using Player.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using EventBus = EventBusNamespace.EventBus;

namespace Actors.Player.AttackSystem.Scripts
{
    public class WeaponAttack : AbstractAttack, IDisposable
    {
        [Header("AttackConfig")] [SerializeField]
        private WeaponAttackSettings baseAttackSettings;

        private WeaponAttackSettings _currentAttackSettings;
        
        private int _maxComboAttack;
        private int _currentCountAttack;
        private string _currentAnimationName;
        private float _lastClick;
        private bool _isEndCombo;
        private Coroutine _endComboCoroutine;

        private Action<SendEquipWeaponEvent> _equipWeaponAction;
        private bool CanAttack =>
            Input.GetMouseButtonDown(0) && Cooldown <= 0 && !_isEndCombo && GlobalAttackStates.IsBusy;

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
            if (Time.time - _lastClick > _currentAttackSettings.AttackSettings.comboWindow && !_isEndCombo && _currentCountAttack > 0)
            {
                TryStartEndCombo();
            }
            
            InputLogic();

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(_currentAnimationName) && GlobalAttackStates.IsAttacking)
            {
                GlobalAttackStates.UpdateAttackState(false);
            }

            if (!_isEndCombo && _currentCountAttack >= _maxComboAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName(_currentAnimationName))
            {
                TryStartEndCombo();
            }
        }

        private void InputLogic()
        {
            if (CanAttack)
            {
                Attack();
                _lastClick = Time.time;
                Cooldown += 0.1f;
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
                if (_queueAttackDictionary.TryGetValue(_currentCountAttack, out var animationName))
                {
                    SetAnimation(animationName);
                    _currentAnimationName = animationName;
                    GlobalAttackStates.UpdateAttackState(true);
                    _currentCountAttack++;
                }
            }
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
                if (!_queueAttackDictionary.ContainsKey(attack.countQueue))
                {
                    _queueAttackDictionary.Add(attack.countQueue, attack.triggerName);
                }
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
            
            Cooldown = _currentAttackSettings.AttackSettings.attackCooldown;

            yield return new WaitForSeconds(Cooldown);
            
            foreach (var anim in _queueAttackDictionary)
            {
                animator.ResetTrigger(anim.Value);
            }

            _currentCountAttack = 0;
            _currentAnimationName = "";
            _isEndCombo = false;
        }
        
        
        public void Dispose()
        {
            EventBus.Unsubscribe(_equipWeaponAction);
        }
    }
}