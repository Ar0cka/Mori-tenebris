using System;
using Actors.Player.Movement.Scripts;
using EventBusNamespace;
using PlayerNameSpace;
using StateMachin.States;
using UnityEngine;
using UnityEngine.Serialization;

public class StateMachineRealize : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerScrObj playerScrObj;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [FormerlySerializedAs("movementOffsetScr")] [SerializeField] private MovementColliderOffset movementColliderOffset;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    private FStateMachine stateMachine;

    private Action<MovementColliderOffset> _changeOffset;
    
    private bool _isInitialized;
    
    public float RollCooldown { get; private set; }
    
    public void Initialize(ISubtractionStamina subtractionStamina)
    {
        _changeOffset = ChangeModel;
        
        if (_changeOffset != null) EventBus.Subscribe(_changeOffset);
        
        stateMachine = new FStateMachine();
        
        stateMachine.AddNewState(new IdleState(stateMachine, this, playerScrObj));
        stateMachine.AddNewState(new MovementState(stateMachine, this, playerScrObj, rb2D, spriteRenderer, movementColliderOffset, capsuleCollider, animator));
        stateMachine.AddNewState(new SprintRun(stateMachine, this, playerScrObj, rb2D, subtractionStamina, spriteRenderer, movementColliderOffset, capsuleCollider, animator));
        stateMachine.AddNewState(new Roll(stateMachine, this, playerScrObj, rb2D, spriteRenderer,  capsuleCollider, animator, subtractionStamina));
        
        stateMachine.ChangeState<IdleState>();
        
        _isInitialized = true;
        
        Debug.Log("Initialize statemachine");
    }

    private void Update()
    {
        if (_isInitialized)
        {
            if (RollCooldown > 0)
            {
                RollCooldown -= Time.deltaTime;
            }
            
            stateMachine.Update();
        }
    }

    private void FixedUpdate()
    {
        if (_isInitialized)
        stateMachine.FixedUpdate();
    }

    private void ChangeModel(MovementColliderOffset colliderOffset)
    {
        stateMachine.UpdateStateData<MovementState>(colliderOffset);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(_changeOffset);
    }

    public void RollCooldownReset(float cooldwon)
    {
        RollCooldown = cooldwon;
    }
}