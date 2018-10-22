using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

	[SerializeField]
	GameObject crexCharacterInfoScreen;
    [SerializeField]
    GameObject brackoCharacterInfoScreen;

    //int currentCharacterInfoScreen = 0;

	void Start () {
        HideCharacterInfoScreen();
	}
	
	//void Update () {
		
	//}

    public void ShowCurrentCharacterInfoScreen()
    {
        ShowCharacterInfoScreen(GameManager.instance.currentFungisaur);
    }

    public void ShowCharacterInfoScreen(int index)
    {
        ShowCharacterInfoScreen((Fungisaur)index);
    }

    public void ShowCharacterInfoScreen(Fungisaur fungisaur) {
        //currentCharacterInfoScreen = (int)fungisaur;
        if (GameManager.instance.currentFungisaur != fungisaur)
            GameManager.instance.ResetButton();
        GameManager.instance.currentFungisaur = fungisaur;
        switch(fungisaur) {
            case Fungisaur.Crex:
                crexCharacterInfoScreen.SetActive(true);
                break;
            case Fungisaur.Bracko:
                brackoCharacterInfoScreen.SetActive(true);
                break;
        }
    }

    public void IncrementCharacterInfoScreen(int amount) {
        HideCharacterInfoScreen();
        int index = (int)GameManager.instance.currentFungisaur;
        index = (index + amount) % System.Enum.GetValues(typeof(Fungisaur)).Length;
        ShowCharacterInfoScreen(index);
    }

    public void HideCharacterInfoScreen() {
        crexCharacterInfoScreen.SetActive(false);
        brackoCharacterInfoScreen.SetActive(false);
    }
}
