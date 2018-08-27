using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour {

	public Sprite[] loadingscreens;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadingScreenAssigner()
	{

		GetComponent<Image>().sprite = loadingscreens[Random.Range(0,loadingscreens.Length-1)];
	}
}
