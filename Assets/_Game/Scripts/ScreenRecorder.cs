using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRecorder : MonoBehaviour {

//    public void OnReadyForRecording(bool enabled) {
//        if(enabled) {
//            // The recording is supported
////            myGameObject.SetUpRecording();
//            GetComponent<UnityEngine.UI.Button>().interactable = true;
//        } 
//    }

    public GameObject photoSavedText;

    void Start() {
        // Other init code
        // ...

        // Register for the Everyplay ReadyForRecording event
//        Everyplay.ReadyForRecording += OnReadyForRecording;
    }
    public void ToggleRecording() {
        //        RenderTexture tempRT = new RenderTexture(Screen.width,Screen.height, 24 );
        //
        //        Camera.main.targetTexture = tempRT;
        //        Camera.main.Render();
        //
        //        RenderTexture.active = tempRT;
        //
        //        Texture2D myTexture2D = new Texture2D(Screen.width, Screen.height);
        //        myTexture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        //        myTexture2D.Apply();
        //
        //        RenderTexture.active = null;
        //        Camera.main.targetTexture = null;
        //
        //        NativeToolkit.SaveImage(myTexture2D, "Fungisaurs_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff"));

        StartCoroutine(TakePhotoNoUI());

//        if (Everyplay.IsRecording())
//        {
//            UnityEngine.UI.ColorBlock colors = GetComponent<UnityEngine.UI.Button>().colors;
//            colors.normalColor = colors.highlightedColor = Color.white;
//            GetComponent<UnityEngine.UI.Button>().colors = colors;
//            Everyplay.StopRecording();
//            Everyplay.Show();
//        }
//        else
//        {
//            Everyplay.StartRecording();
//            UnityEngine.UI.ColorBlock colors = GetComponent<UnityEngine.UI.Button>().colors;
//            colors.normalColor = colors.highlightedColor =  Color.red;
//            GetComponent<UnityEngine.UI.Button>().colors = colors;
//        }
    }

    IEnumerator TakePhotoNoUI() {
        UIManager.instance.ToggleNoUIMode();

        yield return new WaitForSeconds(0.2f);

        NativeToolkit.SaveScreenshot("Fungisaurs_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff"));

		yield return new WaitForSeconds(0.2f);
        UIManager.instance.ToggleNoUIMode();
		StartCoroutine(ShowSavedPhotoText());
    }

    IEnumerator ShowSavedPhotoText() {
        yield return null;
        photoSavedText.GetComponent<UnityEngine.UI.Text>().color = new Color(241 / 255f, 130f / 255f, 33f / 255f, 255f);
        yield return new WaitForSeconds(0.8f);

        for (float t = 1f; t > 0f; t -= Time.deltaTime * 2f)
        {
            photoSavedText.GetComponent<UnityEngine.UI.Text>().color = new Color(241 / 255f, 130f / 255f, 33f / 255f, t);
            yield return null;
        }
        photoSavedText.GetComponent<UnityEngine.UI.Text>().color = new Color(241 / 255f, 130f / 255f, 33f / 255f, 0f);
    }
}
