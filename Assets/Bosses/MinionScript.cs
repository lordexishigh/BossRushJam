using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionScript : MonoBehaviour
{
    [SerializeField]
    protected GameObject ammoGO;

    [SerializeField]
    protected ParticleSystem _particleSystem;

    protected static Transform playerTransform;

    protected NavMeshAgent agent;
    protected Rigidbody rb;
  
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        if (!playerTransform)
		{
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        StartFunc();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected virtual void StartFunc() { }

    protected virtual void Move()
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
                ammoGO.transform.position = transform.position;
                ammoGO.transform.rotation = transform.rotation;
                ammoGO.GetComponent<Rigidbody>().velocity = rb.velocity;
                gameObject.SetActive(false);
			}
			else
			{
                StartCoroutine(MinionExplosion());
            }
            return;
        }

        if(temp.tag == "PlayerBullet")
		{
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
