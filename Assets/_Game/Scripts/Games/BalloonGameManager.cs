using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonGameManager : MonoBehaviour {

    public static BalloonGameManager instance;

    [SerializeField]
    GameObject balloonPrefab;
    GameObject balloonObject = null;
    public int balloonGameScore = 0;

    [Header("UI")]
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

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Instance of BalloonGameManager already exists");
            this.enabled = false;
        }
        else
        {
            instance = this;
        }
    }

    public void SetBalloonGameState(bool active) {
        if(active) {
            balloonGameScoreUI.enabled = true;
            balloonGameSpawnButton.gameObject.SetActive(true);
            balloonGameScoreImage.gameObject.SetActive(false);
        } else {
            balloonGameScoreUI.enabled = false;
            DespawnBalloon();
        }
    }

    public void SpawnBalloon()
    {
        Vector3 spawnPosition = FungisaurManager.instance.GetCurrentFungisaur().transform.position + Vector3.up * 1.5f;
        balloonObject = (GameObject)Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);
        balloonGameScore = 0;
        balloonGameSpawnButton.gameObject.SetActive(false);
        balloonGameScoreImage.gameObject.SetActive(true);
        UpdateBalloonGameScoreCounter();
    }

    void UpdateBalloonGameScoreCounter()
    {
        balloonGameScoreImage.transform.Find("ScoreText").GetComponent<UnityEngine.UI.Text>().text = balloonGameScore.ToString();
        balloonGameScoreImage.transform.Find("BorderImage").GetComponent<UnityEngine.UI.Image>().fillAmount = ((float)(balloonGameScore % 6)) / 6f;
    }

    public void DespawnBalloon()
    {
        if (balloonObject != null)
        {
            balloonObject.GetComponent<Balloon>().PlayPopAnimationAndDestroy();
            AudioManager.instance.Play("BalloonPop");
            balloonObject = null;
        }
        StartCoroutine(ShowBalloonGameResults());
        balloonGameScoreUI.text = "Balloon Game Score: " + 0;
    }

    public void EndBalloonGame()
    {
        DespawnBalloon();
        balloonGameSpawnButton.gameObject.SetActive(true);
        balloonGameScoreImage.gameObject.SetActive(false);
    }

    public void AddBalloonPoint()
    {
        balloonGameScore++;
        UpdateBalloonGameScoreCounter();
        if (balloonGameScoreUI == null)
            Debug.LogError("balloonGameScoreUI is null");
        else
            balloonGameScoreUI.text = balloonGameScore.ToString();
    }

    IEnumerator ShowBalloonGameResults()
    {
        balloonGameResultsText.enabled = true;
        balloonGameResultsText.text = "Your score in the Balloon Game was " + balloonGameScore;
        if (balloonGameScore > 20)
        {
            balloonGameResultsText.text += ". Incredible!";
            AudioManager.instance.Play("Applause");
        }
        else if (balloonGameScore > 10)
        {
            balloonGameResultsText.text += ". Pretty good!";
            AudioManager.instance.Play("TaDaSound");
        }
        else if (balloonGameScore > 5)
        {
            balloonGameResultsText.text += ". Nice job!";
            AudioManager.instance.Play("TaDaSound");
        }
        Color color = balloonGameResultsText.color;
        color.a = 1f;
        balloonGameResultsText.color = color;
        yield return new WaitForSeconds(3f);

        float fadeDuration = 2f;
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeDuration)
        {
            color.a = t;
            balloonGameResultsText.color = color;
        }
        balloonGameResultsText.enabled = false;
    }
}
