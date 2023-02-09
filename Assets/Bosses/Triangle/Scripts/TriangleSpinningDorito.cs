using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleSpinningDorito : ProjectileScript
{
    private static float maxTimeLife = 10;
    private Vector3 startingScale;
    private Vector3 scaleMod;

    private float timeAlive;

    private void Start()
	{
        startingScale = transform.localScale;
        scaleMod = startingScale / 20;
	}

    void OnEnable()
	{
        timeAlive = 0;
        if(startingScale != Vector3.zero)
            transform.localScale = startingScale;
	}

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if(timeAlive > maxTimeLife) { timeAlive = 0; StartCoroutine(TimeIsUp()); }
    }

    private IEnumerator TimeIsUp()
	{
        for (int i = 0; i < 20; i++)
		{
            transform.localScale = transform.localScale - scaleMod;
            yield return new WaitForSeconds(0.05f);
		}
        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
	}

    protected override void OnCollisionEnter(Collision col)
	{
        if (col.gameObject.tag != "Player") return;

        StartCoroutine(DeactivateProjectile());
		
	}
}
