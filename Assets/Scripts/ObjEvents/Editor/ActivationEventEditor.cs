using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActivationEvent), true)]
public class ActivationEventEditor : TweenEventEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ActivationEvent cast = (ActivationEvent)target;
        cast.activate = EditorGUILayout.Toggle("Activate", cast.activate);
    }

}
