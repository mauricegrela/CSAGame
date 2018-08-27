using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Bit of a hack. Every event inherit from TweenEvent, but they don't all Tween.
public class TweenEvent : DOTweenAnimation
{

    [HideInInspector]
    public string pageName;
    [HideInInspector]
    public string audioName;

    protected virtual void Awake()
    {
        // Registring is now done though PageManager Awake
        // PageManager.instance.Register(this);
        enabled = false;
    }

    public virtual void OnNextStep()
    {
        
    }

    /// <summary>
    /// This method should be overriden without calling the base-method. 
    /// </summary>
    public virtual void OnActivate(bool instant = false)
    {
        DOPlayForward();
        if (instant)
        {
            DOTween.Complete(id, true);
        }
    }

    /// <summary>
    /// This method should be overriden without calling the base-method. 
    /// </summary>
    public virtual void OnDeactivate()
    {
        DORewind();
    }
}
