using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScript : MonoBehaviour {

	public Sprite[] loadingscreens;
    [SerializeField]
    private Color Off;
    [SerializeField]
    private Color On;
    [SerializeField]
    private Color OnSides;
    [SerializeField]
    private Image[] Children;

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

    public void VisualToggle(bool Switch)
    {
        if(Switch == true)
        {
            //Debug.Log("TrunOn");
            GetComponent<Image>().color = On;
            Children[0].GetComponent<Image>().color = OnSides;
            Children[1].GetComponent<Image>().color = OnSides;
        }
        else
        {
            //Debug.Log("TrunOff");
            GetComponent<Image>().color = Off;
            Children[0].GetComponent<Image>().color = Off;
            Children[1].GetComponent<Image>().color = Off;
        }
    }
}
