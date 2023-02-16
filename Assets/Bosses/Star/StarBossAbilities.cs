using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBossAbilities : BossParentClass
{
    [SerializeField]
    private GameObject miniBossGO;

    [SerializeField]
    private GameObject ammoableStar;

    private static PoolingClass ammoableObjPool;

    private GameObject[] floorsAvailable;

    private static int activeFloorIndex = 0;

    private bool skip = true;

    static Vector3[] directions = new Vector3[] {
        Vector3.right, Vector3.left, Vector3.forward, Vector3.back,
        (Vector3.back + Vector3.left) / 3, (Vector3.back + Vector3.right) / 3,
        (Vector3.forward + Vector3.left) / 3, (Vector3.forward + Vector3.right) / 3,  
        Vector3.zero 
    };

    protected override void StartFunc()
    {
        floorsAvailable = GameObject.FindGameObjectsWithTag("Floor");
        if (!miniBoss) ammoableObjPool = new PoolingClass(ammoableStar, 50);
        StartCoroutine(SpawnMinions());
    }

    private void Update()
	{
        if (abilitiesActive >= maxAbilitiesActive || !inRange) return;

        switch (Random.Range(0, 2))
        {
            case 0:
                StartCoroutine(DroppingStars());
                break;
            case 1:
                StartCoroutine(FireStars());
                break;
            //case 2:
            //    StartCoroutine(RainObjects());
            //    break;
            //case 3:
            //    StartCoroutine(SmashPlayer(4));
            //    break;
            default:
                print("Shit gone wrong");
                break;
        }
    }

    private IEnumerator DroppingStars(int waves = 6)
	{
        abilitiesActive++;

        GameObject obj;
        Rigidbody rb;
        Vector3 pos;

        for(int i = 0; i < waves; i++)
		{
            pos = playerTransform.position + Vector3.up * 100;

            foreach(Vector3 dir in directions)
            {
                obj = shootObjPool.DequeueObj();
                obj.transform.position = pos + dir * 100;
                rb = obj.GetComponent<Rigidbody>();
                rb.velocity = Vector3.down * 50;
                rb.angularVelocity = Vector3.one * Random.Range(50, 100);
            }

            yield return new WaitForSeconds(2);
        }
        yield return new WaitForSeconds(2);
        abilitiesActive--;
    }

    private IEnumerator FireStars(int waves = 60)
	{
		abilitiesActive++;

		GameObject obj;
        Transform trans;
		Vector3 pos;

		for (int i = 0; i < waves; i++)
		{
			pos = playerTransform.position + Vector3.up * 100;

            for (int j = 0; j < 4; j++)
			{
                obj = shootObjPool.DequeueObj();
                trans = obj.transform;
                trans.position = pos + directions[Random.Range(0, directions.Length - 2)] * 200;
                trans.LookAt(playerTransform);
                obj.GetComponent<Rigidbody>().velocity = trans.forward * 200;
            }
            yield return new WaitForSeconds(2);
		}
        yield return new WaitForSeconds(2);
        abilitiesActive--;
	}

    private IEnumerator SpawnMinions(int waves = 1)
	{
        if(miniBoss) { yield return new WaitForSeconds(5f); }
        while (getMiniBossAlive()) { yield return new WaitForSeconds(5f); }

        int dirLength = directions.Length - 2 - (miniBoss ? 4 : 0);

        activeFloorIndex = Random.Range(0, floorsAvailable.Length);

        Vector3 floorPos = floorsAvailable[activeFloorIndex].transform.position + Vector3.up * 100;

        for (int j = 0; j < dirLength; j++)
        {
            minionObjPool.DequeueObj().transform.position = floorPos + directions[j] * 200;
        }

        if(!miniBoss && !skip) 
        {
            //GameObject obj = Instantiate(transform.parent.gameObject, , Quaternion.identity);
            miniBossGO.SetActive(true);
            miniBossGO.transform.position = floorPos + Vector3.up * 40;
        }
        else { skip = false; }

        yield return new WaitForSeconds(Random.Range(20, 30));
        StartCoroutine(SpawnMinions());
    }

    public static GameObject getAmmoableStar()
	{
        return ammoableObjPool.DequeueObj();
	}

    public bool getMiniBossAlive()
	{
        return miniBossGO.activeSelf;
	}

    public static int getActiveFloorIndex()
	{
        return activeFloorIndex;
	}
}
