using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMinionScript : MinionScript
{
    private Vector3 offsetDirection;
    private float offsetSign = 1;
    private float offsetTimer;

    protected override void StartFunc()
	{
        offsetDirection = transform.right;
	}

    protected override void Move()
    {
       // if (!TriangleBossAbilities.GetInRange()) return; 

		Vector3 pos = playerTransform.position;
		pos.y = transform.position.y;

        transform.LookAt(pos);

		offsetDirection = transform.right * offsetSign;

        rb.velocity = (Vector3.Normalize(pos - transform.position) + offsetDirection) * 50 + Vector3.up * rb.velocity.y;

        offsetTimer += Time.deltaTime;
        if(offsetTimer > 1)
		{
            offsetSign *= -1;
            offsetTimer = 0;
		}
    }
}
