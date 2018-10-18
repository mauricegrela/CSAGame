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
    public int sentenceContainerCurrent=0;
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
    [SerializeField]
    private GameObject BackButton;
    [SerializeField]
    private GameObject NextButton;

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
        //OG_PostitionTextBody = TextBody.gameObject.transform.position;
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
            //Debug.Log(sceneindex);
  
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
        //Resetting logic for finding the 
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
    }


    void Update()
    {

    }


    public void GotoNext()
    {   sceneindex++;
        bool isloadingScene;

        string NextScene;
        NextScene = StoryManager.GetComponent<StoryManager>().NextScene;

        if (sceneindex >= StoryManager.GetComponent<StoryManager>().pagesPerScene)
        {//If the player is at the last page of the scene
            isloadingScene = true;
            audioSource.Stop();
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

            //Resetting logic for finding the 
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
            }
        }
        StoryManager.GetComponent<StoryManager>().PanRight();
    }

    public void SetUpNewTextFoward()
    {
        //Set Story Variables
        isGoingBack = false;
        isForward = true;
        transform.hasChanged = false;
        //UI Dots
        UIDots.GetComponent<DotGenerator>().updateDots(sceneindex);

        foreach (SentenceRowContainer Child in sentenceContainer)
        {//Disable all the Text Containers
            //if (Child != null)
            //Child.gameObject.SetActive(false);
        }



        for (int i = 0; i < sentenceContainer.Length; i++)
        {//Set all the containers back to null
            sentenceContainer[i] = null;
        }

        
        sentenceContainerCounter = 0;
        sentenceContainerCurrent = 0;

        foreach (Transform child in StoryManager.GetComponent<StoryManager>().TextPositions[sceneindex].transform)
        {
            // do whatever you want with child transform object here
            if(child.gameObject.tag == "TextPlacement")
            {
                sentenceContainer[sentenceContainerCounter] = child.gameObject.GetComponent<SentenceRowContainer>();;
                sentenceContainerCounter++;
                TextPositionref = child.gameObject;//GameObject.FindWithTag("TextPlacement"); 
                //Debug.Log("Working");
            }

            if (child.gameObject.tag == "SpeechBubble")
            {
                child.gameObject.GetComponent<SpeechBubbleAnimation>().setActive();
                //Debug.Log("working");
                //TextPositionref = child.gameObject;//GameObject.FindWithTag("TextPlacement");    
            }

        }

        NextSentence(isForward);
    }

    public void GotoPrevious()
    {
        sceneindex--;
       
        //Debug.Log(sceneindex);
        //bool isloadingScene;

        string LastScene;
        LastScene = StoryManager.GetComponent<StoryManager>().LastScene;

        if (sceneindex <0)
        {//If the player is at the last page of the scene
            //isloadingScene = true;
            //Debug.Log("Moving scenes");
            //Debug.Log(sceneindex+"///"+StoryManager.GetComponent<StoryManager>().pagesPerScene);
            audioSource.Stop();
            GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
            LoadingScreen.GetComponent<Image>().enabled = true;
            Resources.UnloadUnusedAssets();
            SceneManager.UnloadScene(EnvironmentTracker);

            //Check if the player has reached the end of this scene, Once reached, go to the next scene.
            SceneManager.LoadScene(LastScene, LoadSceneMode.Additive);
            isGoingBack = true;

            LoadingScreen.GetComponent<Image>().enabled = false;

            Debug.Log("Moving scenes");
        }
        else
        {
            Debug.Log("working");
            if (StoryManager.GetComponent<StoryManager>().TextPositions[sceneindex].tag == "panning")
            {
                sceneindex--;
                audioIndex--;
                Debug.Log(sceneindex);
            }
        }
        StoryManager.GetComponent<StoryManager>().PanLeft();

    }

    public void SetUpNewTextBack()
    {
        //Set Story Variables
        isGoingBack = true;
        isForward = false;

        //UI Dots
        UIDots.GetComponent<DotGenerator>().updateDots(sceneindex);

        foreach (SentenceRowContainer Child in sentenceContainer)
        {//Disable all the Text Containers
            //if (Child != null)
            //Child.gameObject.SetActive(false);
        }

        for (int i = 0; i < sentenceContainer.Length; i++)
        {//Set all the containers back to null
            sentenceContainer[i] = null;
        }


        sentenceContainerCounter = 0;
        sentenceContainerCurrent = 0;

        foreach (Transform child in StoryManager.GetComponent<StoryManager>().TextPositions[sceneindex].transform)
        {
            // do whatever you want with child transform object here
            if (child.gameObject.tag == "TextPlacement")
            {
                sentenceContainer[sentenceContainerCounter] = child.gameObject.GetComponent<SentenceRowContainer>(); ;
                sentenceContainerCounter++;
                TextPositionref = child.gameObject;//GameObject.FindWithTag("TextPlacement"); 
                //Debug.Log("Working");
            }

            if (child.gameObject.tag == "SpeechBubble")
            {
                child.gameObject.GetComponent<SpeechBubbleAnimation>().setActive(); 
            }

        }

        PreviousSentence(isGoingBack);

    }

    public void SetToLastPosition()
    {//This function goes through all the dynamic instances in the scene and sets their location to the last memeber of the array

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
        //Debug.Log("Holla");
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
        if (pageIndex >= currentStory.pageObjects.Count)
        {//when the player reaches the end of the narrative
            Debug.Log("Story ended! Back to menu...");
            //SceneManager.LoadScene("Menu");
            return;
        }

        AudioObject currentAudio = currentPage.audioObjects[audioIndex];

        PlayPreviousSentence();

    }

    void PlayPreviousSentence()
    {

        if (audioIndex > 0)
        {//reduce the passage book mark
            audioIndex--;
        }
        AudioObject currentAudio = currentPage.audioObjects[audioIndex];
        StartCoroutine(RunSequence(currentAudio));

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

        if(audioIndex == 0)
        {
            BackButton.GetComponent<Image>().enabled = false;
        }
            else 
            {
                BackButton.GetComponent<Image>().enabled = true;   
            }

        if (audioIndex == 38)
        {
            NextButton.GetComponent<Image>().enabled = false;
        }
        else
        {
            NextButton.GetComponent<Image>().enabled = true;
        }

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

        foreach (WordGroupObject wordGroup in obj.sentence.wordGroups)
        {
            if (wordGroup.text.Contains("///"))
            {//Get The Narrator
                //Speaker = wordGroup.text;
                //Speaker = Speaker.Remove(0, 10);
                Debug.Log(Speaker);
                sentenceContainerCurrent += 1;
            }
            else
            {
                Debug.Log (wordGroup.text);
                //sentenceContainer[sentenceContainerCurrent].gameObject.SetActive(false);
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

            //We calculate it like this because the times given are actually absolute times, not times per word
            //float waitTime = wordGroup.time;
            float waitTime = obj.sentence.wordGroups[i+1].time;
            if (prevWordGroup != null && i > 1)
            {
                waitTime -= obj.sentence.wordGroups[i].time;
            }
            //Debug.Log(waitTime+"///"+wordGroup.text);
            i++;
            yield return new WaitForSecondsRealtime(waitTime);
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