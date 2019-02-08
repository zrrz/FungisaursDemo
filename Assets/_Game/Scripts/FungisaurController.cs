using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungisaurController : MonoBehaviour {

    public SingleJoystick singleJoystick;
    public float moveSpeed = 6.0f; // Character movement speed.
    public int rotationSpeed = 8; // How quick the character rotate to target location.
    //Vector3 input01;

    Animator animator;

    UnityARCameraManager cameraManager;

    [SerializeField]
    bool getsSad = true;

	//[SerializeField]
    float timeTillDepressed = 20f;
    [HideInInspector]
    public float depressedTimer = 0f;

    float sadAmount = 0f;

    public bool movementLocked = false;

    public Transform foodSpawnLocation;
    public Transform mouthTransform;

    public float moveSpeedAnimationMod = 1f;

    public string roarSound = "CrexRoarTap0";

    public enum MovementMode { Manual, Targeted, Stationary }

    public MovementMode movementMode = MovementMode.Manual;

    Transform targetObject;
    Vector3 lookAtPosition;

    void Start() {
        animator = GetComponentInChildren<Animator>();
        cameraManager = FindObjectOfType<UnityARCameraManager>();
    }

    void Update() {
        transform.SetSiblingIndex(0); //HACK Why am I still doing this?

        if(movementMode == MovementMode.Manual) {
            ManualMovement();
        } else if(movementMode == MovementMode.Targeted) {
            TargetedMovement();
        } else if(movementMode == MovementMode.Stationary) {
            StationaryMovement();
        }
    }

    void StationaryMovement() {
        Vector3 lookDir = (lookAtPosition - transform.position).normalized;
        lookDir.y = 0f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-lookDir), Time.deltaTime*2f);
    }

    void TargetedMovement() {

        if(targetObject == null) {
            animator.SetFloat("MoveSpeed", 0f);
            return;
        }

        //Distance check done in FetchGameManager

        Vector3 moveDir = (targetObject.position - transform.position).normalized;
        moveDir.y = 0f;

        if (animator != null)
        {
            animator.SetFloat("MoveSpeed", Mathf.Max(Mathf.Abs(moveDir.x), Mathf.Abs(moveDir.z)) * moveSpeedAnimationMod);
        }

        if (moveDir != Vector3.zero)
        {
            //Vector3 movementDir = cameraManager.m_camera.transform.TransformDirection(new Vector3(input.x, 0, input.y));
            //movementDir.y = 0f;

            transform.position += moveDir * transform.localScale.magnitude * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(-moveDir);

            depressedTimer = 0f;
        }
    }

    void ManualMovement()
    {
        if (movementLocked)
        {
            animator.SetFloat("MoveSpeed", 0f);
            return;
        }
        if (getsSad)
        {
            depressedTimer += Time.deltaTime;
            if (depressedTimer > timeTillDepressed)
            {
                sadAmount = Mathf.Lerp(sadAmount, 1f, Time.deltaTime * 2f);
            }
            else
            {
                sadAmount = Mathf.Lerp(sadAmount, 0f, Time.deltaTime * 2f);
            }
            animator.SetFloat("Sad", sadAmount);
        }


        // get input from both joysticks
        Vector3 input = singleJoystick.GetInputDirection();

        //        Debug.Log(singleJoystick.GetInputDirection());

        if (animator != null)
        {
            animator.SetFloat("MoveSpeed", Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y)) * moveSpeedAnimationMod);
        }

        if (input != Vector3.zero)
        {
            Vector3 movementDir = cameraManager.m_camera.transform.TransformDirection(new Vector3(input.x, 0, input.y));
            movementDir.y = 0f;

            transform.position += movementDir * transform.localScale.magnitude * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(-movementDir);

            depressedTimer = 0f;
        }
    }

    public void SetTargetObject(Transform target)
    {
        targetObject = target;
    }

    public void SetLookAtPosition(Vector3 position) {
        lookAtPosition = position;
    }

    public void SetMovementMode(MovementMode movementMode) {
        this.movementMode = movementMode;
    }

    public void Swipe(CrexDragHandler.DraggedDirection direction) {
        depressedTimer = 0f;

        if (direction == CrexDragHandler.DraggedDirection.Left)
        {
            animator.SetTrigger("SwipeLeft");
            AudioManager.instance.Play("TailSwoosh0");
        }
        if (direction == CrexDragHandler.DraggedDirection.Right)
        {
            animator.SetTrigger("SwipeRight");
            AudioManager.instance.Play("TailSwoosh0");
        }
        if (direction == CrexDragHandler.DraggedDirection.Up)
        {
            animator.SetTrigger("SwipeUp");
            AudioManager.instance.Play("JumpLanding0");
        }
        if (direction == CrexDragHandler.DraggedDirection.Down)
        {
            animator.SetTrigger("Eat");
            AudioManager.instance.Play("Biting0");
        }
        if (direction == CrexDragHandler.DraggedDirection.Tap)
        {
            animator.CrossFade("Crex_Boop", 0.25f);
            AudioManager.instance.Play(roarSound);
        }
    }
}
