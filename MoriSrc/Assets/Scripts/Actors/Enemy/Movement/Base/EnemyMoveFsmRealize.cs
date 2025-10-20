using Actors.Enemy.Movement.Service;
using FiniteStateMachine;
using ScrObj.EnemyMoveScr;
using UnityEngine;
using Zenject;

namespace Actors.Enemy.Movement
{
    public class EnemyMoveFsmRealize<TConfig> : FsmRealizeBase<EnemyMoveFsm, MoveEnemyFsmUnityState> 
        where TConfig : MoveData
    {
        [Header("Configs")]
        [SerializeField] protected TConfig data;
        
        [Header("Components")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected Rigidbody2D rb2D;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        
        [Inject] protected DetectedPlayerService DetectedPlayerService;

        public override void Initialize()
        {
            
        }
    }
}