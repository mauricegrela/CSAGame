using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuChapterManager : MonoBehaviour {

    private bool IsMenuSetUp = false;
	public string[] ButtonDescription;

    private StoryObject currentStory
    {
        get
        {
            return DataManager.currentStory;
        }
    }
    
    public GameObject ButtonTemplate;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReturnToMainMenu()
    {
        Application.LoadLevel("Menu");
    }

        public void ChapterINI()
    {
		//Debug.Log (DataManager.currentStoryName);
		for (int i = 0; i < transform.GetChildCount(); i++) {
			if(transform.GetChild(i).name == ("BookMarks_"+DataManager.currentStoryName))
			{
			transform.GetChild (i).gameObject.SetActive (true);
			}
			//Debug.Log (transform.GetChild(i).name);
		}
      
       /* if (IsMenuSetUp == false)
        {//Set up the Chapter reference in the story
            RectTransform rt = gameObject.GetComponent<RectTransform>();
            float ScrollRoomHeight = 0;
            IsMenuSetUp = true;
            int rowCounter = 0;
            float xCounter = 0;
            float yCounter = 0;
            Vector3 PositionTracker ;
			for (int audio = 0; audio < ButtonDescription.Length; audio++)
            {//Create a button for each page. 
                GameObject LanguageButton = Instantiate(ButtonTemplate) as GameObject;
                LanguageButton.transform.SetParent(gameObject.transform, false);
                PositionTracker = new Vector3(LanguageButton.transform.position.x + xCounter, LanguageButton.transform.position.y - yCounter, LanguageButton.transform.position.z);
                LanguageButton.transform.position = PositionTracker;
				LanguageButton.GetComponentInChildren<Text> ().text = ButtonDescription [audio];
				//Create Activity    
                //LanguageButton.GetComponent<Button>().onClick.AddListener(() => gameObject.GetComponent<gotopage>());
                //LanguageButton.GetComponent<gotopage>().setPageTarget(audio+1);


            }
            ButtonTemplate.SetActive(false);
        }
            else
            {//Set up which of the chapters is sellected. 

            }*/
                
    }
}
