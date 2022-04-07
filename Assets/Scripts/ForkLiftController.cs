using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkLiftController : MonoBehaviour
{
    protected Rigidbody rb;
    public float move_speed = 10f;
    public float rotation_speed = 10f;
    public float center_of_mass_offset = 0.2f;
    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.right * center_of_mass_offset;
    }

    public void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        Vector3 direction = Vector3.zero; 

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            direction.x = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            direction.x = -1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            direction.z = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            direction.z = 1;

        // apply force (force is rotated as object is rotated) 
        // we need to fix this in blender first or swap the forces here 
        rb.AddForce(transform.right * direction.x * move_speed * Time.fixedDeltaTime, ForceMode.Impulse);

        // move speed
        float localForwardVelocity = Vector3.Dot(rb.velocity,transform.right);
        Debug.Log(localForwardVelocity);

        if (localForwardVelocity != 0)
        rb.AddTorque(transform.forward * direction.z * rotation_speed * Time.fixedDeltaTime, ForceMode.Impulse);

    }



}
