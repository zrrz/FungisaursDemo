using System;
using System.Collections.Generic;
using UnityEngine.XR.iOS;
using UnityEngine;

public class ARHitTestManager : MonoBehaviour
{
	public Transform m_HitTransform;
    public Transform xPlacementObject;

    bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
        if (hitResults.Count > 0) {
            Debug.LogError("hittest");
            FungisaurManager.instance.PlaceFungisaur();
            xPlacementObject.GetComponent<SpriteRenderer>().enabled = false;

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

	void Update () {
        if (!FungisaurManager.instance.fungisaurPlaced && UIManager.instance.gameStarted)
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

