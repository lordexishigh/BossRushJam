using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : MonoBehaviour
{
    private static Transform playerTransform;
    private NavMeshAgent agent;
    private GameObject ammoGO;
    private Rigidbody rb;
    private ParticleSystem _particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        _particleSystem = transform.parent.Find("ParticleSystem").gameObject.GetComponent<ParticleSystem>();
        ammoGO = transform.parent.Find("AmmoableSquare").Find("SquareFunctionality").gameObject;

        if (!playerTransform)
		{
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        agent.destination = playerTransform.position;
    }

    private void OnCollisionEnter(Collision col)
	{
        GameObject temp = col.gameObject;
        if (temp.tag == "Player")
		{
			if (temp.GetComponent<PlayerMovement>().GetCharged())
			{
                ammoGO.SetActive(true);
                ammoGO.GetComponent<Rigidbody>().velocity = rb.velocity;
                ammoGO.transform.position = transform.position;
                ammoGO.transform.rotation = transform.rotation;
                gameObject.SetActive(false);
			}
			else
			{
                StartCoroutine(MinionExplosion());
            }
        }

        if(temp.tag == "PlayerBullet")
		{
          //  AmmoableScript ammoScript = temp.GetComponent<AmmoableScript>();

           // if (ammoScript.GetCollided() && ammoScript.GetAmmo())
           
            TurnToAmmo();
			
		}
	}

    private IEnumerator MinionExplosion()
	{
        Vector3 scaleVec = transform.localScale / 20;
        for(int i = 0; i < 5; i++)
		{
            transform.localScale += scaleVec;
            yield return new WaitForSeconds(0.05f);
        }
        
        _particleSystem.Play();
        transform.localScale = scaleVec * 20;
        transform.parent.gameObject.SetActive(false);

        //yield return new WaitForSeconds(1.5f);

       
    }

    private void TurnToAmmo()
	{
        ammoGO.SetActive(true);
       // ammoGO.GetComponent<Rigidbody>().velocity = rb.velocity;
        ammoGO.transform.position = transform.position;
        ammoGO.transform.rotation = transform.rotation;
        gameObject.SetActive(false);
	}

    private void OnDestroy()
	{
        if (playerTransform)
            playerTransform = null;
	}
}
