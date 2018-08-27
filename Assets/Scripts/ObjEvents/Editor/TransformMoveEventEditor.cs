using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(TransformMoveEvent), true)]
public class TransformMoveEventEditor : TweenEventEditor
{
    Camera targetCam;
    Camera previewCam;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TransformMoveEvent cast = (TransformMoveEvent)target;
        cast.tweenDuration = EditorGUILayout.FloatField("Duration", cast.tweenDuration);
        cast.targetObj = (Transform)EditorGUILayout.ObjectField("Target", cast.targetObj, typeof(Transform), true);

        targetCam = cast.targetObj.GetComponent<Camera>();

        if (previewCam == null)
            previewCam = cast.GetComponent<Camera>();

        if (previewCam != null)
        {
            if (GUILayout.Button("Disable Cam Preview"))
            {
                DestroyImmediate(previewCam);
            }
        }

        if (targetCam != null)
        {
            if (previewCam == null)
            {
                if (GUILayout.Button("Enable Cam Preview"))
                {
                    previewCam = cast.gameObject.AddComponent<Camera>();
                    ComponentUtility.MoveComponentUp(previewCam);
                    UpdateCamFromMain();
                }
            }
        }
    }

    void UpdateCamFromMain ()
    {
        if (targetCam != null)
        {
            if (ComponentUtility.CopyComponent(targetCam))
            {
                ComponentUtility.PasteComponentValues(previewCam);
            }
        } 
    }
}
