using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.SceneManagement;
using DG.DOTweenEditor;
using System;

[CustomEditor(typeof(TweenEvent), true)]
public class TweenEventEditor : DOTweenAnimationInspector
{

    private static StoryObject story;

    public override void OnInspectorGUI()
    {
        
        TweenEvent baseEvent = (TweenEvent)target;


        if (StoryNameManager.instance == null)
        {
            GUILayout.Label("No StoryNameManager Singleton found in scene");
            return;
        }
        if (story == null)
        {
            story = DataManager.LoadStory(StoryNameManager.instance.storyName,"0");
        }

        if (story == null)
        {
            Debug.LogErrorFormat("Story with the name {0} could not be loaded!", DataManager.currentStoryName);
            return;
        }


        //We get the index based on the name
        int index = 0;
        for (int i = 0; i < story.pageObjects.Count; i++)
        {
            if (baseEvent.pageName == story.pageObjects[i].name)
            {
                index = i;
                break;
            }
        }

        string[] pageNames = story.GetPageNames();
        index = EditorGUILayout.Popup("Page", index, pageNames); 

        baseEvent.pageName = pageNames[index];

        PageObject page = story.GetPage(baseEvent.pageName);

        //We get the index based on the name
        index = 0;
        for (int i = 0; i < page.audioObjects.Count; i++)
        {
            if (baseEvent.audioName == page.audioObjects[i].name)
            {
                index = i;
                break;
            }
        }

        string[] audioNames = page.GetAudioNames();
        index = EditorGUILayout.Popup("Audio", index, audioNames); 

        baseEvent.audioName = audioNames[index];

        //This is quite hacky. Every event is a DOTweenAnimation, but things like ActivationEvents simple don't show that.
        if (baseEvent.GetType() == typeof(TweenEvent))
        {
            base.OnInspectorGUI();
        }

    }
}
