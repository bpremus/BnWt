using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // base class 

    [SerializeField]
    protected Interactable child_object;

    virtual public void Interact(Interactable other)
    { 
    
    }

    virtual public void Interacting(Interactable other)
    { 
    
    }

    protected void OnTriggerEnter(Collider other)
    {
        Interactable i = other.GetComponent<Interactable>();
        if (i)
        {
            Interact(i);
        }
    }

    protected void OnTriggerStay(Collider other)
    {

        Interactable i = other.GetComponent<Interactable>();
        if (i)
        {
            Interacting(i);
        }
    }

    public Interactable HasChild()
    {
        return this.child_object;
    }

    public Interactable GetChild()
    {
        Interactable i = this.child_object;
        if (i)
        { 
            DetachChild();
            this.child_object = null;
        }
        return i;
    }

    virtual protected void OnChildSpawn()
    {
        Vector3 child_snap_offset = transform.position;
        child_object.transform.SetParent(this.transform);
        child_object.transform.localPosition = Vector3.zero;
    }
    virtual protected void DetachChild()
    {
        child_object.transform.SetParent(null);
    }


}
