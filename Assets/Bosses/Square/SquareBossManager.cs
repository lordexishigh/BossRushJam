using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareBossManager : MonoBehaviour
{
	[SerializeField]
    private float health;
    private Slider healthSlider;
  
    private GameObject shield;
    private Slider shieldSlider;
    private bool shielded;
    private float shieldHealth;

    private SquareBossAbilities squareAbilities;

    private void Start()
	{
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        shieldSlider = GameObject.Find("ShieldSlider").GetComponent<Slider>();
        squareAbilities = transform.parent.Find("BossAbilities").gameObject.GetComponent<SquareBossAbilities>();

        healthSlider.maxValue = health;
        healthSlider.value = health;

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
        if (!squareAbilities.GetInRange()) return;
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
            squareAbilities.SetMaxAbilities();
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
