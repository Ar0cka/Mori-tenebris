using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
 namespace Player.PlayerAttack
 {
     public abstract class AbstractAttackClass : MonoBehaviour
     {
         [Tooltip("Назначать по порядку срабатывания + очередь начинается с 1")] 
         [SerializeField] private float comboWindow;
         [SerializeField] protected List<AttackSetting> attacks;
         [SerializeField] protected Animator animator;

         [SerializeField] private float exitFromComboDelay;
         
         private int _maxCountAttack;
         
         [Min(1)] private int _currentCountAttack;
         private string _currentAnimation;
         private float _lastClicked;
         private Dictionary<int, AttackSetting> _attackData = new Dictionary<int, AttackSetting>();

         private bool _isEndCombo;

         protected void Awake()
         {
             _maxCountAttack = attacks.Count;
             
             _currentCountAttack = Math.Clamp(_currentCountAttack, 1, _maxCountAttack);
             
             foreach (var attack in attacks)
             {
                 if (!_attackData.ContainsKey(attack.positionInQueue))
                 {
                     _attackData.Add(attack.positionInQueue, attack);
                 }
             }

             _currentCountAttack = 1;
         }

         public virtual void UpdateLogic()
         {
             if (Time.time - _lastClicked > comboWindow)
             {
                 EndCombo();
             }
             
             if (CanStartNextAttack())
             {
                 if (_attackData.TryGetValue(_currentCountAttack, out var value) && Input.GetKeyDown(value.comboKey))
                 {
                     _lastClicked = Time.time;
                     _currentAnimation = value.nameTrigger;
                     
                     _currentCountAttack++;
                     
                     StartAnimation(value.nameTrigger);
                 }
             }
             else if (!_isEndCombo)
             {
                 _isEndCombo = true;
                 StartCoroutine(EndCombo());
             }
         }

         protected void StartAnimation(string animationName)
         {
             animator.SetTrigger(animationName);
         }
         
         protected bool CanStartNextAttack()
         {
             if (_currentCountAttack > _maxCountAttack)
             {
                 return false;
             }

             return true;
         }

         private void ResetCount()
         {
             _currentCountAttack = 1;
         }

         private void ResetAllTriggers()
         {
             foreach (var attack in attacks)
             {
                 animator.ResetTrigger(attack.nameTrigger);
             }
         }

         protected IEnumerator EndCombo()
         {
             yield return new WaitForSeconds(exitFromComboDelay);
             
             if (!string.IsNullOrEmpty(_currentAnimation) && !animator.GetCurrentAnimatorStateInfo(0).IsName(_currentAnimation))
             {
                 ResetCount();
                 ResetAllTriggers();
             }
             
             _isEndCombo = false;
         }
     }

     [Serializable]
     public class AttackSetting
     {
         public string nameTrigger;
         [Min(1)] public int positionInQueue;
         public KeyCode comboKey;
     }
 }