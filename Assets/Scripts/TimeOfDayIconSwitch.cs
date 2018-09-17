using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayIconSwitch : MonoBehaviour {

    public Sprite[] icons;
    UnityEngine.UI.Image image;

	void Start () {
        image = GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(UpdateIcon());
	}
	
	void Update () {
		
	}

    IEnumerator UpdateIcon() {
        while(true) {
            int hour = System.DateTime.Now.Hour;
            if (hour >= 20 || hour < 5) //8p to 5am
                image.sprite = icons[0];
            else if (hour >= 5 && hour < 8) //5a to 8a
                image.sprite = icons[1];
            else if (hour >= 8 && hour < 18) //8a to 6p
                image.sprite = icons[2];
            else if (hour >= 18 && hour < 21) //6p to 8p
                image.sprite = icons[3];

            yield return new WaitForSeconds(30f);
        }
    }
}
