using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleAnimator : MonoBehaviour {

    public enum State {
        Full, Half, Quarter
    }

    [SerializeField]
    Texture2D[] appleTextures;

    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;

	//void Awake () {
        
	//}
	
	void Update () {
        
	}

    public void SetState(State state) {
        float blendshapeWeight = 0f;
        Texture2D appleTexture = null;

        if (state == State.Full) {
            blendshapeWeight = 0f;
            appleTexture = appleTextures[0];
        } else if(state == State.Half) {
            blendshapeWeight = 50f;
            appleTexture = appleTextures[1];
        } else if(state == State.Quarter) {
            blendshapeWeight = 100f;
            appleTexture = appleTextures[2];
        }

        skinnedMeshRenderer.SetBlendShapeWeight(0, blendshapeWeight);
        skinnedMeshRenderer.material.mainTexture = appleTexture;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CrexBoop crexBoop = collision.gameObject.GetComponent<CrexBoop>();
        if (crexBoop != null)
        {
            if(!eating)
                StartCoroutine(Eat(crexBoop));
        }
    }

    bool eating = false;

    IEnumerator Eat(CrexBoop crexBoop) {
        crexBoop.movementLocked = true;
        eating = true;
        crexBoop.Swipe(CrexDragHandler.DraggedDirection.Down);
        yield return new WaitForSeconds(1f);
        SetState(State.Half);
        yield return new WaitForSeconds(.625f);

        crexBoop.Swipe(CrexDragHandler.DraggedDirection.Down);
        yield return new WaitForSeconds(1f);
        SetState(State.Quarter);
        yield return new WaitForSeconds(.625f);

        crexBoop.Swipe(CrexDragHandler.DraggedDirection.Down);
        yield return new WaitForSeconds(1f);
        GameManager.instance.DespawnFood();
        crexBoop.movementLocked = false;
    }
}
