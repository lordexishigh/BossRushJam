using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private bool doorClosed;
    private Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        doorClosed = false;
    }

    private void OnTriggerEnter(Collider col)
	{
        if (!doorClosed && col.gameObject.tag == "Player") 
        {
            anim.Play();
            doorClosed = true;
        }
	}
}
