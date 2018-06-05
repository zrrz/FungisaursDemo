using System;
using System.Collections.Generic;

namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
		public Transform m_HitTransform;
        public Transform xPlacementObject;

        bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            if (hitResults.Count > 0) {
                GameManager.instance.crexPlaced = true;
                Debug.LogError("CREX PLACED");
                GameManager.instance.crexModel.SetActive(true);
                xPlacementObject.GetComponent<SpriteRenderer>().enabled = false;
                GameObject.Find("Canvas/SummonText").GetComponent<UnityEngine.UI.Text>().enabled = false;
                GameObject.Find("Canvas/DustCloud").GetComponent<Animator>().Play("DustAnim");
                GameManager.instance.crexModel.GetComponent<CrexBoop>().depressedTimer = 0f;
                AudioManager.instance.Play("Poof");

                foreach (var hitResult in hitResults) {
                    m_HitTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
                    m_HitTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
                    Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                    transform.LookAt(Camera.main.transform);
                    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
                    return true;
                }
            }
            return false;
        }

//        public string output = "";
//        public string stack = "";
//        void OnEnable() {
//            Application.logMessageReceived += HandleLog;
//        }
//        void OnDisable() {
//            Application.logMessageReceived -= HandleLog;
//        }
//        void HandleLog(string logString, string stackTrace, LogType type) {
//            output = logString;
//            stack = stackTrace;
//        }
//
//        void OnGUI() {
//            GUILayout.Label(output);
//        }
		
		void Update () {
//            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
//            {
//                return;
//            }
            if (!GameManager.instance.crexPlaced)
            {
                if (xPlacementObject.GetComponent<SpriteRenderer>().enabled == false)
                    xPlacementObject.GetComponent<SpriteRenderer>().enabled = true;
                // prioritize reults types
                ARHitTestResultType[] resultTypes = {
//                    ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                    // if you want to use infinite planes use this:
                    //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
//                    ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
                    ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                }; 

                var screenPosition = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width/2f, Screen.height/2f, 0f));
                ARPoint point = new ARPoint {
                    x = screenPosition.x,
                    y = screenPosition.y
                };
                foreach (ARHitTestResultType resultType in resultTypes)
                {
                    List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultType);
                    if (hitResults.Count > 0)
                    {
                        foreach (var hitResult in hitResults)
                        {
                            xPlacementObject.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
//                            xPlacementObject.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                        }
                        GameManager.instance.summonText.GetComponent<UnityEngine.UI.Text>().text = "Tap the X to summon your Fungisaur!";
                    }
                }
                if (Input.GetButtonDown("Fire1") && m_HitTransform != null)
    			{
//    				var touch = Input.GetTouch(0);
//    				if (touch.phase == TouchPhase.Began)
//    				{
                        foreach (ARHitTestResultType resultType in resultTypes)
                        {
                            if (HitTestWithResultType (point, resultType))
                            {
                                return;
                            }
                        }
//    				}
    			}
            }
		}
	}
}

