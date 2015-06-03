using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundManager : MonoBehaviour {

	public Transform prefab; // prosty fragment trasy (bez przeszkod) do powielania
	public int numberOfObjects;
	public float recycleOffset;
	public Vector3 startPosition;
	
	private Vector3 nextPosition;
	private Queue<Transform> objectQueue;
	
    /*
     * Generator nieskonczonej trasy
     * Manager odpowiada za tryb 'endless runner'
     * w miare pokonywania koljnych odcinkow generowane sa kolejny by plansza nigdy sie nie skonczyla
     */

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
        //funkcja sprawdza czy dodac koljny element trasy w zaleznosci od pokonanej odleglosci
		if (objectQueue.Peek().localPosition.z + recycleOffset < PlayerController.distanceTraveled) {
			Recycle();
		}
	}

	private void Recycle () {

        // by uniknac usuwania i tworzenia nowych obiektow czesci trasy ktore minelismy przenoszone sa do przodu
		Transform o = objectQueue.Dequeue();
		o.localPosition = nextPosition;
		nextPosition.z += 10;
		objectQueue.Enqueue(o);
	}
}
