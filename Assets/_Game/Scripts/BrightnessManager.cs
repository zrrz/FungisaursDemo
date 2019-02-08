using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessManager : MonoBehaviour {

    public static BrightnessManager instance;

    [SerializeField]
    float minBrightness = 0f;
    [SerializeField]
    float maxBrightness = 0.5f;

    Light light;

	private void Awake()
	{
        if(instance != null) {
            Debug.LogError("Instance of BrightnessManager already exists");
            this.enabled = false;
        } else {
            instance = this;
        }

        light = GetComponent<Light>();
        if (light == null)
            Debug.LogError("No Light Component on BrightnessManager", gameObject);
	}

	void Start () {
        //Init value
        SetBrightness(0.5f);
	}
	
	void Update () {
		
	}

    /// <summary>
    /// Sets the brightness (percentage of min/max).
    /// </summary>
    /// <param name="brightnessValue">Brightness value.</param>
    public void SetBrightness(float brightnessValue)
    {
        light.intensity = Mathf.Lerp(0.0f, 0.5f, brightnessValue);
    }
}
