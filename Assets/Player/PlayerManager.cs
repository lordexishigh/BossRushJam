using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private float health = 3;
    private PlayerMovement mov;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Harmful" && !mov.GetCharged())
        {
            print("hit");
            health -= 1;
        }
	}
}
