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
    private ParticleSystem shieldParticleSystem;
    private bool shielded;
    private float shieldHealth;

    private void Start()
	{
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        shieldSlider = GameObject.Find("ShieldSlider").GetComponent<Slider>();

        healthSlider.maxValue = health;
        healthSlider.value = health;

        shielded = false;
        shield = transform.GetChild(0).gameObject;
        shield.SetActive(false);

        shieldParticleSystem = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        
        shieldHealth = health - health * 4 / 10;

        shieldSlider.maxValue = shieldHealth;
        shieldSlider.value = shieldHealth;
        shieldSlider.gameObject.SetActive(false);
	}

    private void OnCollisionEnter(Collision col)
	{
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
                shieldParticleSystem.Play(); 
            }
            return;
		} 

        health -= 5;

        healthSlider.value = health;

        if(!shielded && health < 41)
		{
            shielded = true;
            shield.SetActive(true);
            shieldSlider.gameObject.SetActive(true);
            shieldParticleSystem.Play();
		}

        if(health < 1)
		{
            // dead
		}
		
	}
}
