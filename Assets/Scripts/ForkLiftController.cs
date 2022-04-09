using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    public List<Interactable> GetObjectsInRange(float radius = 10f)
    {
        Debug.DrawRay(transform.position, transform.right * radius, Color.green);
        float pickup_angle = 35f;

        List<Interactable> objects_in_range = new List<Interactable>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == this) continue;
            SimpleCrate crate = hitCollider.GetComponent<SimpleCrate>();
            if (crate)
            {
                // if crate is in front 
                Vector3 directionToTarget = crate.transform.position - transform.position;
                float ang = Vector3.Angle(transform.right, directionToTarget);
                if (ang < pickup_angle)
                {
                    objects_in_range.Add(crate);
                }
            }
            Trolly trolly = hitCollider.GetComponent<Trolly>();
            if (trolly)
            {
                // if crate is in front 
                Vector3 directionToTarget = trolly.transform.position - transform.position;
                float ang = Vector3.Angle(transform.right, directionToTarget);
                if (ang < pickup_angle)
                {
                    objects_in_range.Add(trolly);
                }
            }
        }

        // sort by distance
        objects_in_range.OrderBy((d) =>
        (d.transform.position - transform.position).sqrMagnitude).ToArray();

        return objects_in_range;
    }

    [SerializeField]
    float pickup_distance = 5f;
    float _timer = 0;
    public void InteractControl()
    {
        //debug
      //  _interactables = GetObjectsInRange(pickup_distance);

        _timer += Time.deltaTime;
        if (_timer < 0.5)
        {
            return;
        }

        // we only check if player hit the button
        if (Input.GetKey(KeyCode.Space))
        {
            List<Interactable> interactables = GetObjectsInRange(pickup_distance);
            if (interactables.Count > 0)
            {
                //box on ground
                SimpleCrate sc = interactables[0].GetComponent<SimpleCrate>();
                if (sc)
                {
                    // pick it up if we dont have one
                    if (child_object == null)
                    {
                        // pickup crate 
                        SetChild(sc);
                        _timer = 0;
                        return;
                    }
                }
                // box on trolly
                if (interactables.Count > 1)
                { 
                    sc = interactables[1].GetComponent<SimpleCrate>();
                    if (sc)
                    {
                        // pick it up if we dont have one
                        if (child_object == null)
                        {
                            // pickup crate 
                            SetChild(sc);
                            _timer = 0;
                            // notify troll that we picked it up
                            return;
                        }
                    }
                }
                Trolly tc = interactables[0].GetComponent<Trolly>();
                if (tc)
                {
                    // if we have one place it on trolly
                    if (child_object)
                    {
                        tc.SetChild(child_object);
                        child_object = null;
                        _timer = 0;
                        return;
                    }
                }
            }

            // drop on ground 
            if (child_object)
            {
                DetachChild();
                child_object = null;
                _timer = 0;
            }

        }
    } 


    override public void Interact(Interactable other)
    {
       
    }

    override public void Interacting(Interactable other)
    {
       
    }

    public void HandleChild()
    { 
    
    }

    public void SetChild(SimpleCrate child_object)
    {
        // good pick it up
        this.child_object = child_object;
        child_object.SetAsChild();

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
        InteractControl();
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

        // this was for test
        //UILayer.Instance.SetBottomText(localForwardVelocity + " Kph");

        if (localForwardVelocity != 0)
        {
            ForkliftState _state = ForkliftState.fwd;
            if (direction.x < 0)
            {
                direction = -direction;
                _state = ForkliftState.back;
            }
            rb.AddTorque(transform.forward * direction.z * rotation_speed * Time.fixedDeltaTime, ForceMode.Impulse);
            if (forklift_state == ForkliftState.idle)
            {
                forklift_state = _state;
                OnStartMoving();

            }
        }
        else
        {
            if (forklift_state != ForkliftState.idle)
            {
                forklift_state = ForkliftState.idle;
                OnStopMoving();
            }
        }

        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        var rot = Quaternion.FromToRotation(transform.forward, Vector3.up);
        rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * 10f, ForceMode.Impulse);
    }

    public enum ForkliftState { idle, fwd, back };
    ForkliftState forklift_state = ForkliftState.idle;



    // test callbacks 
    // -----------------------------

    public void OnStartMoving()
    {
        // forward
        if (forklift_state == ForkliftState.fwd)
        {
            Debug.Log("wrooom");
        }
        // back
        if (forklift_state == ForkliftState.back)
        {
            Debug.Log("beep beep beep!");
        }
    }

    public void OnStopMoving()
    {
        Debug.Log("Cssssm");
    }

}
