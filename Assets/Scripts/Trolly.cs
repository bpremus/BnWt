using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trolly : Interactable
{
    [SerializeField]
    protected ConveyerBeltPath _mainPath;

    public float distance = 0;

    public float move_timer = 2f;
    public float stop_timer = 3f;

    public bool derailed = false;

    [SerializeField]
    protected BeltNode.NodeType _currentNodeType;

    [SerializeField]
    protected BilboardBubble bilboard;
    private float bilboardScale;

    protected bool show_goods_bubble = false;

    [SerializeField]
    public int requered_family = 0;

    // override 
    public override void Interact(Interactable other)
    {
        Debug.Log("interact with " + other.name);
    }

    protected FMOD.Studio.EventInstance TrolleyLoop;


    public virtual void OnCollisionEnter(Collision collision)
    {
        ForkLiftController forkLift = collision.collider.GetComponent<ForkLiftController>();
        if (forkLift)
        {
            if (forkLift.localForwardVelocity > 5)
            {
                if (derailed == false)
                {
                    OnDerailed();
                }
                derailed = true;
            }           
        }
    }

    public void SetChild(Interactable child_object)
    {
        this.child_object = child_object;
        OnChildSpawn();
    }

    // -----------------------------------

    // added collider
    protected Collider col;
    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        bilboardScale = bilboard.transform.localScale.x;
        bilboard.transform.localScale = Vector3.zero;
        _currentNodeType = BeltNode.NodeType.wait;

        TrolleyLoop = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Ambient & Objects/Trolley_Movement");
        TrolleyLoop.start();

    }

    public void SetPath(ConveyerBeltPath path)
    {
        _mainPath = path;
    }

    protected int _lastNode = 0;
    public virtual void SetOnPath()
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
        transform.position = postion;

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

    public void Update()
    {
        SetOnPath();
        move_mehanics();
        ShowGoods();

        TrolleyLoop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform, GetComponent<Rigidbody2D>()));

    }
    public bool hardcore = false;
    public void ShowGoods()
    {
        if (bilboard == null) return;
        if (hardcore)
        {
            if (show_goods_bubble)
            {
                bilboard.gameObject.SetActive(true);
                //bilboard.transform.DOScale(bilboardScale, 0.2f).SetEase(Ease.OutBounce);
                bilboard.SetFamily(requered_family);
            }
            else
            {
                bilboard.gameObject.SetActive(false);
            }
        }
        else
        {
            if (child_object == null && requered_family != 0)
            {
                bilboard.gameObject.SetActive(true);
                bilboard.transform.DOScale(bilboardScale, 0.2f).SetEase(Ease.OutBounce);
                bilboard.SetFamily(requered_family);
            }
            else
            {
                bilboard.gameObject.SetActive(false);
            }
        }
    }

    public virtual void move_mehanics()
    {
        if (_mainPath.power > 0)
        {
            float cur_speed = _mainPath.move_speed;
            if (_currentNodeType == BeltNode.NodeType.slow)
            {
                cur_speed = _mainPath.slow_speed;
            }
            else if (_currentNodeType == BeltNode.NodeType.slow)
            {
                cur_speed = _mainPath.fast_speed;
            }
            else
            {

            }
            distance += cur_speed * _mainPath.power *  Time.deltaTime;
        }
    }

    public virtual void OnNodeReach(int index)
    {
        // last node 
        if (index == -1)
        {

        }
        else
        { 
            
        }
    }


    public virtual void OnDestinationReach()
    {
        Debug.Log("cart reached destination");

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

    public void OnDerailed()
    {

        // notify score script 
        GameManager.Instance.OnCartDerailed();

        TrolleyLoop.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        TrolleyLoop.release();

        Debug.Log("cart derailed");
    }

}
