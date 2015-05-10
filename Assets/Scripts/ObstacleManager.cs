using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

	public int obstaclePerGround;
	public int minHeight;
	public int maxHeight;
	public int minWidth;
	public int maxWidth;


	public float zDistance = 10;
	public float minSpread = 5;
	public float maxSpread = 10;
	
	public Transform playerTransform;
	public Transform obstaclePrefab;
	
	float zSpread;
	float lastZPos;



	void Start () {
		lastZPos = Mathf.NegativeInfinity;
		zSpread = Random.Range(minSpread, maxSpread);
	}

	void Update () {
		if(playerTransform.position.z - lastZPos >= zSpread){
			float lanePos = Random.Range(0, 3);
			lanePos = (lanePos-1)*1.5f;
			Instantiate(obstaclePrefab, new Vector3(lanePos, 0.5f, playerTransform.position.z + zDistance), Quaternion.identity);
			
			lastZPos = playerTransform.position.z;
			zSpread = Random.Range(minSpread, maxSpread);
		}
	}
}
