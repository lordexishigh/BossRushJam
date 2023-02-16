using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private ParticleSystem chargeParticles;
    private Vector3 velocity = Vector3.zero;
    private Rigidbody rb;
    private bool onGround;
    private float speed = 70;
    private float jumpSpeed;
    private float chargeCooldown = 0;
    LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        if (UI.GetActiveSceneIndex() == 3) jumpSpeed = 100;
        else jumpSpeed = 50;

        onGround = true;
        mask = LayerMask.GetMask("Default", "Square");
        rb = GetComponent<Rigidbody>();
        chargeParticles = GetComponent<ParticleSystem>();
    }

    private void Update()
	{
        chargeCooldown -= Time.deltaTime;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.2f, Vector3.down, out hit, transform.localScale.y / 2f, mask))
            onGround = true;
        else
            onGround = false;
	}

    public void Move(Vector2 inputs)
	{
        if(chargeCooldown > 0.5f || inputs == Vector2.zero) { return; }
        velocity = transform.forward * inputs.y * speed + transform.right * inputs.x * speed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;
    }

    public void Jump()
	{
        if(onGround)
            rb.velocity = rb.velocity + Vector3.up * jumpSpeed;
	}

    public void ChargeForward()
	{
        if (chargeCooldown > 0) return;

        rb.velocity = transform.forward * speed * 3;
        StartCoroutine(ChargeParticlesToggle());
        chargeCooldown = 1;
    }

    private IEnumerator ChargeParticlesToggle()
	{
        chargeParticles.Play(false);
        yield return new WaitForSeconds(0.5f);
        chargeParticles.Stop();
    }

    public bool GetCharged()
	{
        if (chargeCooldown > 0.4f)
            return true;
        return false;
	}
}
