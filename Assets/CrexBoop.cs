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

	//[SerializeField]
    float timeTillDepressed = 45f;
    [HideInInspector]
    public float depressedTimer = 0f;

    float sadAmount = 0f;

    void Start() {
        animator = GetComponentInChildren<Animator>();
        cameraManager = FindObjectOfType<UnityARCameraManager>();
    }

    void Update() {
        depressedTimer += Time.deltaTime;
        if(depressedTimer > timeTillDepressed) {
            sadAmount = Mathf.Lerp(sadAmount, 1f, Time.deltaTime * 2f);
        } else {
            sadAmount = Mathf.Lerp(sadAmount, 0f, Time.deltaTime * 2f);
        }
        animator.SetFloat("Sad", sadAmount);

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

            transform.position += movementDir * transform.localScale.magnitude * moveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(-movementDir);

            depressedTimer = 0f;


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
        depressedTimer = 0f;

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
        if (direction == CrexDragHandler.DraggedDirection.Down)
        {
            animator.SetTrigger("Eat");
        }
        if (direction == CrexDragHandler.DraggedDirection.Tap)
        {
            animator.CrossFade("Crex_Boop", 0.25f);
        }
    }
}
