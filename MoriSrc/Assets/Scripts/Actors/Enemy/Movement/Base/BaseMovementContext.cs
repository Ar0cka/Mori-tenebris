using Actors.Enemy.Movement.Base.Service;
using UnityEngine;

namespace Actors.Enemy.Movement.Base
{
    public class BaseMovementContext<TConfig, TFsm>
    {
        public TConfig Config { get; private set; }
        public TFsm Fsm { get; private set; }
        public Rigidbody2D Rb2D { get; private set; }
        public Animator Animator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public DetectedPlayerService DetectedPlayerService { get; private set; }

        public BaseMovementContext(TConfig config, TFsm fsm, Rigidbody2D rb2D, Animator animator, SpriteRenderer spriteRenderer, DetectedPlayerService detectedPlayerService)
        {
            Config = config;
            Fsm = fsm;
            Rb2D = rb2D;
            Animator = animator;
            SpriteRenderer = spriteRenderer;
            DetectedPlayerService = detectedPlayerService;
        }
    }
}