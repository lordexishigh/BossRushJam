using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
            TriangleBossAbilities.SetInRange(true);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            TriangleBossAbilities.SetInRange(false);
    }
}
