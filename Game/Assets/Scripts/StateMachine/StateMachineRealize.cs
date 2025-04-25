using PlayerNameSpace;
using StateMachin.States;
using UnityEngine;

public class StateMachineRealize : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerScrObj playerScrObj;
    [SerializeField] private Rigidbody2D rb2D;

    private FStateMachine stateMachine;

    private void Start()
    {
        stateMachine = new FStateMachine();
        
        stateMachine.AddNewState(new IdleState(stateMachine, this));
        stateMachine.AddNewState(new MovementState(stateMachine, this, playerScrObj, rb2D));
        
        stateMachine.ChangeState<IdleState>();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void SetWalkAnimation(bool stateAnimation)
    {
        animator?.SetBool("Walk", stateAnimation);
    }
}