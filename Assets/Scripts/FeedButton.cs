using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FeedButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler {

    GameObject spawnedFood;
    [SerializeField]
    GameObject foodPrefab;

    public enum FoodType {
        Apple, Corn
    }

    [SerializeField]
    FoodType foodType;

	void Start () {
		
	}
	
	void Update () {
		
	}

    void Click() {
        GameManager.instance.StartThrowFeed(foodType);
        //AudioManager.instance.Play("PlayButton");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(spawnedFood == null) {
			foreach(GameObject obj in eventData.hovered) {
                if (obj == eventData.pointerDrag)
					return;
			}
			
            SpawnFood();
        }
        else {
            float crexDistance = Vector3.Distance(Camera.main.transform.position, GameManager.instance.crexModel.transform.position);
            Vector3 applePosition = 
                Camera.main.transform.position 
                      + Camera.main.ScreenPointToRay(eventData.position).direction.normalized * crexDistance;
            spawnedFood.transform.position = applePosition;
        }
    }

    void SpawnFood() {
        Debug.LogError("Spawn Food");
        spawnedFood = (GameObject)Instantiate(foodPrefab);
        //GameManager.instance.feedGameSpawnButton.raycastTarget = false;
        GameManager.instance.feedGameSpawnButton.enabled = false;
        GameManager.instance.foodObject = spawnedFood;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spawnedFood = null;
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
