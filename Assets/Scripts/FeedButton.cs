using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FeedButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler {

    GameObject spawnedApple;
    [SerializeField]
    GameObject applePrefab;

	void Start () {
		
	}
	
	void Update () {
		
	}

    void Click() {
        GameManager.instance.StartThrowFeed();
        //AudioManager.instance.Play("PlayButton");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(spawnedApple == null) {
			foreach(GameObject obj in eventData.hovered) {
                if (obj == eventData.pointerDrag)
					return;
			}
			
			SpawnApple();
        }
        else {
            float crexDistance = Vector3.Distance(Camera.main.transform.position, GameManager.instance.crexModel.transform.position);
            Vector3 applePosition = 
                Camera.main.transform.position 
                      + Camera.main.ScreenPointToRay(eventData.position).direction.normalized * crexDistance;
            spawnedApple.transform.position = applePosition;
        }
    }

    void SpawnApple() {
        Debug.LogError("SpawnApple");
        spawnedApple = (GameObject)Instantiate(applePrefab);
        //GameManager.instance.feedGameSpawnButton.raycastTarget = false;
        GameManager.instance.feedGameSpawnButton.enabled = false;
        GameManager.instance.foodObject = spawnedApple;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spawnedApple = null;
        //foreach (GameObject obj in eventData.hovered)
        //{
        //    if (obj == eventData.lastPress)
                
        //}
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (GameObject obj in eventData.hovered)
        {
            if (obj == gameObject)
				Click();
        }
    }
}
