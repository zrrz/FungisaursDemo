using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Fungisaur
{
    Crex, Bracko
}

public class GameManager : MonoBehaviour {

    public GameObject crexModel;
    public GameObject brackoModel;

//    [HideInInspector]
    public bool crexPlaced = true;



    public Fungisaur currentFungisaur = Fungisaur.Crex;

    [SerializeField]
    GameObject titleCard;
    [SerializeField]
    public GameObject summonText;

    public static GameManager instance;

    [Header("Balloon Game")]
    [SerializeField]
    GameObject balloonPrefab;
    GameObject balloonObject = null;
    public int balloonGameScore = 0;
    [SerializeField]
    UnityEngine.UI.Text balloonGameScoreUI;
    [SerializeField]
    UnityEngine.UI.Text balloonGameResultsText;
    [SerializeField]
    UnityEngine.UI.Text balloonGameToggleText;

	[SerializeField]
	UnityEngine.UI.Image balloonGameSpawnButton;
    [SerializeField]
    GameObject balloonGameScoreImage;

    [Header("Feed Game")]
    [SerializeField]
    GameObject applePrefab;
    [SerializeField]
    GameObject cornPrefab;
    [System.NonSerialized]
    public GameObject foodObject = null;
    int foodGameScore = 0;
    [SerializeField]
    UnityEngine.UI.Text foodGameScoreUI;
    [SerializeField]
    UnityEngine.UI.Text foodGameResultsText;
    [SerializeField]
    UnityEngine.UI.Text foodGameToggleText;

    [SerializeField]
    public UnityEngine.UI.Image feedGameSpawnButton;

    [SerializeField]
    Canvas canvas;

	[SerializeField]
	GameObject mainUI;
    [SerializeField]
    GameObject balloonGameUI;
    [SerializeField]
    GameObject feedGameUI;
    [SerializeField]
    GameObject scaleUI;
    [SerializeField]
    GameObject characterSelectScreenUI;
    [SerializeField]
    GameObject characterInfoScreenUI;

    [SerializeField]
    GameObject optionsMenu1;
    [SerializeField]
    GameObject optionsMenu2;


    public enum GameMode {
        Normal, Balloon, Feed, Scale, NoUI, Options, CharacterSelect, CharacterInfo
    }

    GameMode _gameMode;
    public GameMode gameMode {
        get { return _gameMode; }
    }

    void Start() {
        instance = this;
        //titleCard = GameObject.Find("Canvas/TitleCard");
        //summonText = GameObject.Find("Canvas/SummonText");
    }

