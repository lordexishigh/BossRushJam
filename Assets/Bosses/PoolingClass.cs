using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingClass
{
    private Queue<GameObject> objectQueue = new Queue<GameObject>();

    public PoolingClass(GameObject obj, int amount = 100)
    {
        for (int i = 0; i < amount; i++)
        {
            obj = GameObject.Instantiate(obj) as GameObject;
            obj.SetActive(false);
            objectQueue.Enqueue(obj);
        }
    }

    public GameObject DequeueObj(bool reenqueue = true, bool activate = true)
	{
        GameObject obj = objectQueue.Dequeue();
        if(reenqueue)
            objectQueue.Enqueue(obj);

        if (activate)
            obj.SetActive(true);
        return obj;
	}
}
