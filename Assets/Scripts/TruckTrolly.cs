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

        if (_lastNode != on_path)
        {
            OnNodeReach(on_path);
            _lastNode = on_path;
        }

        if (on_path == -1)
        {
            OnDestinationReach();
        }
    }

    public override void OnChildSpawn()
    {
        Vector3 child_snap_offset = transform.position;
        child_object.transform.SetParent(this.transform);
        child_object.transform.localPosition = new Vector3(0, 1.25f, 0);
    }

    public override void OnCollisionEnter(Collision collision)
    {
      
    }

    public override void OnNodeReach(int index)
    {
        // last node 
        if (index == -1)
        {
            
        }
        else
        {

                Debug.Log("truck on node:" + index);

            if (index == 1)
            {
                // first node 

                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Ambient & Objects/Truck_Horn", gameObject);
            }

        }


    }


    public override void OnDestinationReach()
    {
        Debug.Log("truck reached destination");

        // notify score script 
        if (requered_family > 0)
        {
            // required family for outgoing carts

            int child_family = -1;
            if (child_object == null)
            {
                // invalid delivery no child
                GameManager.Instance.OnCartReachDestination(-1);
            }
            else
            {
                SimpleCrate sc = child_object.GetComponent<SimpleCrate>();
                if (sc)
                {
                    child_family = sc.family;
                }

                if (requered_family == child_family)
                {

                    // valid cargo delivery
                    GameManager.Instance.OnCartReachDestination(1);
                }
                else
                {
                    // invalid delivery
                    GameManager.Instance.OnCartReachDestination(-1);
                }
            }
        }

        // destroy cart 
        TrolleyLoop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        TrolleyLoop.release();
        Destroy(this.gameObject);
    }

}
