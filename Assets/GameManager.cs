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

    void Start() {
        instance = this;
        titleCard = GameObject.Find("Canvas/TitleCard");
        summonText = GameObject.Find("Canvas/SummonText");
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
    }

    public void ShowSummonText() {
        summonText.GetComponent<UnityEngine.UI.Text>().enabled = true;
    }
}
