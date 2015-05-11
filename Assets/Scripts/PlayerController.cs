using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class PlayerController : MonoBehaviour
{
		public ScoreController scoreController;
		public float speed;
		public static float initialZSpeed = 1;
		public float zSpeed = initialZSpeed;
		public float zAcc = 1.0001f;
		public float acceleration;
		private Rigidbody rb;
		public static float distanceTraveled;
	
		void Start ()
		{
				rb = GetComponent<Rigidbody> ();
		}

		void updateSpeed ()
		{
				zSpeed *= zAcc;
				
		}

		void OnCollisionEnter (Collision other)
		{
				if (other.gameObject.name == "Obstacle(Clone)") {
						zSpeed = initialZSpeed;
						scoreController.collisionDetected (other);
				}
		}
	
		void FixedUpdate ()
		{
				float moveHorizontal = Input.GetAxis ("Horizontal");
				//float moveHorizontal = 0.0f;

				float moveVertical = Input.GetAxis ("Vertical");
				//float moveVertical = 0.0f;
		
				Vector3 movement = new Vector3 (moveHorizontal, 0.0f, zSpeed);
				rb.AddForce (movement * speed);
				//rb.MovePosition ();
				distanceTraveled = transform.localPosition.z;
				updateSpeed ();
		}
}