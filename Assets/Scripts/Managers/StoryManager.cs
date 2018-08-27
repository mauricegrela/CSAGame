using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour {

	public int AudioIndexPosition; 
	public string LevelName;
	public int pagesPerScene;
	public string NextScene;
	public string LastScene;
	public bool isLastscene;
	public bool isFirstscene;
	public bool isLoadingLevel;
	public int StreamingAssetsCounter;
	public GameObject PageManager;
	private GameObject Canvas;
    //CoRoutine Loading

    private IEnumerator coroutine;
    private int Counter = 0;

    void Awake()
    {
        //Set references 
        PageManager = GameObject.FindGameObjectWithTag("PageManager");
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        PageManager.GetComponent<PageManager>().LoadingScreen.GetComponent<Image>().enabled = true;
        if (isLoadingLevel == true)
        {//If this is going to load a different streaming package, load it here. 

            PageManager.GetComponent<PageManager>().audioIndex = 0;
            DataManager.LoadStory(DataManager.currentStoryName, StreamingAssetsCounter.ToString());
        }
        else if (StreamingAssetsCounter.ToString() != DataManager.CurrentAssetPackage.ToString())
        {//this condition will trigger when the player loads a level from the menu that requires a streaming asset that is not currently loaded.
            DataManager.LoadStory(DataManager.currentStoryName, StreamingAssetsCounter.ToString());
        }
    }

	// Use this for initialization
	void Start () {



        //Debug.Log(StreamingAssetsCounter.ToString() + "////" + DataManager.CurrentAssetPackage.ToString());


		//This variable loads the offset from the page manager so that the level starts off at the right passage.
		int chapterOffset = PageManager.GetComponent<PageManager> ().ChapterOffSet;

		for(int i = 0; i < Canvas.transform.GetChildCount(); i++)
		{//This loop generates the book mark dots 
			if (Canvas.transform.GetChild (i).name == "UI Dots") {
				Canvas.transform.GetChild (i).GetComponent<DotGenerator> ().GenrateTheDots (pagesPerScene);
			}
		}

		if (PageManager.GetComponent<PageManager> ().isGoingBack == false) {
			//Set up the narrative variables
			/*PageManager.GetComponent<PageManager> ().AssetAssigner (LevelName, AudioIndexPosition+chapterOffset);
			PageManager.GetComponent<PageManager> ().GoToPage (AudioIndexPosition+chapterOffset);
			PageManager.GetComponent<PageManager> ().ChapterskipSetCharacters(chapterOffset);*/
            coroutine = WaitGoingForward(0.05f);
            StartCoroutine(coroutine);
			} 
				else 
				{//If the player is going backwards
				/*PageManager.GetComponent<PageManager>().AssetAssigner (LevelName,pagesPerScene-1);
                PageManager.GetComponent<PageManager>().SetToLastPosition();
                PageManager.GetComponent<PageManager>().GetComponent<PageManager>().GoToPage(pagesPerScene - 1);
				PageManager.GetComponent<PageManager>().isGoingBack = false;*/

                coroutine = WaitGoingBack(0.05f);
                StartCoroutine(coroutine);
				}
	//PageManager.GetComponent<PageManager> ().ChapterOffSet = 0;
	//PageManager.GetComponent<PageManager> ().LoadingScreen.GetComponent<Image> ().enabled = false;
	}

    private IEnumerator WaitGoingBack(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (Counter == 0)
            {
                PageManager.GetComponent<PageManager>().AssetAssigner(LevelName, pagesPerScene - 1);
            }
            else if (Counter == 1)
            {
                PageManager.GetComponent<PageManager>().SetToLastPosition();
            }
            else if (Counter == 2)
            {
                PageManager.GetComponent<PageManager>().GetComponent<PageManager>().GoToPage(pagesPerScene - 1);
                PageManager.GetComponent<PageManager>().isGoingBack = false;
                PageManager.GetComponent<PageManager>().LoadingScreen.GetComponent<Image>().enabled = false;
                StopCoroutine(coroutine);
            }
            else
            {

                print("error");
            }
            Counter++;
        }
    }

    private IEnumerator WaitGoingForward(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (Counter == 0)
            {
                PageManager.GetComponent<PageManager> ().AssetAssigner (LevelName, AudioIndexPosition);
            }
            else if (Counter == 1)
            {
                PageManager.GetComponent<PageManager>().GoToPage(AudioIndexPosition );
            }
            else if (Counter == 2)
            {
                PageManager.GetComponent<PageManager>().ChapterskipSetCharacters(0);
                PageManager.GetComponent<PageManager>().LoadingScreen.GetComponent<Image>().enabled = false;
                StopCoroutine(coroutine);
            }
            else
            {

                print("error");
            }
            Counter++;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
