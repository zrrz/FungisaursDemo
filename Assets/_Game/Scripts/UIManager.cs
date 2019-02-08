using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    GameObject mainUI;

    [SerializeField]
    GameObject scaleUI;
    [SerializeField]
    GameObject feedGameUI;
    [SerializeField]
    GameObject balloonGameUI;
    [SerializeField]
    GameObject fetchGameUI;
    [SerializeField]
    GameObject danceGameUI;
    [SerializeField]
    GameObject characterInfoScreenUI;
    [SerializeField]
    GameObject optionsMenu1;
    [SerializeField]
    GameObject optionsMenu2;

	[SerializeField]
	GameObject crexCharacterInfoScreen;
    [SerializeField]
    GameObject brackoCharacterInfoScreen;

    [SerializeField]
    CharacterButton crexCharacterButton;
    [SerializeField]
    CharacterButton brackoCharacterButton;

    [SerializeField]
    GameObject popupScreen;
    [SerializeField]
    GameObject homeConfirmationPopup;
    [SerializeField]
    GameObject welcomePopup;
    [SerializeField]
    GameObject summonInfoPopup;
    [SerializeField]
    GameObject movePopup;
    [SerializeField]
    GameObject interactPopup;
    [SerializeField]
    GameObject balloonPopup;
    [SerializeField]
    GameObject feedPopup;
    [SerializeField]
    GameObject unlockPopup;
    [SerializeField]
    GameObject unlockedCRexPopup;

    public bool gameStarted = false;

    [System.Serializable]
    class CharacterButton {
        [SerializeField]
        public GameObject characterButton;
		public Image characterImage;
        public Image blockedImage;
        public Image outlineImage;
    }

    [SerializeField]
    NameCard characterSelectNameCard;
    [SerializeField]
    NameCard mainPageNameCard;

    [System.Serializable]
    class NameCard {
        [SerializeField]
        public GameObject nameCard;
        public Text scoreText;
        public Text levelText;
        public Text nameText;
        public Image characterImage;
    }

    //int currentCharacterInfoScreen = 0;

    [SerializeField]
    GameObject titleScreen;
    [SerializeField]
    GameObject scanCharacterPage;
    [SerializeField]
    GameObject mainPage;
    [SerializeField]
    GameObject characterSelectPage;
    [SerializeField]
    GameObject creditsPage;
    [SerializeField]
    GameObject storyPage;
    [SerializeField]
    GameObject socialPage;
    [SerializeField]
    GameObject brightnessPage;
	[SerializeField]
	GameObject pauseMenuPage;
    [SerializeField]
    GameObject pauseMenuButton;
    [SerializeField]
    GameObject joystick;
    [SerializeField]
    GameObject characterSelectScreenUI;

    [SerializeField]
    public GameObject summonText;

	[System.NonSerialized]
	public bool delayedHideUI = false;

    bool showWelcomePopup = true;
    bool showMovePopup = true;
    bool showBalloonPopup = true;
    bool showFeedPopup = true;
    bool showUnlockPopup = true;

    private void Awake()
    {
        if(instance != null) {
            Debug.LogError("Instance of UIManager already exists.");
            this.enabled = false;
            return;
        } else {
            instance = this;
        }
    }

    void Start () {
        HideCharacterInfoScreen();
        OutlineCharacterButton(FungisaurManager.instance.currentFungisaur);
        UpdateNameCard(FungisaurManager.instance.currentFungisaur, mainPageNameCard);
    }
    
    //void Update () {
        
    //}

    public void SpawnFungisaur() {
        GameObject.Find("Canvas/Main/SummonText").GetComponent<UnityEngine.UI.Text>().enabled = false;
        GameObject.Find("Canvas/Main/DustCloud").GetComponent<Animator>().Play("DustAnim");

        if(showMovePopup) {
            showMovePopup = false;
            ShowMovePopup(true);
        }
    }


    void DelayedHideUIState()
    {
        delayedHideUI = true;
    }

    public void ShowSummonText()
    {
        summonText.GetComponent<UnityEngine.UI.Text>().text = "Tap the X to summon your Fungisaur!"; //TODO only once
        summonText.GetComponent<UnityEngine.UI.Text>().enabled = true;
    }

    public void ShowCurrentCharacterInfoScreen()
    {
        ShowCharacterInfoScreen(FungisaurManager.instance.currentFungisaur);
    }

    public void ShowCharacterInfoScreen(int index)
    {
        ShowCharacterInfoScreen((Fungisaur)index);
    }

    public void StartGameButton()
    {
        instance.ShowSummonText();

        instance.ToggleTitleScreen(false);

        if(showWelcomePopup) {
            showWelcomePopup = false;
			ShowWelcomePopup(true);
        }
        gameStarted = true;
    }

    //public void HomeButton()
    //{
    //    ShowReturnToHomeConfirmationDialogue(true);
    //}

    void TogglePopupScreen(GameObject popup, bool show) {
        if (show)
        {
            if (popupScreen.activeSelf == false)
            {
                popupScreen.SetActive(true);
            }
            popup.SetActive(true);
        }
        else
        {
            if (popupScreen.activeSelf == true)
            {
                popupScreen.SetActive(false);
            }
            popup.SetActive(false);
        }
    }

    public void ShowWelcomePopup(bool show)
    {
        TogglePopupScreen(welcomePopup, show);
    }

    public void ShowSummonInfoPopup(bool show)
    {
        TogglePopupScreen(summonInfoPopup, show);
    }

    public void ShowMovePopup(bool show)
    {
        TogglePopupScreen(movePopup, show);
    }

    public void ShowInteractPopup(bool show)
    {
        TogglePopupScreen(interactPopup, show);
    }

    public void ShowBalloonPopup(bool show)
    {
        TogglePopupScreen(balloonPopup, show);
    }

    public void ShowFeedPopup(bool show)
    {
        TogglePopupScreen(feedPopup, show);
    }

    public void ShowUnlockPopup(bool show)
    {
        TogglePopupScreen(unlockPopup, show);
    }

    public void ShowUnlockedCRexPopup(bool show)
    {
        TogglePopupScreen(unlockedCRexPopup, show);
    }

	public void ShowReturnToHomeConfirmationPopup(bool show) {
		TogglePopupScreen(homeConfirmationPopup, show);
	}

    public void GoToHomeScreen() {
        ShowReturnToHomeConfirmationPopup(false);
        FungisaurManager.instance.ResetFungisaur();
        ToggleTitleScreen(true);
        gameStarted = false;
    }

    public void ResetButton()
    {
        FungisaurManager.instance.ResetFungisaur();
        AudioManager.instance.Play("Poof");
    }

    public void ShowCharacterInfoScreen(Fungisaur fungisaur) {
        //currentCharacterInfoScreen = (int)fungisaur;
        if (FungisaurManager.instance.currentFungisaur != fungisaur) {
			ResetButton();
            FungisaurManager.instance.currentFungisaur = fungisaur;
			OutlineCharacterButton(fungisaur);
            UpdateNameCard(fungisaur, characterSelectNameCard);
            UpdateNameCard(fungisaur, mainPageNameCard);
        } else {
            switch(fungisaur) {
                case Fungisaur.Crex:
                    crexCharacterInfoScreen.SetActive(true);
                    break;
                case Fungisaur.Bracko:
                    brackoCharacterInfoScreen.SetActive(true);
                    break;
            }
        }
    }

    public void IncrementCharacterInfoScreen(int amount) {
        HideCharacterInfoScreen();
        int index = (int)FungisaurManager.instance.currentFungisaur;
        int length = System.Enum.GetValues(typeof(Fungisaur)).Length;
        index += amount;
        if (index < 0)
            index = length;
        if (index >= length)
            index = 0;
        ShowCharacterInfoScreen(index);
    }

    public void HideCharacterInfoScreen() {
        crexCharacterInfoScreen.SetActive(false);
        brackoCharacterInfoScreen.SetActive(false);
    }

    public void OutlineCharacterButton(Fungisaur fungisaur) {
        switch (fungisaur)
        {
            case Fungisaur.Crex:
                crexCharacterButton.outlineImage.enabled = true;
                brackoCharacterButton.outlineImage.enabled = false;
                break;
            case Fungisaur.Bracko:
                brackoCharacterButton.outlineImage.enabled = true;
                crexCharacterButton.outlineImage.enabled = false;
                break;
        }
    }

    void UpdateNameCard(Fungisaur fungisaur, NameCard nameCard) {
        switch (fungisaur)
        {
            case Fungisaur.Crex:
                nameCard.nameText.text = "C-REX";
                nameCard.levelText.text = "8";
                nameCard.scoreText.text = "153";
                nameCard.characterImage.sprite = crexCharacterButton.characterImage.sprite;
                break;
            case Fungisaur.Bracko:
                nameCard.nameText.text = "BRACKO";
                nameCard.levelText.text = "10";
                nameCard.scoreText.text = "468";
                nameCard.characterImage.sprite = brackoCharacterButton.characterImage.sprite;
                break;
        }
    }

    public void ToggleTitleScreen(bool state)
    {
        if (state == true)
        {
            titleScreen.gameObject.SetActive(true);
        }
        else
        {
            titleScreen.SetActive(false);
            TogglePauseMenu(false);
        }
    }

    public void ToggleCharacterSelectScreen(bool state)
    {
        if (state == true)
        {
            characterSelectScreenUI.SetActive(true);
        }
        else
        {
            characterSelectScreenUI.SetActive(false);
            TogglePauseMenu(false);
        }
    }

    public void TogglePauseMenu(bool state)
    {
        if (state == true)
        {
            pauseMenuPage.SetActive(true);
            pauseMenuButton.SetActive(true);
        }
        else
        {
            pauseMenuPage.SetActive(false);
            pauseMenuButton.SetActive(false);
        }
    }

    public void ToggleScanMode(bool state) {
        if(state == true) {
            if (showUnlockPopup)
            {
                showUnlockPopup = false;
                ShowUnlockPopup(true);
            }
            scanCharacterPage.SetActive(true);
            mainPage.SetActive(false);
			joystick.SetActive(false);
            characterSelectPage.SetActive(false);
        } 
        else 
        {
            scanCharacterPage.SetActive(false);
            mainPage.SetActive(true);
            joystick.SetActive(true);
            characterSelectPage.SetActive(true);
        }
    }

    public void ToggleCreditsScreen(bool state)
    {
        if (state == true)
        {
            creditsPage.SetActive(true);
        }
        else
        {
            creditsPage.SetActive(false);
            TogglePauseMenu(false);
        }
    }

    public void ToggleStoryScreen(bool state)
    {
        if (state == true)
        {
            storyPage.SetActive(true);
            StoryScreenManager storyScreenManager = storyPage.GetComponent<StoryScreenManager>();
            if(storyScreenManager == null) {
                Debug.LogError("No StoryScreenManager on storyPage");
                return;
            }
            storyScreenManager.SetCurrentPage(0);
            AudioManager.instance.Play("StoryScreen_Theme");
        }
        else
        {
            storyPage.SetActive(false);
            AudioManager.instance.Play("LoadingScreen_Theme");
        }
    }

    public void ToggleBrightnessPage(bool state)
    {
        if (state == true)
        {
            brightnessPage.SetActive(true);
        }
        else
        {
            brightnessPage.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the brightness (percentage of min/max).
    /// </summary>
    /// <param name="brightnessValue">Brightness value.</param>
    public void SetBrightness(float brightnessValue) {
        BrightnessManager.instance.SetBrightness(brightnessValue);
    }

    public void ToggleSocialPage(bool state)
    {
        if (state == true)
        {
            socialPage.SetActive(true);
        }
        else
        {
            socialPage.SetActive(false);
        }
    }

    public void OpenFacebookSocial() {
        Application.OpenURL("https://www.facebook.com/Fungisaurs/");
    }

    public void OpenInstagramSocial() {
        Application.OpenURL("https://www.instagram.com/fungisaurs/");
    }

    public void OpenTwitterSocial() {
        Application.OpenURL("https://twitter.com/fungisaurs");
    }

    public void OpenYoutubeSocial() {
        Application.OpenURL("https://www.youtube.com/channel/UC7J9wEQzi1sbhR9Ck6ospNw");
    }

    public void ToggleScaleMode()
    {
        switch (GameManager.instance.gameMode)
        {
            case GameManager.GameMode.Scale:
                mainUI.SetActive(true);
                scaleUI.SetActive(false);
                GameManager.instance.SetGameMode(GameManager.GameMode.Normal);
                //foodGameScoreUI.enabled = false;
                //DespawnFood();
                //foodGameToggleText.text = "Food Game Start";
                break;
            case GameManager.GameMode.Normal:
                mainUI.SetActive(false);
                scaleUI.SetActive(true);
                GameManager.instance.SetGameMode(GameManager.GameMode.Scale);
                //foodGameScoreUI.enabled = true;
                //SpawnFood();
                //foodGameToggleText.text = "Food Game End";
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    public void ToggleNoUIMode()
    {
        switch (GameManager.instance.gameMode)
        {
            case GameManager.GameMode.NoUI:
                GameManager.instance.SetGameMode(GameManager.GameMode.Normal);
                canvas.enabled = true;
                break;
            case GameManager.GameMode.Normal:
                GameManager.instance.SetGameMode(GameManager.GameMode.NoUI);
                Invoke("DelayedHideUIState", 0.1f);
                canvas.enabled = false;
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    public void ToggleOptionsMenu()
    {
        if (optionsMenu1.activeSelf)
        {
            optionsMenu1.SetActive(false);
            optionsMenu2.SetActive(false);
            optionsMenu1.transform.parent.GetChild(0).gameObject.SetActive(true);
            optionsMenu2.transform.parent.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            optionsMenu1.SetActive(true);
            optionsMenu2.SetActive(true);
            optionsMenu1.transform.parent.GetChild(0).gameObject.SetActive(false);
            optionsMenu2.transform.parent.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ToggleBalloonGame()
    {
        switch (GameManager.instance.gameMode)
        {
            case GameManager.GameMode.Play:
				GameManager.instance.SetGameMode(GameManager.GameMode.Normal);
                mainUI.SetActive(true);
                balloonGameUI.SetActive(false);
                BalloonGameManager.instance.SetBalloonGameState(false);
                break;
            case GameManager.GameMode.Normal:
                if(showBalloonPopup) {
                    showBalloonPopup = false;
                    ShowBalloonPopup(true);
                }
				GameManager.instance.SetGameMode(GameManager.GameMode.Play);
                mainUI.SetActive(false);
                balloonGameUI.SetActive(true);
                BalloonGameManager.instance.SetBalloonGameState(true);
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    public void ToggleFetchGame()
    {
        switch (GameManager.instance.gameMode)
        {
            case GameManager.GameMode.Play:
                GameManager.instance.SetGameMode(GameManager.GameMode.Normal);
                mainUI.SetActive(true);
                fetchGameUI.SetActive(false);
                FetchGameManager.instance.SetFetchGameState(false);
                break;
            case GameManager.GameMode.Normal:
                GameManager.instance.SetGameMode(GameManager.GameMode.Play);
                mainUI.SetActive(false);
                fetchGameUI.SetActive(true);
                FetchGameManager.instance.SetFetchGameState(true);
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    public void ToggleDanceGame()
    {
        switch (GameManager.instance.gameMode)
        {
            case GameManager.GameMode.Play:
                GameManager.instance.SetGameMode(GameManager.GameMode.Normal);
                mainUI.SetActive(true);
                danceGameUI.SetActive(false);
                DanceGameManager.instance.SetDanceGameState(false);
                break;
            case GameManager.GameMode.Normal:
                GameManager.instance.SetGameMode(GameManager.GameMode.Play);
                mainUI.SetActive(false);
                danceGameUI.SetActive(true);
                DanceGameManager.instance.SetDanceGameState(true);
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }

    public void ToggleFeedGame()
    {
        switch (GameManager.instance.gameMode)
        {
            case GameManager.GameMode.Feed:
                mainUI.SetActive(true);
                feedGameUI.SetActive(false);
                GameManager.instance.SetGameMode(GameManager.GameMode.Normal);
                //foodGameScoreUI.enabled = false;
                FeedModeManager.instance.DespawnFood();
                //foodGameToggleText.text = "Food Game Start";
                break;
            case GameManager.GameMode.Normal:
                if (showFeedPopup)
                {
                    showFeedPopup = false;
                    ShowFeedPopup(true);
                }
                mainUI.SetActive(false);
                feedGameUI.SetActive(true);
                GameManager.instance.SetGameMode(GameManager.GameMode.Feed);
                //foodGameScoreUI.enabled = true;
                //SpawnFood();
                //foodGameToggleText.text = "Food Game End";
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
    }
}
