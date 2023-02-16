using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossManagerParentClass : MonoBehaviour
{
    [SerializeField]
    protected float health;

    protected float maxHealth;

    [SerializeField]
    protected ParticleSystem explodeParticles; 

    [SerializeField]
    protected Slider healthSlider;

    // Start is called before the first frame update
    protected void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        maxHealth = health;

        StartFunc();
    }

    protected virtual void StartFunc() {  }
}
