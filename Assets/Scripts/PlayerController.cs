using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	
	private Rigidbody rb;
	public static float distanceTraveled;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		//float moveHorizontal = 0.0f;

		float moveVertical = Input.GetAxis ("Vertical");
		//float moveVertical = 0.0f;
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		
		rb.AddForce (movement * speed);

		distanceTraveled = transform.localPosition.z;
	}
}