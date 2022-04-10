using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCrate : Interactable
{
    protected BoxCollider collider;
    protected Rigidbody rigidbody;

    public int family = 0;

    public void Awake()
    {
        
        collider = GetComponent<BoxCollider>();
        if (collider)
            collider.isTrigger = true;
        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody)
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
        if (collider)
            collider.isTrigger = true;
        if (rigidbody)
            rigidbody.isKinematic = true;
    }

    public void UnsetChild()
    {
        if (collider)
            collider.isTrigger = false;
        if (rigidbody)
            rigidbody.isKinematic = false;
    }

}
