using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleBullet : ProjectileScript
{
    //https://www.youtube.com/watch?v=Z6qBeuN-H1M

    private static Transform playerT;
    private static float speed;
    private static float rotateSpeed;

    private Rigidbody rb;

    private float timeSpawned;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!playerT) playerT = GameObject.FindWithTag("Player").GetComponent<Transform>();

        if (speed == 0) speed = 150;

        if (rotateSpeed == 0) rotateSpeed = 100;
    }

    private void OnEnable()
	{
        timeSpawned = Time.time;
	}

    private void FixedUpdate()
    {
        timeSpawned += Time.deltaTime;
        if (0.5f > timeSpawned) return;
        else if(timeSpawned > 10) { gameObject.SetActive(false); }

        rb.velocity = transform.forward * speed;
        RotateRocket();
    }

    private void RotateRocket()
    {
        Vector3 direction = playerT.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }

 //   private void OnCollisionEnter(Collision collision)
	//{
 //       ga
	//}
}
   

