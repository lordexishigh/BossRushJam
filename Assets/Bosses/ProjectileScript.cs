using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	[SerializeField]
    private Collider _collider;

    [SerializeField]
    private ParticleSystem explodingParticle;

    [SerializeField]
    private GameObject visuals;

    protected virtual void OnCollisionEnter(Collision col)
	{
        StartCoroutine(DeactivateProjectile());
	}

    protected IEnumerator DeactivateProjectile()
	{
        if (!visuals) yield break;

        visuals.SetActive(false);
        _collider.enabled = false;

        explodingParticle.transform.position = transform.position;
        explodingParticle.Play();

        yield return new WaitForSeconds(2);

        visuals.SetActive(true);
        gameObject.SetActive(false);
        _collider.enabled = true;
    }

    private void OnDisable()
	{
        transform.rotation = Quaternion.identity;
	}
}
