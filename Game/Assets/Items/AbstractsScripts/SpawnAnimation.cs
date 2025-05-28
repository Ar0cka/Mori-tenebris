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
        [SerializeField] private float rotateDuration = 0.7f;
        [Header("Scale")]
        [SerializeField] private float minScale = 0.9f;
        [SerializeField] private float maxScale = 1.1f;
        [SerializeField] private float scalePulseDuration = 0.5f;

        private Sequence _mainAnimation;
        private Sequence _pulseSequence;

        public void PlaySpawnAnimation()
        {
            _mainAnimation?.Kill();
            _pulseSequence?.Kill();
            
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
            
            _mainAnimation = DOTween.Sequence()
                .Append(transform.DOJump(transform.position, jumpHeight, 1, jumpDuration)
                    .SetEase(Ease.OutQuad))
                .Join(transform.DORotate(new Vector3(0, 360, 0), rotateDuration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .OnComplete(StartPulseAnimation); 
        }

        private void StartPulseAnimation()
        {
            _pulseSequence = DOTween.Sequence()
                .Append(transform.DOScale(maxScale, scalePulseDuration)
                    .SetEase(Ease.InOutSine))
                .Append(transform.DOScale(minScale, scalePulseDuration)
                    .SetEase(Ease.InOutSine))
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            _mainAnimation?.Kill();
            _pulseSequence?.Kill();
        }
    }
}