using DG.Tweening;
using UnityEngine;

namespace Player.Inventory
{
    public class SpawnAnimation : MonoBehaviour
    {
        [Header("Jump")]
        [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float jumpDuration = 0.5f;
        [Header("Rotate")]
        [SerializeField] private float rotateDuration = 0.5f;

        private Sequence _mainAnimation;

        public void PlaySpawnAnimation()
        {
            _mainAnimation?.Kill();
            
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;

            _mainAnimation = DOTween.Sequence()
                .Append(transform.DOJump(transform.position, jumpHeight, 1, jumpDuration)
                    .SetEase(Ease.OutBounce))
                .Join(transform.DORotate(new Vector3(0, 360, 0), rotateDuration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear));
        }
        

        private void OnDestroy()
        {
            _mainAnimation?.Kill();
        }
    }
}