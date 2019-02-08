using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceGameManager : MonoBehaviour {

    public static DanceGameManager instance;

    UnityARCameraManager cameraManager;

    [SerializeField]
    GameObject leftArrowPrefab;
    [SerializeField]
    GameObject rightArrowPrefab;
    [SerializeField]
    GameObject upArrowPrefab;
    [SerializeField]
    GameObject downArrowPrefab;
    [SerializeField]
    GameObject tapArrowPrefab;

    bool playGame = false;

    List<GameObject> spawnedArrows = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Instance of DanceGameManager already exists");
            this.enabled = false;
        }
        else
        {
            instance = this;
        }

        cameraManager = FindObjectOfType<UnityARCameraManager>();
    }

    public void SetDanceGameState(bool active)
    {
        if (active)
        {
        //    balloonGameScoreUI.enabled = true;
        //    balloonGameSpawnButton.gameObject.SetActive(true);
        //    balloonGameScoreImage.gameObject.SetActive(false);
            FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetMovementMode(FungisaurController.MovementMode.Stationary);
            FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetLookAtPosition(cameraManager.m_camera.transform.position);
        }
        else
        {
        //    balloonGameScoreUI.enabled = false;
        //    DespawnBalloon();
            FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetMovementMode(FungisaurController.MovementMode.Manual);
            playGame = false;
        }
    }

    public void StartDanceGame() {
        //TODO play music?
        playGame = true;

    }

    float timer = 0f;

	private void Update()
	{
        if(playGame) {
            for (int i = 0; i < spawnedArrows.Count; i++) {
                spawnedArrows[i].GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 1f) * 120f * Time.deltaTime;
            }
            if(spawnedArrows.Count > 0 && spawnedArrows[0].GetComponent<RectTransform>().anchoredPosition.y < -720f) {
                Destroy(spawnedArrows[0]);
                spawnedArrows.RemoveAt(0);
            }
            timer += Time.deltaTime;
            if(timer > 1.5f)
            {
                int random = Random.Range(0, 5);
                GameObject arrow = null;
                switch(random) {
                    case 0:
                        arrow = upArrowPrefab;
                        break;
                    case 1:
                        arrow = rightArrowPrefab;
                        break;
                    case 2:
                        arrow = downArrowPrefab;
                        break;
                    case 3:
                        arrow = leftArrowPrefab;
                        break;
                    case 4:
                        arrow = tapArrowPrefab;
                        break;
                }
                spawnedArrows.Add(GameObject.Instantiate(arrow, transform));
                timer = 0f;
				int direction = Random.Range(0, System.Enum.GetValues(typeof(CrexDragHandler.DraggedDirection)).Length);
				FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.Swipe((CrexDragHandler.DraggedDirection)direction);
            }
        }
	}
}
