using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Stack<GameObject> shootableObjects = new Stack<GameObject>();

    public void AddShootableObject(GameObject obj)
	{
        shootableObjects.Push(obj);
	}

    public void ShootObject(float velocityMod = 1)
	{
        if (shootableObjects.Count < 1) return;

        if (velocityMod < 1) velocityMod = 1;

        GameObject tempObj = shootableObjects.Pop();

        tempObj.transform.position = transform.position + transform.forward + transform.up;

        tempObj.GetComponent<AmmoableScript>().ChangeObjectState(2);

        tempObj.GetComponent<Rigidbody>().velocity = 50 * velocityMod / 2 * (transform.forward * 4f + transform.up  * velocityMod / 2);//AddForce(1000 * (/*transform.forward +*/transform.up));
    }

    public int ammoAmount()
	{
        return shootableObjects.Count;
	}
}