	private void Update()
	{
        switch(gameMode) {
            case GameMode.Normal:
                break;
            case GameMode.Balloon:
                break;
            case GameMode.Feed:
                break;
            case GameMode.Scale:
                if (Input.touchCount == 2)
                {
                    // Store both touches.
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    // Find the position in the previous frame of each touch.
                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // Find the magnitude of the vector (the distance) between the touches in each frame.
                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    // Find the difference in the distances between each frame.
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                    GetCurrentFungisaur().transform.localScale *= 1 - deltaMagnitudeDiff * Time.deltaTime * 2f;
                    if (GetCurrentFungisaur().transform.localScale.magnitude < 0.5196152f)
                        GetCurrentFungisaur().transform.localScale = Vector3.one * 0.3f;
                }
                break;
            case GameMode.NoUI:
                if(delayedHideUI && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) {
                    ToggleNoUIMode();
                    delayedHideUI = false;
                }
                break;
            case GameMode.Options:
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
	}

    public GameObject GetCurrentFungisaur() {
        if(currentFungisaur == Fungisaur.Crex) {
            return crexModel;
        } else if(currentFungisaur == Fungisaur.Bracko) {
            return brackoModel;
        } else {
            Debug.LogError("Add case for fungisaur");
            return null;
        }
    }

	public void StartGame()
    {
        ShowSummonText();
        crexPlaced = false;
        GetCurrentFungisaur().SetActive(false);
        titleCard.gameObject.SetActive(false);
    }

    public void HomeButton() {
        crexPlaced = false;
        GetCurrentFungisaur().SetActive(false);
        titleCard.gameObject.SetActive(true);
        GetCurrentFungisaur().transform.parent.localPosition = new Vector3(0f, .5f, 0f);
        GetCurrentFungisaur().transform.localPosition = new Vector3(0f, .11f, 0f);
        GetCurrentFungisaur().transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void ResetButton() {
        crexPlaced = false;
        GetCurrentFungisaur().SetActive(false);
        GetCurrentFungisaur().transform.parent.localPosition = new Vector3(0f, .5f, 0f);
        GetCurrentFungisaur().transform.localPosition = new Vector3(0f, .11f, 0f);
        GetCurrentFungisaur().transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        AudioManager.instance.Play("Poof");
    }

    public void ShowSummonText() {
        summonText.GetComponent<UnityEngine.UI.Text>().enabled = true;
    }

    public void ToggleCharacterSelectScreen()
    {
        if (characterSelectScreenUI.activeSelf)
        {
            characterSelectScreenUI.SetActive(false);
        }
        else
        {
            characterSelectScreenUI.SetActive(true);
        }
    }

    public void ToggleSplashScreen()
    {
        if (titleCard.activeSelf)
        {
            titleCard.SetActive(false);
        }
        else
        {
            titleCard.SetActive(true);
        }
    }

    //public void ToggleCharacterInfoScreen()
    //{
    //    if (characterInfoScreenUI.activeSelf)
    //    {
    //        characterInfoScreenUI.SetActive(false);
    //    }
    //    else
    //    {
    //        characterInfoScreenUI.SetActive(true);
    //    }
    //}

    //public void ToggleCharacterInfoScreen(int fungisaurIndex)
    //{
    //    currentFungisaur = (Fungisaur)fungisaurIndex;
    //    if (characterInfoScreenUI.activeSelf)
    //    {
    //        characterInfoScreenUI.SetActive(false);
    //    }
    //    else
    //    {
    //        characterInfoScreenUI.SetActive(true);
    //    }
    //}

    public void ToggleOptionsMenu() {
        if(optionsMenu1.activeSelf)
        {
            optionsMenu1.SetActive(false);
            optionsMenu2.SetActive(false);
            optionsMenu1.transform.parent.GetChild(0).gameObject.SetActive(true);
            optionsMenu2.transform.parent.GetChild(0).gameObject.SetActive(true);
        } else
        {
            optionsMenu1.SetActive(true);
            optionsMenu2.SetActive(true);
            optionsMenu1.transform.parent.GetChild(0).gameObject.SetActive(false);
            optionsMenu2.transform.parent.GetChild(0).gameObject.SetActive(false);
        }

        //switch (gameMode)
        //{
        //    case GameMode.Options:
        //        //mainUI.SetActive(true);
        //        optionsMenu.SetActive(false);
        //        //SetGameMode(GameMode.Normal);
        //        break;
        //    case GameMode.Normal:
        //        //mainUI.SetActive(false);
        //        optionsMenu.SetActive(true);
        //        //SetGameMode(GameMode.Options);
        //        break;
        //    default:
        //        Debug.LogError("Unknown game state");
        //        break;
        //}
    }

    public void ToggleBalloonGame() {
        switch(gameMode) {
            case GameMode.Balloon:
                mainUI.SetActive(true);
                balloonGameUI.SetActive(false);
                SetGameMode(GameMode.Normal);
                balloonGameScoreUI.enabled = false;
				DespawnBalloon();
                //balloonGameToggleText.text = "Balloon Game Start";
                break;
            case GameMode.Normal:
                mainUI.SetActive(false);
                balloonGameUI.SetActive(true);
                SetGameMode(GameMode.Balloon);
                balloonGameScoreUI.enabled = true;
                //SpawnBalloon();
                //balloonGameToggleText.text = "Balloon Game End";
                balloonGameSpawnButton.gameObject.SetActive(true);
                balloonGameScoreImage.gameObject.SetActive(false);
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    public void ClearApples() {
        FoodAnimator[] apples = FindObjectsOfType<FoodAnimator>();
        for (int i = 0; i < apples.Length; i++) {
            Destroy(apples[i].gameObject);
        }
    }

    public void ToggleFeedGame()
    {
        switch (gameMode)
        {
            case GameMode.Feed:
                mainUI.SetActive(true);
                feedGameUI.SetActive(false);
                SetGameMode(GameMode.Normal);
                //foodGameScoreUI.enabled = false;
                DespawnFood();
                //foodGameToggleText.text = "Food Game Start";
                break;
            case GameMode.Normal:
                mainUI.SetActive(false);
                feedGameUI.SetActive(true);
                SetGameMode(GameMode.Feed);
                //foodGameScoreUI.enabled = true;
                //SpawnFood();
                //foodGameToggleText.text = "Food Game End";
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    public void ToggleScaleMode()
    {
        switch (gameMode)
        {
            case GameMode.Scale:
                mainUI.SetActive(true);
                scaleUI.SetActive(false);
                SetGameMode(GameMode.Normal);
                //foodGameScoreUI.enabled = false;
                //DespawnFood();
                //foodGameToggleText.text = "Food Game Start";
                break;
            case GameMode.Normal:
                mainUI.SetActive(false);
                scaleUI.SetActive(true);
                SetGameMode(GameMode.Scale);
                //foodGameScoreUI.enabled = true;
                //SpawnFood();
                //foodGameToggleText.text = "Food Game End";
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    bool delayedHideUI = false;

    void DelayedHideUIState() {
        delayedHideUI = true;
    }

    public void ToggleNoUIMode()
    {
        Debug.LogError("ToggleUI");
        switch (gameMode)
        {
            case GameMode.NoUI:
                SetGameMode(GameMode.Normal);
                canvas.enabled = true;
                //canvas.gameObject.SetActive(true);
                //mainUI.SetActive(true);
                //foodGameScoreUI.enabled = false;
                //DespawnFood();
                //foodGameToggleText.text = "Food Game Start";
                break;
            case GameMode.Normal:
                SetGameMode(GameMode.NoUI);
                Invoke("DelayedHideUIState", 0.1f);
                canvas.enabled = false;
                //canvas.gameObject.SetActive(false);
                //mainUI.SetActive(false);
                //foodGameScoreUI.enabled = true;
                //SpawnFood();
                //foodGameToggleText.text = "Food Game End";
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    void SetGameMode(GameMode newMode) {
        _gameMode = newMode;
    }

    void SpawnFood(FeedButton.FoodType foodType)
    {
        GameObject foodToSpawn = null;
        if(foodType == FeedButton.FoodType.Apple) {
            foodToSpawn = applePrefab;
        } else if(foodType == FeedButton.FoodType.Corn) {
            foodToSpawn = cornPrefab;
        }
        Vector3 spawnPosition = GetCurrentFungisaur().GetComponent<CrexBoop>().foodSpawnLocation.position;
        foodObject = (GameObject)Instantiate(foodToSpawn, spawnPosition, Quaternion.identity);
        foodObject.transform.rotation = Random.rotation;
        foodGameScore = 0;
        GameManager.instance.feedGameSpawnButton.enabled = false;
        foodObject.GetComponent<FoodAnimator>().EatFood(GetCurrentFungisaur().GetComponent<CrexBoop>());
    }

    public void DespawnFood()
    {
        //foodObject.GetComponent<Balloon>().PlayPopAnimationAndDestroy();
        if (foodObject != null)
            Destroy(foodObject);
        foodObject = null;

        //StartCoroutine(ShowFoodGameResults());
        //foodGameScoreUI.text = "Food Game Score: " + 0;
        GameManager.instance.feedGameSpawnButton.enabled = true;
    }

    public void StartThrowFeed(FeedButton.FoodType foodType) {
        SpawnFood(foodType);
	}

    public void SpawnBalloon() {
        Vector3 spawnPosition = GetCurrentFungisaur().transform.position + Vector3.up * 1.5f;
        balloonObject = (GameObject)Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);
        balloonGameScore = 0;
        balloonGameSpawnButton.gameObject.SetActive(false);
        balloonGameScoreImage.gameObject.SetActive(true);
        UpdateBalloonGameScoreCounter();
    }

    void UpdateBalloonGameScoreCounter() {
        balloonGameScoreImage.transform.Find("ScoreText").GetComponent<UnityEngine.UI.Text>().text = balloonGameScore.ToString();
        balloonGameScoreImage.transform.Find("BorderImage").GetComponent<UnityEngine.UI.Image>().fillAmount = ((float)(balloonGameScore % 6))/6f;
    }

    public void DespawnBalloon() {
        if(balloonObject != null) {
			balloonObject.GetComponent<Balloon>().PlayPopAnimationAndDestroy();
			AudioManager.instance.Play("BalloonPop");
			balloonObject = null;
        }
		StartCoroutine(ShowBalloonGameResults());
		balloonGameScoreUI.text = "Balloon Game Score: " + 0;
    }

    public void EndBalloonGame() {
        DespawnBalloon();
        balloonGameSpawnButton.gameObject.SetActive(true);
        balloonGameScoreImage.gameObject.SetActive(false);
    }

    public void AddBalloonPoint() {
        balloonGameScore++;
        UpdateBalloonGameScoreCounter();
        if (balloonGameScoreUI == null)
            Debug.LogError("balloonGameScoreUI is null");
        else
            balloonGameScoreUI.text = "Score: " + balloonGameScore.ToString();
    }

    IEnumerator ShowBalloonGameResults() {
        balloonGameResultsText.enabled = true;
        balloonGameResultsText.text = "Your score in the Balloon Game was " + balloonGameScore;
        if (balloonGameScore > 20) {
            balloonGameResultsText.text += ". Incredible!";
            AudioManager.instance.Play("Applause");
        } else if (balloonGameScore > 10) {
            balloonGameResultsText.text += ". Pretty good!";
            AudioManager.instance.Play("TaDaSound");
        } else if (balloonGameScore > 5) {
            balloonGameResultsText.text += ". Nice job!";
            AudioManager.instance.Play("TaDaSound");
        }
        Color color = balloonGameResultsText.color;
        color.a = 1f;
        balloonGameResultsText.color = color;
        yield return new WaitForSeconds(3f);

        float fadeDuration = 2f;
        for (float t = 0f; t < 1f; t += Time.deltaTime/fadeDuration) {
            color.a = t;
            balloonGameResultsText.color = color;
        }
        balloonGameResultsText.enabled = false;
    }

    IEnumerator ShowFoodGameResults()
    {
        //balloonGameResultsText.enabled = true;
        //balloonGameResultsText.text = "Your score in the Balloon Game was " + balloonGameScore;
        //if (balloonGameScore > 20)
        //    balloonGameResultsText.text += ". Incredible!";
        //else if (balloonGameScore > 10)
        //    balloonGameResultsText.text += ". Pretty good!";
        //else if (balloonGameScore > 5)
        //    balloonGameResultsText.text += ". Nice job!";
        //Color color = balloonGameResultsText.color;
        //color.a = 1f;
        //balloonGameResultsText.color = color;
        yield return new WaitForSeconds(3f);

        //float fadeDuration = 2f;
        //for (float t = 0f; t < 1f; t += Time.deltaTime / fadeDuration)
        //{
        //    color.a = t;
        //    balloonGameResultsText.color = color;
        //}
        //balloonGameResultsText.enabled = false;
    }
}
