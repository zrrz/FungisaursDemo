using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FungisaurController)) ]
public class FungisaurCharacter : MonoBehaviour {

    public Fungisaur fungisaur;

    [System.NonSerialized]
    public FungisaurController fungisaurController;

	private void Awake()
	{
        fungisaurController = GetComponent<FungisaurController>();
	}

	//void Start () {
		
	//}
	
	//void Update () {
		
	//}
}
