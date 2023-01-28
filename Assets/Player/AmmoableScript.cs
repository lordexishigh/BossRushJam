using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoableScript : MonoBehaviour
{
    private static Transform playerTransform;
    private static Shooting shooting;
    private static int scaleMod = 20;

    private bool isAmmo;
    private bool captured;

    private Vector3 scaleVector;

  //  private ParticleSystem explodeParticle;

    void Start()
    {
        isAmmo = false;
        captured = false;

        scaleVector = transform.localScale / scaleMod;

     //   explodeParticle = parentTransform.GetChild(1).GetComponent<ParticleSystem>();
     //   var main = explodeParticle.main;
     //   main.startColor = GetComponent<MeshRenderer>().material.color;

        if (playerTransform) return;
		
        GameObject tempObj = GameObject.FindWithTag("Player");
        playerTransform = tempObj.GetComponent<Transform>();
        shooting = tempObj.GetComponent<Shooting>();
        
    }

	//private void OnEnable()
	//{
 //       if (scaleVector == Vector3.zero) return;
 //       StartCoroutine(AmmoToObj());
	//}

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
                print("WrongState");
                break;
        }
	}

    private IEnumerator ObjToAmmo()
	{
        if (isAmmo || captured || shooting.ammoAmount() > 4) yield break;

        captured = true;
        float step = Vector3.Distance(transform.position, playerTransform.position) / 20;

        Destroy(GetComponent<HingeJoint>());

        for (int i = 0; i < scaleMod - 2; i++)
		{
            RescaleObject(-1);

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);
            print(transform.localScale);
            yield return new WaitForSeconds(0.5f / scaleMod);
		}

        shooting.AddShootableObject(gameObject);

        transform.localScale = Vector3.one * 0.6f;
        print("ok" + transform.localScale);
        transform.position = playerTransform.position + Vector3.up / 2;
        gameObject.layer = LayerMask.NameToLayer("InnerPlayer");

        isAmmo = true;
        transform.rotation = Quaternion.identity;
        transform.parent = playerTransform;
    }

    private IEnumerator AmmoToObj()
    {
        if (!isAmmo) yield break;
        gameObject.layer = LayerMask.NameToLayer("Square");
        isAmmo = false;
        transform.parent = null;

        transform.localScale = scaleVector * 2;

        for (int i = 0; i < scaleMod - 2; i++)
        {
            RescaleObject(1);
            yield return new WaitForSeconds(0.5f / scaleMod);
        }
        
        captured = false;
        gameObject.tag = "PlayerBullet";
    }

    public void RescaleObject(float sign)
	{
        transform.localScale = transform.localScale + scaleVector * sign;
	}

    private void OnCollisionEnter(Collision col)
	{
        if(col.gameObject.tag == "Harmuful" || col.gameObject.tag == "Boss")
		{

		}
        else
            StartCoroutine(ResetTag());
    }

    private IEnumerator ResetTag()
	{
        yield return new WaitForSeconds(0.5f);
        gameObject.tag = "Ammoable";
	}

    public bool GetAmmo()
	{
        return isAmmo;
	}
}
