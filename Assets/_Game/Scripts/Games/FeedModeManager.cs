using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedModeManager : MonoBehaviour {

    public static FeedModeManager instance;

    [SerializeField]
    GameObject applePrefab;
    [SerializeField]
    GameObject cornPrefab;
    [System.NonSerialized]
    public GameObject foodObject = null;
    int foodGameScore = 0;
    [Header("UI")]

    [SerializeField]
    UnityEngine.UI.Text foodGameScoreUI;
    [SerializeField]
    UnityEngine.UI.Text foodGameResultsText;
    [SerializeField]
    UnityEngine.UI.Text foodGameToggleText;
    [SerializeField]
    public UnityEngine.UI.Image feedGameSpawnButton;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Instance of FeedModeManager already exists");
            this.enabled = false;
        }
        else
        {
            instance = this;
        }
    }

	void Start () {
		
	}

    public void SpawnFoodOnCharacter(FeedButton.FoodType foodType)
    {
        GameObject foodToSpawn = null;
        if (foodType == FeedButton.FoodType.Apple)
        {
            foodToSpawn = applePrefab;
        }
        else if (foodType == FeedButton.FoodType.Corn)
        {
            foodToSpawn = cornPrefab;
        }
        Vector3 spawnPosition = FungisaurManager.instance.GetCurrentFungisaur().GetComponent<FungisaurController>().foodSpawnLocation.position;
        foodObject = (GameObject)Instantiate(foodToSpawn, spawnPosition, Quaternion.identity);
        foodObject.transform.rotation = Random.rotation;
        foodGameScore = 0;
        feedGameSpawnButton.enabled = false;
        foodObject.GetComponent<FoodAnimator>().EatFood(FungisaurManager.instance.GetCurrentFungisaur().GetComponent<FungisaurController>());
    }

    public void DespawnFood()
    {
        //foodObject.GetComponent<Balloon>().PlayPopAnimationAndDestroy();
        if (foodObject != null)
            Destroy(foodObject);
        foodObject = null;

        //StartCoroutine(ShowFoodGameResults());
        //foodGameScoreUI.text = "Food Game Score: " + 0;
        feedGameSpawnButton.enabled = true;
    }

    public void ClearFood()
    {
        FoodAnimator[] food = FindObjectsOfType<FoodAnimator>();
        for (int i = 0; i < food.Length; i++)
        {
            Destroy(food[i].gameObject);
        }
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

    public void DragFood(GameObject spawnedFood, Vector2 screenPoint) {
        if(spawnedFood.GetComponent<FoodAnimator>().eating == false) {
			float crexDistance = Vector3.Distance(Camera.main.transform.position, FungisaurManager.instance.GetCurrentFungisaur().transform.position);
			Vector3 applePosition =
				Camera.main.transform.position
				      + Camera.main.ScreenPointToRay(screenPoint).direction.normalized * crexDistance;
			spawnedFood.transform.position = applePosition;
        }
    }
}
