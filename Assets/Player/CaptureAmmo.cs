using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureAmmo : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
	{
        if (col.gameObject.tag != "Ammoable") return;

        col.gameObject.GetComponent<AmmoableScript>().ChangeObjectState(1);
    }
}
