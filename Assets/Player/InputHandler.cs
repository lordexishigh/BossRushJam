using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private float mousePressedTimer = 0;
    private PlayerMovement mov;
    private Shooting shooting;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<PlayerMovement>();
        shooting = GetComponent<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        mov.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

		if (Input.GetButtonDown("Jump"))
		    mov.Jump();

        if (Input.GetMouseButtonDown(1))
            mov.ChargeForward();

        if (shooting.ammoAmount() < 1) return;

        if (Input.GetButton("Fire1"))
            mousePressedTimer += Time.deltaTime;

        if ((Input.GetMouseButtonUp(0) && mousePressedTimer > 0) || mousePressedTimer > 0.5f)
        {
            shooting.ShootObject(1 + mousePressedTimer * 3);
            mousePressedTimer = 0;
        }
       
    }
}
