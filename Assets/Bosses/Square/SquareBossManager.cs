using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareBossManager : BossManagerParentClass
{
    [SerializeField]
    protected SquareBossAbilities abilitiesClass;

    private GameObject shield;
    private Slider shieldSlider;
    private bool shielded;
    private float shieldHealth; 

    protected override void StartFunc()
	{
        shieldSlider = GameObject.Find("ShieldSlider").GetComponent<Slider>();

        shielded = false;
        shield = transform.GetChild(0).gameObject;
        shield.SetActive(false);
        
        shieldHealth = health - health * 4 / 10;

        shieldSlider.maxValue = shieldHealth;
        shieldSlider.value = shieldHealth;
        shieldSlider.gameObject.SetActive(false);
	}

    private void OnCollisionEnter(Collision col)
	{
        if (!TriangleBossAbilities.GetInRange()) return;
        GameObject obj = col.gameObject;
        if (!(obj.tag == "PlayerBullet")) return;

		if (shielded && shieldHealth > 0)
		{
            shieldHealth -= 5;
            shieldSlider.value = shieldHealth;
            if(shieldHealth < 1) 
            { 
                shield.SetActive(false);
                shieldSlider.gameObject.SetActive(false);
            }
            return;
		} 

        health -= 5;

        healthSlider.value = health;

        if(!shielded && health < 41)
		{
            abilitiesClass.SetMaxAbilities();
            shielded = true;
            shield.SetActive(true);
            shieldSlider.gameObject.SetActive(true);
		}

        if(health < 1)
		{
            // dead
		}
		
	}
}
