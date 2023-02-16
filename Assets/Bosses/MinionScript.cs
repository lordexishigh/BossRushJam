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

    [SerializeField]
    protected NavMeshAgent agent;

    [SerializeField]
    protected Rigidbody rb;

    protected static Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        if(!agent) agent = GetComponent<NavMeshAgent>();
        if(!rb) rb = GetComponent<Rigidbody>();

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
        if (agent.enabled == true) 
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
                Transform trans = ammoGO.transform;
                trans.position = transform.position;
                trans.rotation = transform.rotation;
                trans.localScale = transform.localScale;
                ammoGO.GetComponent<Rigidbody>().velocity = rb.velocity;
                gameObject.SetActive(false);
			}
			else
			{
                StartCoroutine(MinionExplosion());
            }
        }

        else if(temp.tag == "PlayerBullet")
		{
            TurnToAmmo();	
		}

        CollisionFunc(col);
    }

    protected virtual void CollisionFunc(Collision col) { } 

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
