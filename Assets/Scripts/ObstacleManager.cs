using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class ObstacleManager : MonoBehaviour
{
    public ScoreController scoreController;
    public int minHeight;
    public int maxHeight;
    public int minWidth;
    public int maxWidth;
    public float zDistance = 10;
    public float minSpread = 5;
    public float maxSpread = 10;
    public float drawOffset = 20;
    public Transform playerTransform;
    public Transform obstaclePrefab;
    float zSpread;
    float destroyMargin = 2;//jak daleko od kuli gracza usuwać obiekty
    float lastZPos;
    private Queue<Transform> objectQueue;
    ObstacleGenerator generator;

    void Start()
    {
        lastZPos = Mathf.NegativeInfinity;
        zSpread = Random.Range(minSpread, maxSpread);
        objectQueue = new Queue<Transform>();
        generator = new ObstacleGenerator(null);
    }

    void Update()
    {
        
        while (objectQueue.Count>0)//przeglądaj przeszkody w celu znalezienia tych do usunięcia. kolekcja jest posortowana.
        {
            if (objectQueue.Peek().position.z + destroyMargin < playerTransform.position.z)
            {
                Destroy(objectQueue.Dequeue().gameObject);
                //tylko minięcie jednej z dwóch ścianek powoduje avoidance (nie chcemy zgłaszać minięcie lewej i minięcie prawej przeszkody, tylko minięcie dziury)
                if (objectQueue.Peek().localPosition.x < 0)
                {
                    scoreController.collisionAvoided();//dodaj punkty za usuwaną przeszkodę
                }


            } else
            {
                break;
            }
        }

        //generowanie nowej przeszkody
        if (playerTransform.position.z - lastZPos >= zSpread)//czy już generować następną przeszkodę?
        {
            ObstacleGenerator.ObstacleParams op = generator.generateParams();
            //z pos
            var zPos = playerTransform.position.z + op.distance + drawOffset;
            //first lane
            Transform left = Instantiate(obstaclePrefab, new Vector3(0, op.height / 2, zPos), Quaternion.identity) as Transform;
            var sc1 = left.localScale;
            sc1.x = op.gapStart - op.start;
            sc1.y = op.height;
            left.localScale = sc1;
            //second lane
            Transform right = Instantiate(obstaclePrefab, new Vector3(op.gapEnd, op.height / 2, zPos), Quaternion.identity) as Transform;
            var sc2 = right.localScale;
            sc2.x = op.end - op.gapEnd;
            sc2.y = op.height;
            right.localScale = sc2;
            //przeskalowanie znormalizowanych parametrów przeszkody
            left.position += new Vector3(-4.5f + left.localScale.x / 2, 0);
            right.position += new Vector3(-4.5f + right.localScale.x / 2, 0);
            objectQueue.Enqueue(left);
            objectQueue.Enqueue(right);
            lastZPos = playerTransform.position.z;
            zSpread = op.distance;
        }

    }
}
