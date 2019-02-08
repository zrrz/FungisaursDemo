using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;


    public enum GameMode {
        Normal, Play, Feed, Scale, NoUI, Options, CharacterSelect, CharacterInfo
    }

    GameMode _gameMode;
    public GameMode gameMode {
        get { return _gameMode; }
    }

    void Start() {
        instance = this;
    }

	private void Update()
	{
        switch(gameMode) {
            case GameMode.Normal:
                break;
            case GameMode.Play:
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
                    FungisaurCharacter currentFungisaur = FungisaurManager.instance.GetCurrentFungisaur();
                    currentFungisaur.transform.localScale *= 1 - deltaMagnitudeDiff * Time.deltaTime * 2f;
                    if (currentFungisaur.transform.localScale.magnitude < 0.5196152f)
                        currentFungisaur.transform.localScale = Vector3.one * 0.3f;
                }
                break;
            case GameMode.NoUI:
                if(UIManager.instance.delayedHideUI && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) {
                    UIManager.instance.ToggleNoUIMode();
                    UIManager.instance.delayedHideUI = false;
                }
                break;
            case GameMode.Options:
                break;
            default:
                Debug.LogError("Unknown game state");
                break;
        }
	}

    public void SetGameMode(GameMode newMode) {
        _gameMode = newMode;
    }
}
