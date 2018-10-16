﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonBehaviour : MonoBehaviour {

	public GameObject Pagemanager; 
	private bool isReadyToClick = true;
	float timeLeft = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			
		timeLeft -= Time.deltaTime;
		if ( timeLeft < 0 )
		{
			isReadyToClick = true;
		}
	}

	public void GoToNext()
	{
		if (isReadyToClick == true) {
			isReadyToClick = false;
			timeLeft = 0.5f;
			Pagemanager.GetComponent<PageManager> ().GotoNext ();
		}
	}

	public void GoTolast()
	{
		if (isReadyToClick == true) {
			isReadyToClick = false;
			timeLeft = 0.5f;
			Pagemanager.GetComponent<PageManager> ().GotoPrevious ();
		}
	}
}
