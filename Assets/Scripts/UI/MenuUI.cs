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

	public GameObject English_Button;
	public GameObject French_Button;

    void Awake()
    {

        //This checks if your computer's operating system is in the French language
        if (Application.systemLanguage == SystemLanguage.English && DataManager.isINISet == false)
        {
            DataManager.currentLanguage = "english";
            DataManager.isINISet = true;
        }
        //Otherwise, if the system is English, output the message in the console
        else if (Application.systemLanguage == SystemLanguage.French && DataManager.isINISet == false)
        {
			Vector3 English_Button_Pos = English_Button.transform.position;
			Vector3 French_Button_Pos = French_Button.transform.position;
			English_Button.transform.position = French_Button_Pos;
			French_Button.transform.position = English_Button_Pos;
            DataManager.currentLanguage = "French";
            DataManager.isINISet = true;
            AssetsINIRef[0].sprite = FrenchTransAssetsINI[0];
            AssetsINIRef[1].sprite = FrenchTransAssetsINI[1];
        }
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
