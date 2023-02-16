using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeScript : MonoBehaviour
{
	[SerializeField]
    private bool passFloor;

    private Transform floorTransform;

    private void Start()
	{
		if (passFloor)
		{
            floorTransform = gameObject.transform;
		}
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            BossParentClass.SetInRange(true , floorTransform);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            BossParentClass.SetInRange(false, floorTransform);
    }
}
