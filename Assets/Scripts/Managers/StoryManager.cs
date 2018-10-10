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
    [SerializeField]
    public GameObject Camera;
    public GameObject[] TextPositions;
    public GameObject InitialTextPosition;

    //CoRoutine Loading
    private IEnumerator coroutine;
    private int Counter = 0;

    //Panning Variable
    private bool isPanningLeft = false;
    private bool isPanningRight = false;
    private float PanningCounter;
    private float PanningSpeed = 20.47f;
    private float PanningLimit = 20.47f;
    private float PanningSetUp = 20.47f;

    //
    private Vector3 DistanceCounter;

    void Awake()
    {
        TextPositions = new GameObject[transform.childCount];
        pagesPerScene = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {//Store all the pages
            TextPositions[i] = transform.GetChild(i).gameObject;
            //Debug.Log(transform.GetChild(i).name);
        }
        PageManager = GameObject.FindGameObjectWithTag("PageManager");
        PageManager.GetComponent<PageManager>().sentenceContainerCounter = 0;
        foreach (Transform child in TextPositions[0].transform)
        {//Store the First of the Text References 
            
            if (child.gameObject.tag == "TextPlacement")
            {
                PageManager.GetComponent<PageManager>().
                sentenceContainer[PageManager.GetComponent<PageManager>().sentenceContainerCounter] 
                = child.gameObject.GetComponent<SentenceRowContainer>();
                
                PageManager.GetComponent<PageManager>().sentenceContainerCounter++;

                //InitialTextPosition = child.gameObject;//GameObject.FindWithTag("TextPlacement"); 
                Debug.Log("Working");
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


        /*for (int i = 0; i <= TextPositions.Length - 1; i++)
        {////Set the children to be a carousel 
            TextPositions[i].GetComponent<Transform>().localPosition = new Vector3(
                TextPositions[i].GetComponent<Transform>().localPosition.x+(PanningSetUp*i),
                TextPositions[i].GetComponent<Transform>().localPosition.y,
                TextPositions[i].GetComponent<Transform>().localPosition.z);

        }*/
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

    public void PanRight()
    {
        isPanningRight = true;
    }

    public void PanLeft()
    {
        isPanningLeft = true;
    }

	// Update is called once per frame
	void Update () {
        
        if(isPanningRight == true && isPanningLeft == false)
        {

            foreach (GameObject child in TextPositions)
            {//Store the First of the Text References                 
                child.SetActive(false);
            }
            int CurrentPage = PageManager.GetComponent<PageManager>().sceneindex;
            TextPositions[CurrentPage].SetActive(true);
            isPanningRight = false;
            PageManager.GetComponent<PageManager>().SetUpNewTextFoward();

            /*int CurrentPage = PageManager.GetComponent<PageManager>().sceneindex;
            DistanceCounter = new Vector3(TextPositions[CurrentPage].transform.position.x,Camera.transform.position.y,Camera.transform.position.z);
            if(Vector3.Distance(DistanceCounter, Camera.transform.position)>0) 
            {//Move if you're not in place
                Camera.transform.position = DistanceCounter;//Vector3.MoveTowards(DistanceCounter, Camera.transform.position, 0.01f);
               
            }
                else
                {
                isPanningRight = false;
                PageManager.GetComponent<PageManager>().SetUpNewTextFoward();
                }

            if(PanningCounter*-1 <=PanningLimit)
            {
                for (int i = 0; i <= TextPositions.Length - 1; i++)
                {//Pan the camera Right
                    //Camera.transform.position -= Vector3.right * (Time.deltaTime * PanningSpeed);
                    TextPositions[i].transform.position -= Vector3.right * (Time.deltaTime * PanningSpeed);
                    /// Camera
                }
            PageManager.GetComponent<PageManager>().ScenetextContainer.GetComponent<Transform>().localPosition 
                       -= Vector3.right * (Time.deltaTime * PanningSpeed);
            
                PanningCounter -= Vector3.right.x * (Time.deltaTime * PanningSpeed);
                //Debug.Log(PanningCounter);
            }
            else
            {
                PanningCounter = 0;
                isPanningRight = false;
                PageManager.GetComponent<PageManager>().SetUpNewTextFoward();
            }*/
        
        }


        if (isPanningRight == false && isPanningLeft == true)
        {
            foreach (GameObject child in TextPositions)
            {//Store the First of the Text References                 
                child.SetActive(false);
            }
            int CurrentPage = PageManager.GetComponent<PageManager>().sceneindex;
            TextPositions[CurrentPage].SetActive(true);
            isPanningLeft = false;
            PageManager.GetComponent<PageManager>().SetUpNewTextBack();

            /*if (PanningCounter  <= PanningLimit)
            {
                for (int i = 0; i <= TextPositions.Length - 1; i++)
                {//Pan the camera Right
                    //TextPositions[i].transform.position += Vector3.right * (Time.deltaTime * PanningSpeed);
                }
                PageManager.GetComponent<PageManager>().ScenetextContainer.GetComponent<Transform>().localPosition
                           += Vector3.right * (Time.deltaTime * PanningSpeed);

                PanningCounter += Vector3.right.x * (Time.deltaTime * PanningSpeed);
                Debug.Log(PanningCounter);
            }
            else
            {
                PanningCounter = 0;
                isPanningLeft = false;
                PageManager.GetComponent<PageManager>().SetUpNewTextBack();
            }*/

        }
	}
}
