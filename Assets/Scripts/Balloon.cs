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

    float gravityScale = 1f;

	void Start()
	{
        rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
        floorLevel = GameManager.instance.GetCurrentFungisaur().transform.Find("Plane").transform.position.y + .01f;
        Physics.gravity = new Vector3(0f, -9.81f, 0f);
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
            GameManager.instance.EndBalloonGame();
        }
	}

	private void OnCollisionEnter(Collision collision)
	{
        CrexBoop crexBoop = collision.gameObject.GetComponent<CrexBoop>();
        if(crexBoop != null) {
            StartCoroutine(KnockUp());
            AudioManager.instance.Play("BalloonBounce");
            crexBoop.Swipe(CrexDragHandler.DraggedDirection.Up);
        }
    }

    //Vector3 velocity = Vector3.zero;

	//private void FixedUpdate()
	//{
 //       velocity += 

 //       GetComponent<Rigidbody>().position += velocity*Time.fixedDeltaTime;
	//}

	bool knockingUp = false;

    IEnumerator KnockUp() {
        float xzRangeMin = 150f;
        float xzRangeMax = 350f;

        int ballonGameScore = GameManager.instance.balloonGameScore;

        if(ballonGameScore > 5) {
            xzRangeMin = 150f * (1f + ballonGameScore * 0.07f);
            xzRangeMax = 350f * (1f + ballonGameScore * 0.1f);
        }
        if(ballonGameScore > 10) {
            gravityScale = 1f + (ballonGameScore * 0.1f);
            Physics.gravity = new Vector3(0f, -9.81f, 0f) * gravityScale;
        }

        if(!knockingUp) {
            yield return new WaitForSeconds(0.1f);
            Vector3 upForce = Vector3.up * Random.Range(knockUpForceMin, knockUpForceMax) * (1f + ballonGameScore * 0.03f);
            Vector3 xzForce = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * Random.Range(xzRangeMin, xzRangeMax);
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
