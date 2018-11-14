using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ParentsCornerActivation : MonoBehaviour {


    public string[] BootColNames;
    public Sprite[] BootCols;
    public Image BootImg;
    public InputField InputRef;
    public GameObject[] Locks;
    public Image[] IMGRef;
    public Color Unlocked;
    public GameObject Unlock;
    public GameObject ParentsCorner;

    private string CorrectAnswer;
    private int CorrectAnswerIndex;

	// Use this for initialization
	void Start () {
        //SetNewImage();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetNewImage()
    {
        Debug.Log("Setting New Image");
        CorrectAnswerIndex = Random.Range(0, BootColNames.Length );
        Debug.Log(BootColNames[CorrectAnswerIndex]);
        CorrectAnswer = BootColNames[CorrectAnswerIndex];
        BootImg.sprite = BootCols[CorrectAnswerIndex];

    }


    public void CheckIfAnswerRight(string Answer)
    {
        Debug.Log(InputRef.text.ToLower() +"///////"+ BootColNames[CorrectAnswerIndex]);
        if(InputRef.text.ToLower() == BootColNames[CorrectAnswerIndex])
        {//If the correct answer is input
            gameObject.SetActive(false);
            UnlockGame();
            /*for (int i = 0; i <= Locks.Length - 1; i++)
            {
                Locks[i].SetActive(false);

            }

            for (int i = 0; i <= IMGRef.Length - 1; i++)
            {
                IMGRef[i].color = Unlocked;
            }*/
//            Unlock.GetComponent<MyIAPManager>().BuyNonConsumable();
        }
    }

    public void UnlockGame()
    {
        ParentsCorner.SetActive(true);
        /*gameObject.SetActive(false);
        for (int i = 0; i <= Locks.Length - 1; i++)
        {
            Locks[i].SetActive(false);

        }

        for (int i = 0; i <= IMGRef.Length - 1; i++)
        {
            IMGRef[i].color = Unlocked;
        }*/
        //Unlock.GetComponent<MyIAPManager>().BuyNonConsumable();
    }
    public void redirect()
    {
        Application.OpenURL("http://www.asc-csa.gc.ca/eng/astronauts/canadian/active/bio-david-saint-jacques.asp");
    }
}
