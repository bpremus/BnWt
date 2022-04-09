using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkLiftController : Interactable
{
    protected Rigidbody rb;
    public float move_speed = 10f;
    public float rotation_speed = 10f;
    public float center_of_mass_offset = 0.2f;
    public float localForwardVelocity = 0;
    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.right * center_of_mass_offset;
    }


    override public void Interact(Interactable other)
    {
        SimpleCrate sc = other.GetComponent<SimpleCrate>();
        if (sc)
        {

        }

        Trolly tc = other.GetComponent<Trolly>();
        if (tc)
        {
            // we have a child 
            if (child_object)
            {
                if (tc.HasChild())
                {
                    // nothing to do 
                    return;
                }
                else
                {
                  // place it on trolly
                  //  if (Input.GetKey(KeyCode.Space) == false)
                  //  {
                        tc.SetChild(child_object);
                        child_object = null;
                  //  }

                }
                return;
            }
           // else
           // {
           //     // pick child
           //     if (tc.HasChild())
           //     {
           //         if (child_object == null)
           //         {
           //             // pick it up from trolly
           //             SetChild( tc.GetChild() );
           //         }
           //     }
           // }         
        }
    }

    override public void Interacting(Interactable other)
    {
        if (child_object == null)
        { 
            Trolly tc = other.GetComponent<Trolly>();
            if (tc)
            {
                if (tc.HasChild())
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        SetChild(tc.GetChild());
                    }
                }
            }
        }
        //  SimpleCrate sc = other.GetComponent<SimpleCrate>();
        //  if (sc)
        //  {
        //      if (Input.GetKey(KeyCode.Space))
        //      { 
        //          SetChild(sc);
        //      }
        //  }
    }

    public void HandleChild()
    { 
    
    }


    public void SetChild(Interactable child_object)
    {
        if (this.child_object == child_object)
        {
            // we already have it 
        }
        else if (this.child_object == null)
        {
            // good pick it up
            this.child_object = child_object;
        }

        OnChildSpawn();
    }


    override protected void OnChildSpawn()
    {
        Vector3 child_snap_offset = transform.position;
        child_object.transform.SetParent(this.transform);
        child_object.transform.localPosition = new Vector3(1.2f,0,1f);
        child_object.transform.localRotation = Quaternion.identity;
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
        Vector3 fwd = transform.right;
        fwd.y = 0;
        rb.AddForce(fwd * direction.x * move_speed * Time.fixedDeltaTime, ForceMode.Impulse);

        // move speed
        localForwardVelocity = Mathf.Floor(rb.velocity.magnitude);
        //Debug.Log(localForwardVelocity);

        UILayer.Instance.SetBottomText(localForwardVelocity + " Kph");

        if (localForwardVelocity != 0)
        rb.AddTorque(transform.forward * direction.z * rotation_speed * Time.fixedDeltaTime, ForceMode.Impulse);

    }



}
