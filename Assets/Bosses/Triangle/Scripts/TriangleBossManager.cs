using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleBossManager : BossManagerParentClass
{
	[SerializeField]
    private GameObject miniBosses;
   // private TriangleBossAbilities tAbilities;

    protected override void StartFunc()
    {
  //      tAbilities = GetComponent<TriangleBossAbilities>();
		//if (tAbilities.GetMiniBoss()) { health = 50; }
  //      else health = 100;
    }

    protected void OnEnable()
	{
        if (!healthSlider) return;
        healthSlider.gameObject.SetActive(true);
	}

    protected void OnDisable()
	{
        if (!healthSlider) return;
        healthSlider.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!TriangleBossAbilities.GetInRange()) { print("outofrange"); return; }
        GameObject obj = col.gameObject;

        if (!(obj.tag == "PlayerBullet")) return;

        health -= 5;
        print(healthSlider);
        healthSlider.value = health;

        if (health > 0) return;

		if (!abilitiesClass.GetMiniBoss())
		{
			SplitBoss();
		}
	}

    private void SplitBoss()
	{
        if (!miniBosses) return;

        miniBosses.SetActive(true);
        gameObject.SetActive(false);
    }
}
