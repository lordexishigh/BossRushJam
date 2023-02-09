using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossParentClass : MonoBehaviour
{
    [SerializeField]
    protected GameObject ObjectToShot;

    [SerializeField]
    protected GameObject Minion;

    [SerializeField]
    protected bool miniBoss = false;

    protected List<Vector3> targetPositions = new List<Vector3>();

    protected static PoolingClass shootObjPool;// = new Queue<GameObject>();

    protected static PoolingClass minionObjPool;// = new Queue<GameObject>();

    protected Transform floorTransform;

    protected Transform playerTransform;

    protected Transform parentTransform;

    protected int abilitiesActive;

    protected int maxAbilitiesActive;

    protected static bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        abilitiesActive = 0;
        maxAbilitiesActive = 1;

        floorTransform = GameObject.Find("Floor").GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        parentTransform = transform.parent;

        float floorScaleDivided = floorTransform.localScale.x / 4;

        for (int i = -1; i < 2; i = i + 2)
        {
            for (int j = -1; j < 2; j = j + 2)
            {
                targetPositions.Add(transform.position + new Vector3(floorScaleDivided * i, floorTransform.position.y + floorTransform.localScale.y / 2 + 5, floorScaleDivided * j));
            }
        }

		if (!miniBoss)
		{
            if (shootObjPool == null) shootObjPool = new PoolingClass(ObjectToShot, transform.position);
            if (minionObjPool == null) minionObjPool = new PoolingClass(Minion, transform.position);
        }

        //GameObject obj;

        //for (int i = 0; i < 200; i++)
        //{
        //    obj = Instantiate(ObjectToShot, transform.position, Quaternion.identity);
        //    obj.SetActive(false);
        //    shootObjQueue.Enqueue(obj);

        //    obj = Instantiate(Minion, transform.position, Quaternion.identity);
        //    obj.SetActive(false);
        //    minionGOQueue.Enqueue(obj);
        //}

        StartFunc();
    }

    protected virtual void StartFunc() { }

    public static void SetInRange(bool state)
    {
        inRange = state;
    }

    public static bool GetInRange()
    {
        return inRange;
    }

    public bool GetMiniBoss()
    {
        return miniBoss;
    }

    public void SetMaxAbilities(int amount = 1)
    {
        maxAbilitiesActive += amount;
    }
}

