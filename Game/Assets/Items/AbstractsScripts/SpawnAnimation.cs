using DG.Tweening;
using UnityEngine;

namespace Player.Inventory
{
    public class SpawnAnimation : MonoBehaviour
    {
        [SerializeField] private float jumpHeight;
        [SerializeField] private float jumpDuration;
        [SerializeField] private float rotateDuration;
        
        public void SpawnAnimationStarted()
        {
            var anim = DOTween.Sequence();
            
            anim.Append(transform.DOJump(transform.position, 
                jumpHeight, 1, 
                jumpDuration).SetEase(Ease.OutBounce));

            anim.Join(transform.DORotate(new Vector2(0, 360), rotateDuration, RotateMode.FastBeyond360));
        }
    }
}