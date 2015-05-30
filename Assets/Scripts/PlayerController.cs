﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp;

/**
 * Trasa jest generowana automatycznie,
 * gracz w trybie emocjonalnym ma zmienną kulkę (sterowalność i rozmiar) w zależności od /collisions in last 10 seconds/
 * a w trybie bez emocji może wybrać sterowalność i rozmiar kulki
 * 
 */
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
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void updateSpeed()
    {
        zSpeed *= zAcc;
                
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Obstacle(Clone)")
        {
            zSpeed = initialZSpeed;
            scoreController.collisionDetected(other);
            rb.AddExplosionForce(140 * zSpeed, other.contacts [0].point, 0);
        }
    }
    
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveHorizontal = 0.0f;

        float moveVertical = Input.GetAxis("Vertical");
        //float moveVertical = 0.0f;
        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, zSpeed);
        rb.AddForce(movement * speed);
        //rb.MovePosition ();
        distanceTraveled = transform.localPosition.z;
        updateSpeed();
    }
}