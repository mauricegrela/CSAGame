using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformMoveEvent : TweenEvent
{
    public Transform targetObj;
    public float tweenDuration;

    Tweener moveTween;
    Tweener rotateTween;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnNextStep()
    {
        
    }

    public override void OnActivate(bool instant)
    {
        moveTween = targetObj.DOMove(transform.position, tweenDuration);
        rotateTween = targetObj.DORotate(transform.eulerAngles, tweenDuration);
        if (instant)
        {
            targetObj.DOComplete(true);
        }
    }

    public override void OnDeactivate()
    {
        moveTween.Rewind(false);
        rotateTween.Rewind(false);
    }
        
   
}
