using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float deltaRotationX;
    public float deltaRotationY;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        deltaRotationY += Input.GetAxis("Mouse X");
        //deltaRotationX -= Input.GetAxis("Mouse Y");

        rb.MoveRotation(Quaternion.Euler(0, deltaRotationY, 0));
    }
}
