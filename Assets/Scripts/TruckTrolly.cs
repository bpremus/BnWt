using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckTrolly : Trolly
{

    protected Vector3 last_pos = Vector3.zero;

    public override void SetOnPath()
    {
        if (derailed)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            return;
        }

        Vector3 postion;
        Vector3 direction;
        int on_path = _mainPath.GetNodeAtDistance(distance, out postion, out direction, out _currentNodeType, out show_goods_bubble);
       
        Vector3 dir = postion - last_pos;
        transform.rotation = Quaternion.LookRotation(dir);

        transform.position = postion;
        last_pos = postion;

        if (on_path == -1)
        {
            OnDestinationReach();
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
      
    }

}
