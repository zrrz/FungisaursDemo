using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchGameManager : MonoBehaviour {

    public static FetchGameManager instance;

    [SerializeField]
    GameObject fetchPrefab;

    UnityARCameraManager cameraManager;

    GameObject fetchObject;

    enum State {
        Fetching, Grabbing, Returning
    }

    State state;

	float grabTimer = 0f;

    Transform returnObject;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Instance of FetchGameManager already exists");
            this.enabled = false;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        cameraManager = FindObjectOfType<UnityARCameraManager>();
        returnObject = new GameObject("ReturnObject").transform;
    }


	private void Update()
	{
        switch(state) {
            case State.Fetching:
                if (fetchObject != null)
                {
                    Vector3 fungisaurPosition = FungisaurManager.instance.GetCurrentFungisaur().transform.position;
                    //fungisaurPosition.y = fetchObject.transform.position.y;
                    float fetchDistance = Vector3.Distance(fetchObject.transform.position, FungisaurManager.instance.GetCurrentFungisaur().transform.position);
                    if (fetchDistance < 0.2f)
                    {
                        //Destroy(fetchObject);
                        FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetTargetObject(null);
                        FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.Swipe(CrexDragHandler.DraggedDirection.Down);
                        //TODO put stick in mouth
                        state = State.Grabbing;
                        grabTimer = 1f;
                    }
                }
                break;

            case State.Grabbing:
                if(grabTimer > 0f) {
                    grabTimer -= Time.deltaTime;
                    if(grabTimer <= 0f) {
                        Destroy(fetchObject.GetComponent<Rigidbody>());
                        fetchObject.transform.parent = FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.mouthTransform;
                        fetchObject.transform.localPosition = Vector3.zero;
                        FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetTargetObject(returnObject);
                        state = State.Returning;
                    }
                }
                break;

            case State.Returning:
                Vector3 position = Camera.main.transform.position;
                position.y = FungisaurManager.instance.GetCurrentFungisaur().transform.Find("Plane").transform.position.y + .01f;
                returnObject.position = position;

                float returnDistance = Vector3.Distance(position, FungisaurManager.instance.GetCurrentFungisaur().transform.position);
                if (returnDistance < 0.2f)
                {
                    Destroy(fetchObject);
                    FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetTargetObject(null);
                    FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.Swipe(CrexDragHandler.DraggedDirection.Down);
                }
                break;
            default:
                Debug.LogError("No case");
                break;
        }

	}

	public void SetFetchGameState(bool active)
    {
        if (active)
        {
            //    balloonGameScoreUI.enabled = true;
            //    balloonGameSpawnButton.gameObject.SetActive(true);
            //    balloonGameScoreImage.gameObject.SetActive(false);
            FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetMovementMode(FungisaurController.MovementMode.Targeted);
        }
        else
        {
        //    balloonGameScoreUI.enabled = false;
        //    DespawnBalloon();
            if(fetchObject) {
				Destroy(fetchObject);
            }
            FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetMovementMode(FungisaurController.MovementMode.Manual);
            FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetTargetObject(null);
        }
    }

    public void SpawnFetchObject() {
        if(fetchObject != null) {
            Destroy(fetchObject);
        }
        Vector3 spawnPosition = cameraManager.m_camera.transform.position;
        fetchObject = (GameObject)Instantiate(fetchPrefab, spawnPosition, Quaternion.identity);
        FungisaurManager.instance.GetCurrentFungisaur().fungisaurController.SetTargetObject(fetchObject.transform);

        //TODO Add force
        Vector3 forward = cameraManager.m_camera.transform.TransformDirection(new Vector3(0f, 0f, 1f));
        forward.y = 0f;
        fetchObject.GetComponent<Rigidbody>().AddForce(forward * 100f + Vector3.up * 30f);

        state = State.Fetching;

        //balloonGameScore = 0;
        //balloonGameSpawnButton.gameObject.SetActive(false);
        //balloonGameScoreImage.gameObject.SetActive(true);
        //UpdateBalloonGameScoreCounter();
    }
}
