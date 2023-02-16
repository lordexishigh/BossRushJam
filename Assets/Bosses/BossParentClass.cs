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

    public static PoolingClass minionObjPool;// = new Queue<GameObject>();

    protected static Transform floorTransform;

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

        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        parentTransform = transform.parent;

        if (UI.GetActiveSceneIndex() != 3)
        {
            floorTransform = GameObject.Find("Floor").GetComponent<Transform>();

            float floorScaleDivided = floorTransform.localScale.x / 4;

            for (int i = -1; i < 2; i = i + 2)
            {
                for (int j = -1; j < 2; j = j + 2)
                {
                    targetPositions.Add(transform.position + new Vector3(floorScaleDivided * i, floorTransform.position.y + floorTransform.localScale.y / 2 + 5, floorScaleDivided * j));
                }
            }
        }

		if (!miniBoss)
		{
            if (shootObjPool == null) shootObjPool = new PoolingClass(ObjectToShot);
            if (minionObjPool == null) minionObjPool = new PoolingClass(Minion);
        }

        StartFunc();
    }

    protected virtual void StartFunc() { }

    public static void SetInRange(bool state, Transform _floorTransform = null)
    {
        inRange = state;
		if (_floorTransform)
		{
            floorTransform = _floorTransform;
		}
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

