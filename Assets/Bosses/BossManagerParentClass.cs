using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BossManagerParentClass : MonoBehaviour
{
    [SerializeField]
    protected float health;

    [SerializeField]
    protected BossParentClass abilitiesClass;

    [SerializeField]
    protected Slider healthSlider;

    // Start is called before the first frame update
    protected void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

        StartFunc();
    }

    protected virtual void StartFunc() {  }
}
