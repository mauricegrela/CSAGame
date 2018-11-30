using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAssetTranslationSwap : MonoBehaviour {


    [SerializeField]
    private Sprite fr_asset;
    [SerializeField]
    private Sprite en_asset;
	// Use this for initialization
	void Start () {
		
        if (DataManager.currentLanguage == "english")
        {
            GetComponent<Image>().sprite = fr_asset;
            //DataManager.currentLanguage = "english";
            //Outputs into console that the system is French
            //Debug.Log("This system is in French. ");
        }
        //Otherwise, if the system is English, output the message in the console
        else if (DataManager.currentLanguage == "French")
        {
            //.currentLanguage = "French";
            //Debug.Log("This system is in English. ");
            GetComponent<Image>().sprite = en_asset;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FlipSwitch ()
    {
        if (DataManager.currentLanguage == "english")
        {
            GetComponent<Image>().sprite = fr_asset;
            //DataManager.currentLanguage = "english";
            //Outputs into console that the system is French
            //Debug.Log("This system is in French. ");
        }
        //Otherwise, if the system is English, output the message in the console
        else if (DataManager.currentLanguage == "French")
        {
            //.currentLanguage = "French";
            //Debug.Log("This system is in English. ");
            GetComponent<Image>().sprite = en_asset;
        }
    }
}
