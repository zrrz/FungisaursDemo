using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject crexModel;

//    [HideInInspector]
    public bool crexPlaced = true;

    GameObject titleCard;
    public GameObject summonText;

    public static GameManager instance;

    [Header("Balloon Game")]
    [SerializeField]
    GameObject balloonPrefab;
    GameObject balloonObject = null;
    int balloonGameScore = 0;
    [SerializeField]
    UnityEngine.UI.Text balloonGameScoreUI;
    [SerializeField]
    UnityEngine.UI.Text balloonGameResultsText;
    [SerializeField]
    UnityEngine.UI.Text balloonGameToggleText;

	[SerializeField]
	UnityEngine.UI.Image balloonGameSpawnButton;

    [Header("Feed Game")]
    [SerializeField]
    GameObject foodPrefab;
    GameObject foodObject = null;
    int foodGameScore = 0;
    [SerializeField]
    UnityEngine.UI.Text foodGameScoreUI;
    [SerializeField]
    UnityEngine.UI.Text foodGameResultsText;
    [SerializeField]
    UnityEngine.UI.Text foodGameToggleText;

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
    GameObject optionsMenu;


    public enum GameMode {
        Normal, Balloon, Feed, Scale, NoUI
    }

    GameMode _gameMode;
    public GameMode gameMode {
        get { return _gameMode; }
    }

    void Start() {
        instance = this;
        titleCard = GameObject.Find("Canvas/TitleCard");
        summonText = GameObject.Find("Canvas/SummonText");
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
                    crexModel.transform.localScale *= 1 - deltaMagnitudeDiff * Time.deltaTime * 2f;
                }
                break;
            case GameMode.NoUI:
                if(delayedHideUI && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) {
                    ToggleNoUIMode();
                    delayedHideUI = false;
                }
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
	}

	public void StartGame()
    {
        ShowSummonText();
        crexPlaced = false;
        crexModel.SetActive(false);
        titleCard.gameObject.SetActive(false);
    }

    public void HomeButton() {
        crexPlaced = false;
        crexModel.SetActive(false);
        titleCard.gameObject.SetActive(true);
        crexModel.transform.parent.localPosition = new Vector3(0f, .5f, 0f);
        crexModel.transform.localPosition = new Vector3(0f, .11f, 0f);
        crexModel.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void ResetButton() {
        crexPlaced = false;
        crexModel.SetActive(false);
        crexModel.transform.parent.localPosition = new Vector3(0f, .5f, 0f);
        crexModel.transform.localPosition = new Vector3(0f, .11f, 0f);
        crexModel.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        AudioManager.instance.Play("Poof");
    }

    public void ShowSummonText() {
        summonText.GetComponent<UnityEngine.UI.Text>().enabled = true;
    }

    public void ToggleOptionsMenu() {
        if (optionsMenu.activeSelf)
            optionsMenu.SetActive(false);
        else
            optionsMenu.SetActive(true);
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
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
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
                foodGameScoreUI.enabled = false;
                DespawnFood();
                foodGameToggleText.text = "Food Game Start";
                break;
            case GameMode.Normal:
                mainUI.SetActive(false);
                feedGameUI.SetActive(true);
                SetGameMode(GameMode.Feed);
                foodGameScoreUI.enabled = true;
                SpawnFood();
                foodGameToggleText.text = "Food Game End";
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

    void SpawnFood()
    {
        Vector3 spawnPosition = crexModel.transform.position + Vector3.up * 1.5f;
        foodObject = (GameObject)Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        foodGameScore = 0;
    }

    public void DespawnFood()
    {
        //foodObject.GetComponent<Balloon>().PlayPopAnimationAndDestroy();
        foodObject = null;
        StartCoroutine(ShowFoodGameResults());
        foodGameScoreUI.text = "Food Game Score: " + 0;
    }

	public void StartThrowFeed() {
		
	}

    public void SpawnBalloon() {
        Vector3 spawnPosition = crexModel.transform.position + Vector3.up * 1.5f;
        balloonObject = (GameObject)Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);
        balloonGameScore = 0;
        balloonGameSpawnButton.gameObject.SetActive(false);
    }

    public void DespawnBalloon() {
        balloonObject.GetComponent<Balloon>().PlayPopAnimationAndDestroy();
        AudioManager.instance.Play("BalloonPop");
        balloonObject = null;
        StartCoroutine(ShowBalloonGameResults());
        balloonGameScoreUI.text = "Balloon Game Score: " + 0;
    }

    public void EndBalloonGame() {
        DespawnBalloon();
        balloonGameSpawnButton.gameObject.SetActive(true);
    }

    public void AddBalloonPoint() {
        balloonGameScore++;
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
