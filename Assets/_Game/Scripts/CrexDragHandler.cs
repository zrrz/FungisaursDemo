using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrexDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [SerializeField]
    FungisaurController crexBoop;

    public void OnPointerDown(PointerEventData eventData) {
        //Have to implement or OnPointerUp doesnt get called
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!FungisaurManager.instance.fungisaurPlaced || GameManager.instance.gameMode == GameManager.GameMode.Options)
            return;
        
        //Debug.Log("Press position + " + eventData.pressPosition);
        //Debug.Log("End position + " + eventData.position);
        //Debug.LogError((eventData.position - eventData.pressPosition).magnitude);
        if ((eventData.position - eventData.pressPosition).magnitude < 25f)
        {
            DraggedDirection direction = DraggedDirection.Tap;
            if (crexBoop == null)
                FungisaurManager.instance.GetCurrentFungisaur().GetComponent<FungisaurController>().Swipe(direction);
            else
                crexBoop.Swipe(direction);
        }
        else
        {
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            //Debug.Log("norm + " + dragVectorDirection);
            DraggedDirection direction = GetDragDirection(dragVectorDirection);
            if (crexBoop == null)
                FungisaurManager.instance.GetCurrentFungisaur().GetComponent<FungisaurController>().Swipe(direction);
            else
                crexBoop.Swipe(direction);
        }
    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//    }
//
//    public void OnDrag(PointerEventData eventData)
//    {
//    }
//
//    public void OnEndDrag(PointerEventData eventData)
//    {
//        
//    }

    public enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left,
        Tap
    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }
}
