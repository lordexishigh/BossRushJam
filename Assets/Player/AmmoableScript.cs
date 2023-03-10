using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoableScript : MonoBehaviour
{
    private static Transform playerTransform;
    private static Shooting shooting;
    private static int scaleMod = 20;

    private Rigidbody rb;
    private Vector3 scaleVector;

    private bool isAmmo;
    private bool captured;

    [SerializeField]
    private ParticleSystem fireParticle;
    private ParticleSystem explodeParticle;

    void Start()
    {
        isAmmo = false;
        captured = false;

        rb = GetComponent<Rigidbody>();

        scaleVector = transform.localScale / scaleMod;

        explodeParticle = transform.parent.GetChild(1).GetComponent<ParticleSystem>();
        var main = explodeParticle.main;
        main.startColor = GetComponent<MeshRenderer>().material.color;

        if (playerTransform) return;
		
        GameObject tempObj = GameObject.FindWithTag("Player");
        playerTransform = tempObj.GetComponent<Transform>();
        shooting = tempObj.GetComponent<Shooting>();
    }

    /// <summary>
	/// 1 for ammo , 2 for obj
	/// </summary>
	/// <param name=""></param>
    public void ChangeObjectState(int state)
    {
        switch (state) {
            case 1:
                StartCoroutine(ObjToAmmo());
                break;
            case 2:
                StartCoroutine(AmmoToObj());
                break;
            default:
                Debug.Log("WrongState");
                break;
        }
	}

    private IEnumerator ObjToAmmo()
	{
        if (isAmmo || captured || shooting.ammoAmount() > 19) yield break;

        rb.useGravity = true;
        if(fireParticle) { fireParticle.Stop(); }

        captured = true;
        float step = Vector3.Distance(transform.position, playerTransform.position) / 20;

        Destroy(GetComponent<FixedJoint>());

        for (int i = 0; i < scaleMod - 2; i++)
		{
            RescaleObject(-1);

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
            yield return new WaitForSeconds(0.5f / scaleMod);
		}

        shooting.AddShootableObject(transform.gameObject);

        //transform.localScale = Vector3.one * 0.6f;
        transform.position = playerTransform.position + Vector3.up / 2;
        gameObject.layer = LayerMask.NameToLayer("InnerPlayer");

        isAmmo = true;
        transform.rotation = Quaternion.identity;
        transform.parent.parent = playerTransform;
    }

    private IEnumerator AmmoToObj()
    {
        if (!isAmmo) yield break;
        gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
        isAmmo = false;
        transform.parent.parent = null;
        transform.localScale = scaleVector * 2;

        gameObject.tag = "PlayerBullet";

        for (int i = 0; i < scaleMod - 2; i++)
        {
            RescaleObject(1);
            yield return new WaitForSeconds(0.5f / scaleMod);
        }
        
        captured = false;
    }

    public void RescaleObject(float sign)
	{
        transform.localScale = transform.localScale + scaleVector * sign;
	}

    private void OnCollisionEnter(Collision col)
	{
        if (col.gameObject.tag == "Harmuful" || col.gameObject.tag == "Boss") 
        {
            explodeParticle.Play();
            explodeParticle.gameObject.transform.position = transform.position;
            gameObject.SetActive(false);
        }
        else if(transform.localScale.x > scaleVector.x * (scaleMod - 2)) 
            StartCoroutine(ResetTag());
    }

    private IEnumerator ResetTag()
	{
        yield return new WaitForSeconds(1f);
        gameObject.tag = "Ammoable";
	}

    public bool GetAmmo()
	{
        return isAmmo;
	}
}
