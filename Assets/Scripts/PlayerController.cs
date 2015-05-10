using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	public float acceleration=1;
	
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
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f,acceleration*0.4f);
		rb.AddForce (movement * speed);
		//rb.MovePosition ();
		distanceTraveled = transform.localPosition.z;
	}
}