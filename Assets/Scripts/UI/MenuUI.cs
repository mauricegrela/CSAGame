using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening.Plugins.Options;

public class MenuUI : MonoBehaviour
{
    public Dropdown languageDropdown;
    public Dropdown storyDropdown;

    //TODO Dry this code up
    void Awake()
    {
      
    }


    // In this example we show how to invoke a coroutine and
    // continue executing the function in parallel.

    private IEnumerator coroutine;
    private int Counter = 0;

    void Start()
    {

    }

    public void StartGame()
    {
        //Debug.Log("Working");
        SceneManager.LoadScene("MainStory");
    }

  
}
