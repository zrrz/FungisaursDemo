using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour
{
    [SerializeField]
    float knockUpForceMin = 5f;
    [SerializeField]
    float knockUpForceMax = 7f;

    Rigidbody rb;

    [SerializeField]
    GameObject shadowObject;

    float highestPosition = 0f;

    [SerializeField]
    Vector3 minSize = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    Vector3 maxSize = new Vector3(1f, 1f, 1f);

    Animator animator;

    float floorLevel = 0f;

    bool popping = false;

	void Start()
	{
        rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
        floorLevel = GameManager.instance.crexModel.transform.Find("Plane").transform.position.y + .01f;
	}

	void Update()
	{
        if (popping)
            return;
        
        if (transform.position.y > highestPosition)
            highestPosition = transform.position.y;
        Vector3 shadowPosition = transform.position;
        shadowPosition.y = floorLevel;
        shadowObject.transform.position = shadowPosition;
        shadowObject.transform.localScale = Vector3.Lerp(minSize, maxSize, Mathf.InverseLerp(highestPosition, shadowObject.transform.position.y, transform.position.y));
        shadowObject.transform.eulerAngles = new Vector3(90f, 0f, 0f);

        if (transform.position.y < floorLevel)
        {
            GameManager.instance.ToggleBalloonGame();
        }
	}

	private void OnCollisionEnter(Collision collision)
	{
        CrexBoop crexBoop = collision.gameObject.GetComponent<CrexBoop>();
        if(crexBoop != null) {
            StartCoroutine(KnockUp());
			crexBoop.Swipe(CrexDragHandler.DraggedDirection.Up);
        }
    }

    bool knockingUp = false;

    IEnumerator KnockUp() {
        if(!knockingUp) {
            yield return new WaitForSeconds(0.1f);
			Vector3 upForce = Vector3.up * Random.Range(knockUpForceMin, knockUpForceMax);
			Vector3 xzForce = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * Random.Range(150f, 350f);
			rb.AddTorque(Random.onUnitSphere * Random.Range(0.8f, 1.25f));
			GameManager.instance.AddBalloonPoint();
			animator.Play("BalloonHit", 0);
            for (int i = 0; i < 10; i++) {
                rb.AddForce((upForce + xzForce)/7f);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }

    public void PlayPopAnimationAndDestroy() {
        popping = true;
        animator.Play("BalloonPop", 0);
		rb.angularVelocity = Vector3.zero;
        Destroy(gameObject, 1f);
    }
}
