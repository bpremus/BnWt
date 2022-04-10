using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ForkLiftBehavior : MonoBehaviour
{
    [SerializeField] private float move_speed = 10f, 
                                   rotation_speed = 10f, 
                                   center_of_mass_offset = 0.2f;
    [SerializeField] private Collider _grabBox;
    [SerializeField] private Transform _lift;
    [SerializeField] private LayerMask _layerMask;

    public TMP_Text pointText; //This is only temporary 
    private int score;

    protected Rigidbody rb;

    [SerializeField] private float _liftTime;
    private bool canControl = true;
    private bool isGrabbing = false;
    private PickupBehaviour _pickup;

    [SerializeField] private float _liftMin, _liftMax;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.right * center_of_mass_offset;
    }

    void FixedUpdate()
    {
        if (canControl)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space)) { ChangeLiftState(); }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isGrabbing) { _lift.transform.DOLocalMoveZ(_liftMax, 0.5f); GrabStatus(true); }
            else { _lift.transform.DOLocalMoveZ(_liftMin, 0.5f); GrabStatus(false); }
        }
    }

    void Move()
    {
        Vector3 direction = Vector3.zero; // new Vector3(Input.GetAxisRaw("Vertical"), 0f, Input.GetAxisRaw("Horizontal"));

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            direction.x = 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            direction.x = -1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            direction.z = 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            direction.z = -1;

        // apply force (force is rotated as object is rotated) 
        // we need to fix this in blender first or swap the forces here 
        rb.AddForce(transform.right * direction.x * move_speed * Time.fixedDeltaTime, ForceMode.Impulse);

        // move speed
        float localForwardVelocity = Vector3.Dot(rb.velocity, transform.right);
        //Debug.Log(localForwardVelocity);

        if (localForwardVelocity != 0)
        { rb.AddTorque(transform.forward * direction.z * rotation_speed * Time.fixedDeltaTime, ForceMode.Impulse); }
    }

    public void ChangeLiftState()
    {
        if (isGrabbing && _pickup) 
        {
            Debug.Log("Dropping...");
            _lift.transform.DOLocalMoveZ(_liftMin, 0.5f);

            // These two lines might be irrelevant now?
            _pickup.transform.parent = null; 
            _pickup = null;
        }
        else 
        {
            Collider[] hitColliders = Physics.OverlapBox(_grabBox.bounds.center, _grabBox.bounds.size, Quaternion.identity, _layerMask);
            int i = 0;
            foreach (Collider hitCol in hitColliders)
            {
                Debug.Log("Hit : " + hitCol.name + i);
                if (hitCol.transform.TryGetComponent(out _pickup))
                {
                    if (_pickup.gameObject)
                    {
                        canControl = false;
                        StartCoroutine(AnimatePickUp(_pickup));

                        _pickup.transform.parent = this.transform;
                        GrabStatus(true);
                        return;
                    }
                }
                i++;
            }
        }
    }

    public void GrabStatus(bool status)
    {
        isGrabbing = status;
    }

    private IEnumerator AnimatePickUp(PickupBehaviour _p)
    {
        Debug.Log("Lifting...");
        GrabStatus(true);
        Sequence liftSequence = DOTween.Sequence();
        liftSequence.OnComplete(() => { canControl = true; });
        yield return liftSequence.
            Append(_pickup.transform.DOJump(new Vector3(_grabBox.transform.position.x, _liftMax, _grabBox.transform.position.z), _p.jumpPower, 1, _liftTime / 2).SetEase(Ease.InBounce)).
            Append(_pickup.transform.DORotate(new Vector3(0f, 180 * 2f, 0f), _liftTime / 3, RotateMode.FastBeyond360)).
            Insert(0f, _lift.transform.DOLocalMoveZ(_liftMax, _liftTime)).WaitForCompletion();
    }

    #region UI Feedback
    public void IncrementScore(int points)
    {
        score += points;
        pointText.text = "POINTS: " + score;
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (!_grabBox) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_grabBox.bounds.center, _grabBox.bounds.size);

        RaycastHit hit;
        if (Physics.BoxCast(_grabBox.bounds.center, _grabBox.bounds.size / 2, _grabBox.transform.right, out hit, _grabBox.transform.rotation, 5.0f))
        {
            if (hit.transform != transform)
            {
                Gizmos.color = Color.red;
                //Debug.Log("Hitting " + hit.collider.gameObject.name);
                Gizmos.DrawRay(_grabBox.transform.position, _grabBox.transform.right * hit.distance);
                Gizmos.DrawWireCube(_grabBox.bounds.center + _grabBox.transform.right * hit.distance, _grabBox.bounds.size / 2);
            }
        }
        else { Gizmos.color = Color.green; Gizmos.DrawRay(_grabBox.transform.position, _grabBox.transform.right * 0.5f); }
    }
}
