using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAnimator : MonoBehaviour {

    public enum State {
        Full, Half, Quarter
    }

    [SerializeField]
    Texture2D[] foodTextures;
    [SerializeField]
    Texture2D[] foodNormals;

    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;

	//void Awake () {
        
	//}
	
	void Update () {
        
	}

    public void SetState(State state) {
        float blendshapeWeight = 0f;
        Texture2D foodTexture = null;
        Texture2D foodNormal = null;

        if (state == State.Full) {
            blendshapeWeight = 0f;
            if(foodTextures[0] != null)
                foodTexture = foodTextures[0];
            if (foodNormals[0] != null)
                foodNormal = foodNormals[0];
        } else if(state == State.Half) {
            blendshapeWeight = 50f;
            if (foodTextures[1] != null)
                foodTexture = foodTextures[1];
            if (foodNormals[1] != null)
                foodNormal = foodNormals[1];
        } else if(state == State.Quarter) {
            blendshapeWeight = 100f;
            if (foodTextures[2] != null)
                foodTexture = foodTextures[2];
            if (foodNormals[2] != null)
                foodNormal = foodNormals[2];
        }

        skinnedMeshRenderer.SetBlendShapeWeight(0, blendshapeWeight);
        if(foodTexture != null)
            skinnedMeshRenderer.material.mainTexture = foodTexture;
        if(foodNormal != null)
            skinnedMeshRenderer.material.SetTexture("_BumpMap", foodNormal);
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
        //GameManager.instance.DespawnFood();
        Destroy(gameObject);
        crexBoop.movementLocked = false;
    }
}
