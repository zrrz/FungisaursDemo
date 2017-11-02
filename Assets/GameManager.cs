using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject crexModel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        GameObject.Find("Canvas/TitleCard").gameObject.SetActive(false);
        crexModel.SetActive(true);
    }
}
