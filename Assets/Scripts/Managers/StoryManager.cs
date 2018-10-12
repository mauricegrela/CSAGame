﻿using System.Collections;
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
    [SerializeField]
    public GameObject Camera;
    public Vector3 OGCameraPosition;
    public GameObject[] TextPositions;
    public GameObject InitialTextPosition;

    //CoRoutine Loading
    private IEnumerator coroutine;
    private int Counter = 0;

    //Panning Variable
    private bool isPanningLeft = false;
    private bool isPanningRight = false;
    private float PanningCounter;
    private Transform panningtargetPosition;
    private float PanningSpeed = 20.47f;
    private float PanningLimit = 20.47f;
    private float PanningSetUp = 20.47f;
    private int CurrentPage;
    //
    private Vector3 DistanceCounter;

    void Awake()
    {
        OGCameraPosition = Camera.transform.position;
        TextPositions = new GameObject[transform.childCount];
        pagesPerScene = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {//Store all the pages
            TextPositions[i] = transform.GetChild(i).gameObject;
            //Debug.Log(transform.GetChild(i).name);
        }
        PageManager = GameObject.FindGameObjectWithTag("PageManager");

        PageManager.GetComponent<PageManager>().sentenceContainerCounter = 0;
        PageManager.GetComponent<PageManager>().sentenceContainerCurrent = 0;

        foreach (GameObject child in TextPositions)
        {//Store the First of the Text References                 
            child.SetActive(false);
        }

        int position;

        if(PageManager.GetComponent<PageManager>().isGoingBack == true)
        {
        TextPositions[TextPositions.Length - 1].SetActive(true);
        position = TextPositions.Length - 1;
        }
            else
            {
            TextPositions[0].SetActive(true);
            position = 0;
            }

        foreach (Transform child in TextPositions[position].transform)
        {//Store the First of the Text References 
            
            if (child.gameObject.tag == "TextPlacement")
            {
                PageManager.GetComponent<PageManager>().
                sentenceContainer[PageManager.GetComponent<PageManager>().sentenceContainerCounter] 
                = child.gameObject.GetComponent<SentenceRowContainer>();
                
                PageManager.GetComponent<PageManager>().sentenceContainerCounter++;

                //InitialTextPosition = child.gameObject;//GameObject.FindWithTag("TextPlacement"); 
                //Debug.Log("Working");
            }


        }


        //Set references 
       
        //PageManager.GetComponent<PageManager>().sentenceContainer[0] = InitialTextPosition.GetComponent<SentenceRowContainer>();

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
        //This variable loads the offset from the page manager so that the level starts off at the right passage.
		int chapterOffset = PageManager.GetComponent<PageManager> ().ChapterOffSet;

		for(int i = 0; i < Canvas.transform.GetChildCount(); i++)
		{//This loop generates the book mark dots 
			if (Canvas.transform.GetChild (i).name == "UI Dots") {
				Canvas.transform.GetChild (i).GetComponent<DotGenerator> ().GenrateTheDots (pagesPerScene);
			}
		}

		if (PageManager.GetComponent<PageManager> ().isGoingBack == false) {
            coroutine = WaitGoingForward(0.05f);
            StartCoroutine(coroutine);
			} 
				else 
				{//If the player is going backwards
                coroutine = WaitGoingBack(0.05f);
                StartCoroutine(coroutine);
				}
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
                PageManager.GetComponent<PageManager>().GetComponent<PageManager>().GoToPage(AudioIndexPosition+pagesPerScene - 1);
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

    public void PanRight()
    {
        isPanningRight = true;
        CurrentPage = PageManager.GetComponent<PageManager>().sceneindex;
        Camera.transform.position = OGCameraPosition;
        if (TextPositions[CurrentPage].tag == "panning")
        {
            Debug.Log("This is a specific panning script.");
            TextPositions[CurrentPage].SetActive(true);
            foreach (Transform child in TextPositions[CurrentPage].transform)
            {//Store the First of the Text References 

                if (child.gameObject.tag == "panning target")
                {
                    panningtargetPosition = child.gameObject.transform;
                }

            }
        }
            else
            {
                foreach (GameObject child in TextPositions)
                {//Store the First of the Text References                 
                    child.SetActive(false);
                }

                TextPositions[CurrentPage].SetActive(true);

                isPanningRight = false;
                PageManager.GetComponent<PageManager>().SetUpNewTextFoward(); 
            }
    }

    public void PanLeft()
    {
        isPanningLeft = true;
    }

	// Update is called once per frame
	void Update () {
        
        if(isPanningRight == true && isPanningLeft == false)
        {
            float dist = Vector3.Distance(Camera.transform.position, panningtargetPosition.position);
            if (dist >= 0.1)
            {
                //Camera.transform.Translate(Vector3.right * (Time.deltaTime*2), Space.World);
                float step = 6 * Time.deltaTime;

                // Move our position a step closer to the target.
                Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, panningtargetPosition.position, step);
            }
                else
                {
                    foreach (GameObject child in TextPositions)
                    {//Store the First of the Text References                 
                        child.SetActive(false);
                    }

                TextPositions[CurrentPage].SetActive(true);

                isPanningRight = false;
                PageManager.GetComponent<PageManager>().SetUpNewTextFoward();  
                }
        }


        if (isPanningRight == false && isPanningLeft == true)
        {
            foreach (GameObject child in TextPositions)
            {//Store the First of the Text References                 
                child.SetActive(false);
            }
            CurrentPage = PageManager.GetComponent<PageManager>().sceneindex;

            TextPositions[CurrentPage].SetActive(true);
            isPanningLeft = false;
            PageManager.GetComponent<PageManager>().SetUpNewTextBack();

        }
	}

    public void SetToFinal()
    {
        Debug.Log("SET TO FINAL");
        foreach (GameObject child in TextPositions)
        {//Store the First of the Text References                 
            child.SetActive(false);
        }
        int CurrentPage = PageManager.GetComponent<PageManager>().sceneindex;
        TextPositions[CurrentPage].SetActive(true);  
    }
}
