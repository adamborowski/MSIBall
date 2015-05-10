using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundManager : MonoBehaviour {

	public Transform prefab;
	public int numberOfObjects;
	public float recycleOffset;
	public Vector3 startPosition;
	
	private Vector3 nextPosition;
	private Queue<Transform> objectQueue;
	
	void Start () {
		objectQueue = new Queue<Transform>(numberOfObjects);
		for (int i = 0; i < numberOfObjects; i++) {
			objectQueue.Enqueue((Transform)Instantiate(prefab));
		}
		nextPosition = startPosition;
		for (int i = 0; i < numberOfObjects; i++) {
			Recycle();
		}

	}

	void Update () {
		if (objectQueue.Peek().localPosition.z + recycleOffset < PlayerController.distanceTraveled) {
			Recycle();
		}
	}

	private void Recycle () {
		Transform o = objectQueue.Dequeue();
		o.localPosition = nextPosition;
		nextPosition.z += 10;
		objectQueue.Enqueue(o);
	}
}
