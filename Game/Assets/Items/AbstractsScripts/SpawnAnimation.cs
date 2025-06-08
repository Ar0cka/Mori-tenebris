using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Player.Inventory
{
    public class SpawnAnimation : MonoBehaviour
    {
        [Header("Jump")] [SerializeField] private float jumpHeight = 1f;
        [SerializeField] private float jumpDuration = 0.5f;
        [Header("Rotate")] [SerializeField] private float rotateDuration = 0.5f;

        private Sequence _mainAnimation;

        public void PlaySpawnAnimation()
        {
            if (transform == null) return;

            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;

            _mainAnimation = DOTween.Sequence()
                .Append(transform.DOJump(transform.position, jumpHeight, 1, jumpDuration)
                    .SetEase(Ease.OutBounce))
                .Join(transform.DORotate(new Vector3(0, 360, 0), rotateDuration, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear))
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        private void OnDisable()
        {
            _mainAnimation.Kill();
        }
    }
}