using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
//using UnityEngine.PostProcessing;

public class PlayerManager : MonoBehaviour
{
    private bool recentlyDamaged = false;
    private float health = 3;
    private PlayerMovement mov;
    //private PostProcessVolume volume;
    Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<PlayerMovement>();
        PostProcessVolume volume = GameObject.Find("PostProcessing").GetComponent<PostProcessVolume>();

        volume.profile.TryGetSettings<Vignette>(out vignette);

        vignette.enabled.Override(true);
        vignette.intensity.Override(0);
    }

    private void OnCollisionEnter(Collision collision)
	{
        string tag = collision.gameObject.tag;
        if ((tag == "Harmful" || tag == "Boss") && !mov.GetCharged() && !recentlyDamaged)
        {
            print("hit");
            health -= 1;
            recentlyDamaged = true;
            StartCoroutine(TakeDamageEffect());
            if (health < 1) { Time.timeScale = 0; print("dead"); }
        }
	}

    private IEnumerator TakeDamageEffect()
	{
        for (int i = 25; i > -1; i--)
		{
            vignette.intensity.Override(i * 0.01f);
            yield return new WaitForSeconds(0.04f);
        }
        recentlyDamaged = false;
    }
}
