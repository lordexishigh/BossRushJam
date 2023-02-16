using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : BossManagerParentClass
{
    [SerializeField]
    protected StarBossAbilities abilitiesClass;

    Vector3 scaleMod;

    void Awake()
	{
        scaleMod = transform.parent.localScale / 20;
	}

    void OnEnable()
    {
        if(maxHealth != 0)
		{
            health = maxHealth;
		}
        StartCoroutine(RescaleBoss(1));
        
        if(abilitiesClass.GetMiniBoss()) 
        { 
            healthSlider.gameObject.SetActive(true);
            healthSlider.value = health;
        }
    }

    void OnDisable()
	{
        if (abilitiesClass.GetMiniBoss()) { healthSlider.gameObject.SetActive(false); }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!abilitiesClass.GetMiniBoss() && abilitiesClass.getMiniBossAlive()) return;
        GameObject obj = col.gameObject;

        if ((obj.tag != "PlayerBullet")) return;

        health -= 5;

        healthSlider.value = health;

        if (health > 0) return; //dead

        explodeParticles.Play();
        StartCoroutine(RescaleBoss(-1));
        
        if(abilitiesClass.GetMiniBoss())
        {
            for (int i = 0; i < 5; i++)
            {
                obj = StarBossAbilities.getAmmoableStar();
                obj.transform.position = transform.position;
            } 
        }
    }

    private IEnumerator RescaleBoss(int sign = 1)
	{
        Transform parent = transform.parent; 

        if(sign == 1)
		{
            parent.localScale = Vector3.zero;
		}

        for(int i = 0; i < 20; i++)
		{
            parent.localScale += sign * scaleMod;
            yield return new WaitForSeconds(0.05f);
		}
        if(sign == -1)
		{
            parent.localScale = Vector3.zero;
            parent.gameObject.SetActive(false);
        }
	}
}
