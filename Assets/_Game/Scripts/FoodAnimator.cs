using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodAnimator : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler   {

    public enum State {
        Full, Half, Quarter
    }

    [SerializeField]
    Texture2D[] foodTextures;
    [SerializeField]
    Texture2D[] foodNormals;

    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;

	public bool eating = false;

    //void Awake () {
        
    //}

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogError("BeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.LogError("OnDrag");
        if(!eating) {
            FeedModeManager.instance.DragFood(gameObject, eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.LogError("OnEndDrag");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.LogError("OnPointerUp");
    }

    public void SetState(State state) {
        float blendshapeWeight = 0f;
        Texture2D foodTexture = null;
        Texture2D foodNormal = null;

        if (state == State.Full) {
            blendshapeWeight = 0f;
            if(foodTextures.Length >= 1 && foodTextures[0] != null)
                foodTexture = foodTextures[0];
            if(foodNormals.Length >= 1 && foodNormals[0] != null)
                foodNormal = foodNormals[0];
        } else if(state == State.Half) {
            blendshapeWeight = 50f;
            if (foodTextures.Length >= 2 && foodTextures[1] != null)
                foodTexture = foodTextures[1];
            if (foodNormals.Length >= 2 && foodNormals[1] != null)
                foodNormal = foodNormals[1];
        } else if(state == State.Quarter) {
            blendshapeWeight = 100f;
            if (foodTextures.Length >= 3 && foodTextures[2] != null)
                foodTexture = foodTextures[2];
            if (foodNormals.Length >= 3 && foodNormals[2] != null)
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
        FungisaurController crexBoop = collision.gameObject.GetComponent<FungisaurController>();
        EatFood(crexBoop);
    }

    public void EatFood(FungisaurController crexBoop) {
        if (crexBoop != null)
        {
            if (!eating)
                StartCoroutine(Eat(crexBoop));
        } 
    }


    IEnumerator Eat(FungisaurController crexBoop) {
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
