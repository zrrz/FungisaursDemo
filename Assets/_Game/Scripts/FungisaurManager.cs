using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Fungisaur
{
    Crex, Bracko
}

public class FungisaurManager : MonoBehaviour {

    public static FungisaurManager instance;

    public FungisaurCharacter crexModel;
    public FungisaurCharacter brackoModel;

    //    [HideInInspector]
    public bool fungisaurPlaced = true;

    public Fungisaur currentFungisaur = Fungisaur.Crex;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Instance of FungisaurManager already exists");
            this.enabled = false;
        }
        else
        {
            instance = this;
        }
    }

	void Start () {
        fungisaurPlaced = false;

        crexModel.gameObject.SetActive(false);
        brackoModel.gameObject.SetActive(false);
	}
	
	void Update () {
		
	}

    public FungisaurCharacter GetCurrentFungisaur()
    {
        if (currentFungisaur == Fungisaur.Crex)
        {
            return crexModel;
        }
        else if (currentFungisaur == Fungisaur.Bracko)
        {
            return brackoModel;
        }
        else
        {
            Debug.LogError("Add case for fungisaur");
            return null;
        }
    }

    public void ResetFungisaur() {
        fungisaurPlaced = false;
        GetCurrentFungisaur().gameObject.SetActive(false);
        GetCurrentFungisaur().transform.parent.localPosition = new Vector3(0f, .5f, 0f);
        GetCurrentFungisaur().transform.localPosition = new Vector3(0f, .11f, 0f);
        GetCurrentFungisaur().transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void PlaceFungisaur() {
        fungisaurPlaced = true;
        Debug.LogError("FUNGISAUR PLACED");
        GetCurrentFungisaur().gameObject.SetActive(true);
        GetCurrentFungisaur().GetComponent<FungisaurController>().depressedTimer = 0f;
        AudioManager.instance.Play("Poof");
        UIManager.instance.SpawnFungisaur();
    }
}
