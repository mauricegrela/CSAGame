using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PageManager : Singleton<PageManager>
{
    //public static event Action<string,string> onSentenceChange;

    private StoryObject currentStory
    {
        get
        {
            return DataManager.currentStory;
            Debug.Log(currentStory);
        }
    }

    private PageObject currentPage
    {
        get
        {
            return currentStory.pageObjects[pageIndex];
        }
    }
    //PageKeepers
    private int pageIndex;
    public int audioIndex;
    public int sceneindex;
    /// <summary>/// /////TURN THIS PRIVATE/// </summary>
    private int LastPageLoader;
    public bool isGoingBack = false;
    public int ChapterOffSet = 0;

    //Technicals
    private AudioSource audioSource;
    [SerializeField]
    public SentenceRowContainer[] sentenceContainer;
    public int sentenceContainerCounter;
    private int sentenceContainerCurrent=0;
    private bool isForward = true;
    private List<TweenEvent> tweenEvents = new List<TweenEvent>();
    [SerializeField]
    private GameObject[] Characters;
    [SerializeField]
    private GameObject[] DynamicProps;

    //Narrative Manager vars
    public GameObject StoryManager;
    private string EnvironmentTracker;
    public GameObject TextBody;
    private Vector3 OG_PostitionTextBody;

    //Camera Variables
    private Vector3 cameraPreviousPosition;
    public Transform cameraTransformTracker;
    private bool isCamMoving = false;

    //Menu Variables
    public bool isMenuDeployed = false;

    //Mouse Tracking Variabels
    private bool CanSwipe = true;
    private Vector2 mouseStartPosition;
    private float mouseDistance;
    private float mouseoffset;
    private bool isMouseMoving;
    private Vector2 mouseEndPosition;
    private GameObject MountainTest;
    private GameObject CharacterCoin;
    private string Speaker;
    private int narrativeCounter = 1;

    //UI Assets
    public GameObject UIDots;
    public GameObject ScentenceContainer;
    public Image LoadingScreen;

    //Debuging Vars
    public GameObject Scenetext;

    private GameObject TextPositionref;

    [SerializeField]
    public GameObject ScenetextContainer;

    protected override void Awake()
    {
        base.Awake();
        //sentenceContainer = FindObjectOfType<SentenceRowContainer>();
        audioSource = GetComponent<AudioSource>();
    }


    // Use this for initialization
    IEnumerator Start()
    {//Initiage the story
     //AssetAssigner (DataManager.currentStoryName + "_start", 11);
        //DataManager.LoadStory(DataManager.currentStoryName, "0");
        OG_PostitionTextBody = TextBody.gameObject.transform.position;
        yield return null;
    }


    public void AssetAssigner(string CurrentLevel, int lastPage)
    {//when ever a level is loaded, this code will run to store all of the relative data
        Resources.UnloadUnusedAssets();
        EnvironmentTracker = CurrentLevel;
        StoryManager = GameObject.FindGameObjectWithTag("StoryManager");//Find the story manager found in every level

        if (isGoingBack == true)
        {
            sceneindex = lastPage;
            Debug.Log(sceneindex);
  
        }
    }

    public void ChapterskipSetCharacters(int StartingPosition)
    {
        foreach (GameObject Child in Characters)
        {//Play the next animation on all the characters
            if (Child.GetComponent<Animator>() != null || Child.GetComponent<Camera>() != null || Child.GetComponent<Image>() != null)
            {
                Child.GetComponent<CharacterAnimationSystems>().setUpCharacters(StartingPosition);
            }
        }
    }

    public void ChapterSkip(String LevelToLoad)
    {//Launches when the player skips to a chapter through clicking on the book mark
        StopAllCoroutines();

        foreach (SentenceRowContainer Child in sentenceContainer)
        {
            if(Child != null)
            Child.Clear();
        }

        LoadingScreen.GetComponent<LoadingScript>().LoadingScreenAssigner();
        LoadingScreen.GetComponent<Image>().enabled = true;
        Resources.UnloadUnusedAssets();
        SceneManager.UnloadScene(EnvironmentTracker);
        AssetBundle.UnloadAllAssetBundles(true);
        SceneManager.LoadScene(LevelToLoad, LoadSceneMode.Additive);

    }

    public void ChapterSkipToTheEnd(String LevelToLoad)
    {

        Resources.UnloadUnusedAssets();
        SceneManager.UnloadSceneAsync(EnvironmentTracker);
        SceneManager.LoadScene(LevelToLoad, LoadSceneMode.Additive);

    }


    void Update()
    {

    }


    public void GotoNext()
    {




        sceneindex++;
        bool isloadingScene;

        string NextScene;
        NextScene = StoryManager.GetComponent<StoryManager>().NextScene;

        if (sceneindex >= StoryManager.GetComponent<StoryManager>().pagesPerScene)
        {//If the player is at the last page of the scene
            isloadingScene = true;
            //Debug.Log(sceneindex+"///"+StoryManager.GetComponent<StoryManager>().pagesPerScene);
            GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
            LoadingScreen.GetComponent<Image>().enabled = true;
            Resources.UnloadUnusedAssets();
            SceneManager.UnloadScene(EnvironmentTracker);

            //Check if the player has reached the end of this scene, Once reached, go to the next scene.
            SceneManager.LoadScene(NextScene, LoadSceneMode.Additive);
            isGoingBack = false;
            sceneindex = 0;
            LoadingScreen.GetComponent<Image>().enabled = false;

            sentenceContainerCounter = 0;
            sentenceContainerCurrent = 0;
            //GameObject TextPositionref;
            foreach (Transform child in StoryManager.GetComponent<StoryManager>().TextPositions[sceneindex].transform)
            {
                // do whatever you want with child transform object here
                if (child.gameObject.tag == "TextPlacement")
                {
                    sentenceContainer[sentenceContainerCounter] = child.gameObject.GetComponent<SentenceRowContainer>(); ;
                    sentenceContainerCounter++;
                    TextPositionref = child.gameObject;//GameObject.FindWithTag("TextPlacement"); 
                    Debug.Log("Working");
                }

                if (child.gameObject.tag == "SpeechBubble")
                {
                    child.gameObject.GetComponent<SpeechBubbleAnimation>().setActive();
                    Debug.Log("working");
                    //TextPositionref = child.gameObject;//GameObject.FindWithTag("TextPlacement");    
                }

            }

            //Scenetext.GetComponent<Text>().text = currentPage.audioObjects[audioIndex];
        }
        else
        {// is the scene has a pages it can show.
           
        }
        /*if (isloadingScene == false)
        {
            if (sceneindex == StoryManager.GetComponent<StoryManager>().pagesPerScene - 1
                && StoryManager.GetComponent<StoryManager>().isLastscene == true)
            {//If this is the last scene in the story
             //Canvas.GetComponent<MainStoryScreen>().OnQuitButton();
                GameObject EndindCard = GameObject.FindGameObjectWithTag("EndingCard");
                EndindCard.GetComponentInChildren<FadeScript>().enabled = true;
                EndindCard.GetComponentInChildren<Image>().raycastTarget = true;
            }
        }*/
        //CharacterCoin.GetComponent<SpeakerUIAssign>().ImageAssign(Speaker);

        //sceneindex >= StoryManager.GetComponent<StoryManager>().pagesPerScene


        StoryManager.GetComponent<StoryManager>().PanRight();
    }

    public void SetUpNewTextFoward()
    {
        //isloadingScene = false;
       
        isGoingBack = false;
        isForward = true;
        transform.hasChanged = false;
        foreach (GameObject Mesh in DynamicProps)
        {//Go through all the dynamic meshes and see if there are any that need to be moved or activated. 
            Mesh.GetComponent<DynamicStaticMeshSystem>().MoveMeshForward();
        }

        foreach (GameObject Child in Characters)
        {//Play the next animation on all the characters
            if (Child.GetComponent<Animator>() != null || Child.GetComponent<Camera>() != null || Child.GetComponent<Image>() != null)
            {
                Child.GetComponent<CharacterAnimationSystems>().InvokeNextAnimation(Scenetext.GetComponent<Text>().text);
            }
        }
        UIDots.GetComponent<DotGenerator>().updateDots(sceneindex);

        //sentenceContainer.gameObject.SetActive(false);


        foreach (SentenceRowContainer Child in sentenceContainer)
        {
            if (Child != null)
            Child.gameObject.SetActive(false);
        }

        for (int i = 0; i < sentenceContainer.Length; i++)
        {
            sentenceContainer[i] = null;
        }

        //sentenceContainer.Clear();
        sentenceContainerCounter = 0;
        sentenceContainerCurrent = 0;
        //GameObject TextPositionref;
        foreach (Transform child in StoryManager.GetComponent<StoryManager>().TextPositions[sceneindex].transform)
        {
            // do whatever you want with child transform object here
            if(child.gameObject.tag == "TextPlacement")
            {
                sentenceContainer[sentenceContainerCounter] = child.gameObject.GetComponent<SentenceRowContainer>();;
                sentenceContainerCounter++;
                TextPositionref = child.gameObject;//GameObject.FindWithTag("TextPlacement"); 
                Debug.Log("Working");
            }

            if (child.gameObject.tag == "SpeechBubble")
            {
                child.gameObject.GetComponent<SpeechBubbleAnimation>().setActive();
                Debug.Log("working");
                //TextPositionref = child.gameObject;//GameObject.FindWithTag("TextPlacement");    
            }

        }

        //ScenetextContainer.GetComponent<RectTransform>().position = TextPositionref.GetComponent<RectTransform>().position;
        //sentenceContainer = TextPositionref.GetComponent<SentenceRowContainer>();
        NextSentence(isForward);
    }

    public void GotoPrevious()
    {
        sceneindex--;
        Debug.Log(sceneindex);
        if (sceneindex < 0)
        {// if the player has reached the end of a bookmark. 
            if (StoryManager.GetComponent<StoryManager>().isFirstscene == true)
            {//This condition handels what the player can do when they playe goes backwards from the first passage
                GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
                Canvas.GetComponent<MainStoryScreen>().OnQuitButton();
            }
            else
            {//otherwise, go to the last scene
                isGoingBack = true;
                ChapterSkipToTheEnd(StoryManager.GetComponent<StoryManager>().LastScene);

                Debug.Log("Loading a new Level");
            }
        }
        else
        {//If the player is still working their way backwards through the scene.
            //isForward = false;
            //PreviousSentence(isGoingBack);
            /*GetComponent<PageManager>().GoToPage(audioIndex-1);
            transform.hasChanged = false;
            foreach (GameObject Mesh in DynamicProps)
            {//Go through all the dynamic meshes and see if there are any that need to be moved or activated. 
                Mesh.GetComponent<DynamicStaticMeshSystem>().MoveMeshBackward();
            }

            foreach (GameObject Child in Characters)
            {//Play the next animation on all the characters
                if (Child.GetComponent<Animator>() != null || Child.GetComponent<Camera>() != null || Child.GetComponent<Image>() != null)
                {
                    //Debug.Log("Launching Previous Anim");
                    Child.GetComponent<CharacterAnimationSystems>().InvokePreviousAnimation(Scenetext.GetComponent<Text>().text);
                }
            }
            UIDots.GetComponent<DotGenerator>().updateDots(sceneindex);

            /*if(isGoingBack == true)
            {
                isGoingBack = false; 

            }*/
        StoryManager.GetComponent<StoryManager>().PanLeft();
        }
        ///CharacterCoin.GetComponent<SpeakerUIAssign>().ImageAssign(Speaker);
    }

    public void SetUpNewTextBack()
    {
        //PreviousSentence(isGoingBack);
        GetComponent<PageManager>().GoToPage(audioIndex - 1);
        transform.hasChanged = false;
        foreach (GameObject Mesh in DynamicProps)
        {//Go through all the dynamic meshes and see if there are any that need to be moved or activated. 
            Mesh.GetComponent<DynamicStaticMeshSystem>().MoveMeshBackward();
        }

        foreach (GameObject Child in Characters)
        {//Play the next animation on all the characters
            if (Child.GetComponent<Animator>() != null || Child.GetComponent<Camera>() != null || Child.GetComponent<Image>() != null)
            {
                //Debug.Log("Launching Previous Anim");
                Child.GetComponent<CharacterAnimationSystems>().InvokePreviousAnimation(Scenetext.GetComponent<Text>().text);
            }
        }
        UIDots.GetComponent<DotGenerator>().updateDots(sceneindex);

        //GameObject TextPositionref;
        foreach (Transform child in StoryManager.GetComponent<StoryManager>().TextPositions[sceneindex].transform)
        {
            // do whatever you want with child transform object here
            if (child.gameObject.tag == "TextPlacement")
            {
                TextPositionref = child.gameObject;//GameObject.FindWithTag("TextPlacement");    

            }
        }

        ScenetextContainer.GetComponent<RectTransform>().position = TextPositionref.GetComponent<RectTransform>().position;

    }

    public void SetToLastPosition()
    {//This function goes through all the dynamic instances in the scene and sets their location to the last memeber of the array
        Characters = GameObject.FindGameObjectsWithTag("Characters");
        foreach (GameObject Child in Characters)
        {
            Child.GetComponent<CharacterAnimationSystems>().ResetToTheEnd();
        }

        DynamicProps = GameObject.FindGameObjectsWithTag("DynamicProps");
        foreach (GameObject Mesh in DynamicProps)
        {
            Mesh.GetComponent<DynamicStaticMeshSystem>().ResetToTheEnd();
        }
        UIDots.GetComponent<DotGenerator>().updateDots(sceneindex);
    }

    private void LanguageMenuDeploy()
    {
        GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
        GameObject text = GameObject.FindGameObjectWithTag("DebugText");
        //Canvas Positions
        Vector3 OGPosition = text.transform.position;
        Vector3 Position = text.transform.position;
        for (int languageCount = 0; languageCount < DataManager.languageManager.Length; languageCount++)
        {
            GameObject LanguageButton = Instantiate(text) as GameObject;
            //Attributes
            LanguageButton.GetComponent<Button>().GetComponentInChildren<Text>().text = DataManager.languageManager[languageCount];
            LanguageButton.GetComponent<Button>().GetComponent<Image>().color = Color.blue;
            LanguageButton.transform.SetParent(Canvas.transform, false);
            Position = new Vector3(Position.x + 100, Position.y, Position.z);
            LanguageButton.transform.position = Position;
            LanguageButton.GetComponent<Button>().onClick.AddListener(() => OnUIButtonClick_Language(LanguageButton.GetComponent<Button>()));
        }
    }

    private void OnUIButtonClick_Language(Button button)
    {//when the player clicks a button
     //Debug.Log("OnUIButtonClick_Language: " + button.gameObject.GetComponentInChildren<Text>().text);
        ChangeLanguage(button.gameObject.GetComponentInChildren<Text>().text);
    }

    private void OnUIButtonClick_Menu(Button button)
    {//when the player clicks a button
        Debug.Log("OnUIButtonClick_Menu: " + button.gameObject.GetComponentInChildren<Text>().text);
        int Page = int.Parse(button.gameObject.GetComponentInChildren<Text>().text);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
    }

    public void ChangeLanguage(string newLanguage)
    {
        StopAllCoroutines();
        foreach (SentenceRowContainer Child in sentenceContainer)
        {
            if (Child != null)
            Child.Clear();
        }
        //Debug.Log(newLanguage);

        DataManager.currentLanguage = newLanguage;
        DataManager.LoadStory(DataManager.currentStoryName, DataManager.CurrentAssetPackage);
        //PreviousSentence (true);
        AudioObject currentAudio = currentPage.audioObjects[audioIndex];
        StartCoroutine(RunSequence(currentAudio));
        Debug.Log(audioIndex + "/" + pageIndex);
        //Scenetext.GetComponent<Text> ().text =currentAudio.name;
    }

    public void Register(TweenEvent evt)
    {
        tweenEvents.Add(evt);
        evt.id = tweenEvents.Count.ToString();
    }

    public void KillCurrentCoroutines()
    {
        //isMenuDeployed = true;
        StopAllCoroutines();
        foreach (SentenceRowContainer Child in sentenceContainer)
        {
            if (Child != null)
            Child.Clear();
        }
    }

    public void ReactivateCurrentCoroutine()
    {
        //isMenuDeployed = false;
        StopAllCoroutines();
        foreach (SentenceRowContainer Child in sentenceContainer)
        {
            if (Child != null)
            Child.Clear();
        }
        StartCoroutine(RunSequence(currentPage.audioObjects[audioIndex]));
    }

    public void GoToPage(int i)
    {
        StopAllCoroutines();
        foreach (SentenceRowContainer Child in sentenceContainer)
        {
            if (Child != null)
            Child.Clear();
        }
        audioIndex = i;
        StartCoroutine(RunSequence(currentPage.audioObjects[audioIndex]));
    }

    void PreviousSentence(bool playFromLast)
    {//Turn off the current passage and prep for the next passage
        StopAllCoroutines();
        foreach (SentenceRowContainer Child in sentenceContainer)
        {
            if (Child != null)
            Child.Clear();
        }

        if (audioIndex < 1 && pageIndex > 0)
        {//Switch to the previous page if can(OBSELETE)
            Debug.Log("Reset to previous page");
            pageIndex--;
            //audioIndex = currentStory.pageObjects.Count;
        }

        AudioObject currentAudio = currentPage.audioObjects[audioIndex];

        PlayPreviousSentence();
        //Debug.Log(playFromLast);
        if (playFromLast == true)
        {//If the player flips to a previous page after moving forward previously
            PreviousSentence(false);
            isGoingBack = false;

        }
    }

    void PlayPreviousSentence()
    {
        AudioObject currentAudio = currentPage.audioObjects[audioIndex];
        StartCoroutine(RunSequence(currentAudio));
        if (audioIndex > 0)
        {//reduce the passage book mark
            audioIndex--;
        }
    }


    void NextSentence(bool playFromLast)
    {//Move the narrative forward
     //Debug.Log("working");
        StopAllCoroutines();
            foreach (SentenceRowContainer Child in sentenceContainer)
            {
            if (Child != null)
                Child.Clear();
            }
        if (pageIndex >= currentStory.pageObjects.Count)
        {//when the player reaches the end of the narrative
            Debug.Log("Story ended! Back to menu...");
            //SceneManager.LoadScene("Menu");
            return;
        }

        AudioObject currentAudio = currentPage.audioObjects[audioIndex];


        PlayCurrentSentence();
        if (playFromLast == false)
        {
            NextSentence(true);
        }
    }


    void PlayCurrentSentence()
    {
        audioIndex++;
        AudioObject currentAudio = currentPage.audioObjects[audioIndex];
        StartCoroutine(RunSequence(currentAudio));
    }

    IEnumerator RunSequence(AudioObject obj)
    {
        //Debug.Log(obj.clip);

        AudioObject currentAudio = currentPage.audioObjects[audioIndex];
        Scenetext.GetComponent<Text>().text = currentAudio.name;

        //Scenetext.GetComponent<Text>().text = obj.clip.name;
        if (obj.clip != null)
        {
            audioSource.clip = obj.clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogErrorFormat("Unable to read the audio from folder {0}. " +
            "Please ensure an audio file is in the folder, and it's set to the assetbundle {1}.", obj.name, DataManager.currentStoryName);
        }

        if (obj.sentence == null)
        {
            Debug.LogErrorFormat("Unable to read the text from folder {0}. " +
            "Please ensure a text file is in the folder, and it's  set to the assetbundle {1}.", obj.name, DataManager.currentStoryName);
            yield break;
        }
        //Displaying all words in the bottom
        foreach (WordGroupObject wordGroup in obj.sentence.wordGroups)
        {
			if (wordGroup.text.Contains("///"))
            {//Get The Narrator
                //Speaker = wordGroup.text;
                //Speaker = Speaker.Remove(0, 10);
                //Debug.Log(Speaker);
                sentenceContainerCurrent += 1;
            }
            else
            {
                //Debug.Log (wordGroup.text);
                //sentenceContainer.AddText(wordGroup);
                /*foreach (SentenceRowContainer Child in sentenceContainer)
                {
                    if (Child != null)
                    Child.AddText(wordGroup);
                }*/
                sentenceContainer[sentenceContainerCurrent].AddText(wordGroup);
            }
        }
        //highlight the proper wordgroups
        int i = 0;
        WordGroupObject prevWordGroup = null;

        while (i < obj.sentence.wordGroups.Count)
        {
            WordGroupObject wordGroup = obj.sentence.wordGroups[i];
            //sentenceContainer.HighlightWordGroup(wordGroup);
                foreach (SentenceRowContainer Child in sentenceContainer)
                {
                    if (Child != null)
                    Child.HighlightWordGroup(wordGroup);
                }
            i++;
            //We calculate it like this because the times given are actually absolute times, not times per word
            float waitTime = wordGroup.time;
            if (prevWordGroup != null && i > 1)
            {
                waitTime -= prevWordGroup.time;
            }
            yield return new WaitForSeconds(waitTime);
            prevWordGroup = wordGroup;
        }
        //sentenceContainer.HighlightWordGroup(null);
        foreach (SentenceRowContainer Child in sentenceContainer)
        {
            if (Child != null)
            Child.HighlightWordGroup(null);
        }
    }

    public static List<T> FindObjectsOfTypeAll<T>()
    {
        List<T> results = new List<T>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var s = SceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }

    public void ChangeMusic(Slider newMusicVolume)
    {
        Debug.Log("newMusicVolume" + newMusicVolume.value);
        audioSource.volume = newMusicVolume.value;
    }

    public void ChangeNarrative(Slider newNarrativeVolume)
    {
        Debug.Log("newNarrativeVolume" + newNarrativeVolume.value);
        audioSource.volume = newNarrativeVolume.value;
    }

    public void ChangeTextStyle(Slider newTextStyle)
    {
        Debug.Log("newTextStyle" + newTextStyle.value);
        //audioSource.volume = newTextStyle.value;
        ScentenceContainer.GetComponent<SentenceRowContainer>().ReadAlongOn = newTextStyle.value;
    }

    public void changeClickDragFunctionality(bool state)
    {
        CanSwipe = state;
    }

}