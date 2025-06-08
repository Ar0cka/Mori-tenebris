using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LableAnimationScale : MonoBehaviour
{
    [SerializeField] private float maxScale;
    [SerializeField] private float scaleDuration;
    private void Start()
    {
        transform.DOScale(maxScale, scaleDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}
