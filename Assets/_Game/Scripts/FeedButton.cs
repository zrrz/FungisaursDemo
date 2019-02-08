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
        FeedModeManager.instance.SpawnFoodOnCharacter(foodType);
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
            FeedModeManager.instance.DragFood(spawnedFood, eventData.position);
        }
    }

    void SpawnFood() {
        //Debug.LogError("Spawn Food");
        spawnedFood = (GameObject)Instantiate(foodPrefab);
        //GameManager.instance.feedGameSpawnButton.raycastTarget = false;
        FeedModeManager.instance.feedGameSpawnButton.enabled = false;
        FeedModeManager.instance.foodObject = spawnedFood;
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
