using PlayerNameSpace;
using StateMachin.States;
using UnityEngine;

public class StateMachineRealize : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerScrObj playerScrObj;
    [SerializeField] private Rigidbody2D rb2D;

    private FStateMachine stateMachine;
    
    private bool initialized = false;

    public void Initialize(ISubtractionStamina subtractionStamina)
    {
        PlayerTransform.PlayerInitialize(transform);
        
        stateMachine = new FStateMachine();
        
        stateMachine.AddNewState(new IdleState(stateMachine, this));
        stateMachine.AddNewState(new MovementState(stateMachine, this, playerScrObj, rb2D));
        stateMachine.AddNewState(new SprintRun(stateMachine, this, playerScrObj, rb2D, subtractionStamina));
        
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
}