using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrexBoop : MonoBehaviour {

    public SingleJoystick singleJoystick;
    public Transform myRotationObject; // The object we will be rotating when moving
    public float moveSpeed = 6.0f; // Character movement speed.
    public int rotationSpeed = 8; // How quick the character rotate to target location.
    Vector3 input01;

    Animator animator;

    UnityARCameraManager cameraManager;

    void Start() {
        animator = GetComponentInChildren<Animator>();
        cameraManager = FindObjectOfType<UnityARCameraManager>();
    }

    void Update() {
        transform.SetSiblingIndex(0);
        // get input from both joysticks
        input01 = singleJoystick.GetInputDirection();

//        Debug.Log(singleJoystick.GetInputDirection());

        if (animator != null)
        {
            animator.SetFloat("MoveSpeed", Mathf.Max(Mathf.Abs(input01.x), Mathf.Abs(input01.y)));
        }

        if (input01 != Vector3.zero)
        {
            Vector3 movementDir = cameraManager.m_camera.transform.TransformDirection(new Vector3(input01.x, 0, input01.y));
            movementDir.y = 0f;

            transform.position += movementDir * moveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(-movementDir);



//            //Move player the same distance in each direction. Player must move in a circular motion.
//
//            float tempAngle = Mathf.Atan2(zMovementInput01, xMovementInput01);
//            xMovementInput01 *= Mathf.Abs(Mathf.Cos(tempAngle));
//            zMovementInput01 *= Mathf.Abs(Mathf.Sin(tempAngle));
//
//            input01 = new Vector3(xMovementInput01, 0, zMovementInput01);
//            input01 = transform.TransformDirection(input01);
//            input01 *= moveSpeed;
//
//            // Make rotation object(The child object that contains animation) rotate to direction we are moving in.
//            Vector3 temp = transform.position;
//            temp.x += xMovementInput01;
//            temp.z += zMovementInput01;
//            Vector3 lookingVector = temp - transform.position;
//            if (lookingVector != Vector3.zero)
//            {
//                myRotationObject.localRotation = Quaternion.Slerp(myRotationObject.localRotation, Quaternion.LookRotation(lookingVector), rotationSpeed * Time.deltaTime);
//            }


//            transform.Translate(input01 * Time.fixedDeltaTime);
        }
    }

    public void Swipe(CrexDragHandler.DraggedDirection direction) {
        if (direction == CrexDragHandler.DraggedDirection.Left)
        {
            animator.SetTrigger("SwipeLeft");
        }
        if (direction == CrexDragHandler.DraggedDirection.Right)
        {
            animator.SetTrigger("SwipeRight");
        }
        if (direction == CrexDragHandler.DraggedDirection.Up)
        {
            animator.SetTrigger("SwipeUp");
        }
        if (direction == CrexDragHandler.DraggedDirection.Tap)
        {
            animator.CrossFade("Crex_Boop", 0.25f);
        }
    }
}
