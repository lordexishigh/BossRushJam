using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarderScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			UI.RestartScene();
		}
		else
		{
			col.gameObject.SetActive(false);
		}
	}
}
