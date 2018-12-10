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

    public Sprite[] FrenchTransAssetsINI;

    public Image[] AssetsINIRef;
    // In this example we show how to invoke a coroutine and
    // continue executing the function in parallel.

    private IEnumerator coroutine;
    private int Counter = 0;

    void Awake()
    {

        //This checks if your computer's operating system is in the French language
        if (Application.systemLanguage == SystemLanguage.English && DataManager.isINISet == false)
        {
            DataManager.currentLanguage = "english";
            DataManager.isINISet = true;
            //Outputs into console that the system is French
            //Debug.Log("This system is in French. ");
        }
        //Otherwise, if the system is English, output the message in the console
        else if (Application.systemLanguage == SystemLanguage.French && DataManager.isINISet == false)
        {
            DataManager.currentLanguage = "French";
            DataManager.isINISet = true;
            AssetsINIRef[0].sprite = FrenchTransAssetsINI[0];
            AssetsINIRef[1].sprite = FrenchTransAssetsINI[1];
            AssetsINIRef[2].sprite = FrenchTransAssetsINI[2];
            //Debug.Log("This system is in English. ");
        }

        //DataManager.LoadStory(DataManager.currentStoryName, "0");
    }

    public void StartGame(string LeveltoLoad)
    {
        //Debug.Log("Working");
        SceneManager.LoadScene("MainStory");
    }

    public void LoadNewLanguage(string LeveltoLoad)
    {
        DataManager.currentLanguage = LeveltoLoad;
        SceneManager.LoadScene("Menu");
    }


    public void SkyboxTest()
    {
        //Debug.Log("Working");
        SceneManager.LoadScene("Accelerometer_Test");
    }
  
    public void GoToMenu()
    {
        //Debug.Log("Working");
        SceneManager.LoadScene("Menu");
    }
  

}
