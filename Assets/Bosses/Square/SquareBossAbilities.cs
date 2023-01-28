using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBossAbilities : MonoBehaviour
{
    [SerializeField]
    private GameObject ObjectToShot;

    [SerializeField]
    private GameObject Minion;

    private Transform playerTransform;

    private List<Vector3> targetPositions = new List<Vector3>();

    private Queue<Transform> shootObjTransQueue = new Queue<Transform>();

    private Queue<GameObject> minionGOQueue = new Queue<GameObject>();

    private Transform floorTransform;

    private int amountOfAbilities = 3;

    private bool abilityActive = false;

    public static bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        floorTransform = GameObject.Find("Floor").GetComponent<Transform>();

        float floorScaleDivided = floorTransform.localScale.x / 4;
        for(int i = -1; i < 2; i = i + 2)
		{
            for(int j = -1; j < 2; j = j + 2)
			{
                targetPositions.Add(new Vector3(floorScaleDivided * i, floorTransform.position.y + floorTransform.localScale.y / 2 + 5, floorScaleDivided * j));
			}
		}

        GameObject obj;

        for (int i = 0; i < 200; i++)
		{
            obj = Instantiate(ObjectToShot, transform.position, Quaternion.identity);
            obj.SetActive(false);
            shootObjTransQueue.Enqueue(obj.GetComponent<Transform>());

            obj = Instantiate(Minion, transform.position, Quaternion.identity);
            obj.SetActive(false);
            minionGOQueue.Enqueue(obj);
        }

        //StartCoroutine(AimLockShoot(500));
        //StartCoroutine(RainObjects(50));
        //StartCoroutine(SpinRandomShooting(50));
        StartCoroutine(MinionSpawn(2));
    }

    private void Update()
	{
        if (abilityActive || !inRange) return;
		
        switch(Random.Range(0, amountOfAbilities))
		{
            case 0:
                StartCoroutine(AimLockShoot());
                break;
            case 1:
                StartCoroutine(SpinRandomShooting());
                break;
            case 2:
                StartCoroutine(RainObjects());
                break;
            default:
                print("Shit gone wrong");
                break;
		}
	}

    private IEnumerator MinionSpawn(int waves = 1, int minionsPerWave = 4)
	{
        abilityActive = true;
        GameObject obj;

        Vector3 spawnPos;

        int locNumber;

        for (int i = 0; i < waves; i++)
		{
            locNumber = Random.Range(0, targetPositions.Count);

            for(int j = 0; j < minionsPerWave; j++)
			{
                obj = minionGOQueue.Dequeue();

                spawnPos = targetPositions[locNumber] + 
                    new Vector3(Random.Range(0, floorTransform.localScale.x / 5), 5, Random.Range(0, floorTransform.localScale.x / 5));
                    
                
                obj.transform.position = spawnPos;
                obj.SetActive(true);
            }
            yield return new WaitForSeconds(3f);
		}

        abilityActive = false;
    }

    private IEnumerator AimLockShoot(int amount = 10)
	{
        abilityActive = true;

        for (int i = 0; i < amount; i++)
        {
            transform.LookAt(playerTransform);
            ShootSquare(transform.rotation);            

            yield return new WaitForSeconds(0.75f);
        }
        abilityActive = false;
    }

    private IEnumerator SpinRandomShooting(int amount = 50, float degreePerShot = 15)
	{
        abilityActive = true;

        float degree = 0;

        for (int i = 0; i < amount; i++)
        {
            transform.LookAt(playerTransform);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * degree);
            for (int j = 0; j < 4; j++)
            {
                ShootSquare(Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * j * 90));
            }

            degree += degreePerShot;

            yield return new WaitForSeconds(0.35f);
        }
        abilityActive = false;
    }

    private IEnumerator RainObjects(int amount = 4)
	{
        abilityActive = true;

        int centerLocationIndex = 0;
        Transform trans;
        GameObject tempGO;

        float scale = floorTransform.localScale.x / 10;

        for (int i = 0; i < amount; i++)
        {
            int blocksToActivate = Random.Range(1, 3);
            for (int b = 0; b < blocksToActivate; b++)
            {
                centerLocationIndex = Random.Range(0, targetPositions.Count);

                for (int j = -2; j < 3; j++)
                {
                    for (int k = -2; k < 3; k++)
                    {
                        trans = shootObjTransQueue.Dequeue();

                        tempGO = trans.gameObject;
                        tempGO.SetActive(true);

                        trans.position = targetPositions[centerLocationIndex] + new Vector3(j * scale, 100, k * scale);

                        tempGO.GetComponent<Rigidbody>().velocity = Vector3.down * 50;

                        shootObjTransQueue.Enqueue(trans);
                    }
                }
            }
            yield return new WaitForSeconds(2);
        }
        abilityActive = false;
    }

    private void ShootSquare(Quaternion angle)
    {
        Transform trans;
        GameObject tempGO;

        trans = shootObjTransQueue.Dequeue();

        tempGO = trans.gameObject;
        tempGO.SetActive(true);

        trans.rotation = angle;

        trans.position = transform.position + trans.forward * transform.parent.localScale.x * 2.1f;

        tempGO.GetComponent<Rigidbody>().velocity = (trans.forward * 3 + trans.up / 6) * Vector3.Distance(trans.position, playerTransform.position);
        trans.eulerAngles = trans.eulerAngles - Vector3.right * trans.eulerAngles.x;
        shootObjTransQueue.Enqueue(trans);
    }

    private void OnTriggerEnter(Collider col)
	{
        if (col.transform == playerTransform)
            inRange = true;
	}

    private void OnTriggerExit(Collider col)
    {
        if (col.transform == playerTransform)
            inRange = false;
    }
}