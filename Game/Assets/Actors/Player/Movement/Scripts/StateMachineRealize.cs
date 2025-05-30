using System;
using Actors.Player.Movement.Scripts;
using EventBusNamespace;
using PlayerNameSpace;
using StateMachin.States;
using UnityEngine;

public class StateMachineRealize : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerScrObj playerScrObj;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private MovementOffsetScr movementOffsetScr;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    private FStateMachine stateMachine;

    private Action<MovementOffsetScr> _changeOffset;
    
    private bool initialized = false;

    public void Initialize(ISubtractionStamina subtractionStamina)
    {
        _changeOffset = ChangeModel;
        
        if (_changeOffset != null) EventBus.Subscribe(_changeOffset);
        
        stateMachine = new FStateMachine();
        
        stateMachine.AddNewState(new IdleState(stateMachine, this));
        stateMachine.AddNewState(new MovementState(stateMachine, this, playerScrObj, rb2D, spriteRenderer, movementOffsetScr, capsuleCollider));
        stateMachine.AddNewState(new SprintRun(stateMachine, this, playerScrObj, rb2D, subtractionStamina, spriteRenderer, movementOffsetScr, capsuleCollider));
        
        stateMachine.ChangeState<IdleState>();
        
        initialized = true;
    }

    private void Update()
    {
        if (initialized)
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (initialized)
        stateMachine.FixedUpdate();
    }

    public void SetWalkAnimation(bool stateAnimation)
    {
        animator?.SetBool("Walk", stateAnimation);
    }

    private void ChangeModel(MovementOffsetScr offsetScr)
    {
        stateMachine.UpdateStateData<MovementState>(offsetScr);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(_changeOffset);
    }
}