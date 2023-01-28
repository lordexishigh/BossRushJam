using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour
{
	[SerializeField]
    private Collider _collider;

    [SerializeField]
    private ParticleSystem explodingParticle;

    [SerializeField]
    private ParticleSystem trailingParticle;

    [SerializeField]
    private GameObject visuals;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _collider = GetComponent<Collider>();
    //    explodingParticle = transform.Find("ExplodingParticles").GetComponent<ParticleSystem>();
    //    trailingParticle = transform.Find("TrailingParticles").GetComponent<ParticleSystem>();
    //    visuals = transform.Find("Visuals").gameObject;
    //}

    private void OnCollisionEnter(Collision col)
	{
        StartCoroutine(DeactivateCube());
	}

    private IEnumerator DeactivateCube()
	{
        if (!visuals) yield break;
        visuals.SetActive(false);
        trailingParticle.Stop();
        _collider.enabled = false;
        explodingParticle.Play();

        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);
        visuals.SetActive(true);
        trailingParticle.Play();
        _collider.enabled = true;
    }

    private void OnDisable()
	{
        transform.rotation = Quaternion.identity;
	}
}
