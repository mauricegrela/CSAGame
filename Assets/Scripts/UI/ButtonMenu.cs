﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void redirect()
    {
        Debug.Log(DataManager.currentLanguage);
        if(DataManager.currentLanguage == "english")
        {
            Application.OpenURL("http://asc-csa.gc.ca/eng/missions/expedition58-59");    
        }
            else
            {
            Application.OpenURL("http://asc-csa.gc.ca/fra/missions/expedition58-59");      
            }
            
    }
}