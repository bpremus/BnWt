using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCrate : Interactable
{
    protected BoxCollider collider;
    protected Rigidbody rigidbody;
    public void Awake()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }
    // override 
    public override void Interact(Interactable other)
    {
    
    }

    public void Update()
    {
        if (this.transform.parent == null)
        {
            UnsetChild();
        }
        else
        {
            SetAsChild();
        }
    }

    public void SetAsChild()
    {
        collider.isTrigger = true;
        rigidbody.isKinematic = true;
    }

    public void UnsetChild()
    {
        collider.isTrigger = false;
        rigidbody.isKinematic = false;
    }

}
