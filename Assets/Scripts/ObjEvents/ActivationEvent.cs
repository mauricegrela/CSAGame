using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationEvent : TweenEvent
{
    public bool activate;

    public override void OnActivate(bool instant)
    {
        gameObject.SetActive(activate);
    }

    public override void OnDeactivate()
    {
        gameObject.SetActive(!activate);
    }

}
