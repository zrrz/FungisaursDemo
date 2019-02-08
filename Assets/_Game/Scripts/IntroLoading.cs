using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLoading : MonoBehaviour {

    public float rotateSpeed = 0.15f;
    public float rotateAmount = 90f / 4f;

    float rotateTime = 0f;

	void Start () {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main", UnityEngine.SceneManagement.LoadSceneMode.Single);
	}
	
	void Update () {
        rotateTime += Time.deltaTime;
        if(rotateTime > rotateSpeed) {
            rotateTime = 0f;
            transform.Rotate(0f, 0f, rotateAmount);
        }
	}
}
