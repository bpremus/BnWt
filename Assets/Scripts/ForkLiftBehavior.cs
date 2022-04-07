using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ForkLiftBehavior : MonoBehaviour
{
    [SerializeField] float _speed = 20f;

    private CharacterController _control;
    [SerializeField] private Collider _grabBox;
    [SerializeField] private Transform _lift;
    [SerializeField] private LayerMask _layerMask;

    private bool canControl = true;
    private bool isGrabbing = false;
    private PickupBehaviour _pickup;

    [SerializeField] private float _liftMin = -2.5f, _liftMax = 2.5f;

    private void Awake()
    {
        _control = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (_pickup != null && DOTween.IsTweening(_pickup.transform)) { canControl = false; } else { canControl = true; }

        Vector3 direction = new Vector3(Input.GetAxisRaw("Vertical"), 0f, Input.GetAxisRaw("Horizontal"));

        if (canControl)
        {
            if (direction.magnitude >= 0.1f)
            {
                _control.Move(direction * _speed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space)) { ChangeLiftState(); }

            
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isGrabbing) { _lift.transform.DOLocalMoveZ(_liftMax, 0.5f); isGrabbing = true; }
            else { _lift.transform.DOLocalMoveZ(_liftMin, 0.5f); isGrabbing = false; }
        }
    }

    void ChangeLiftState()
    {
        if (isGrabbing && _pickup != null) 
        {
            Debug.Log("Dropping...");
            _lift.transform.DOLocalMoveZ(_liftMin, 1.5f);
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
                    if (_pickup.gameObject != null)
                    {
                        Debug.Log("Lifting...");
                        _pickup.transform.DOJump(_grabBox.transform.position, _pickup.jumpPower, 1, 1.5f).SetEase(Ease.InBounce);
                        _pickup.transform.DORotate(new Vector3(0f, 180 * 2f, 0f), 1.5f, RotateMode.FastBeyond360);
                        _lift.transform.DOLocalMoveZ(_liftMax, 1.5f);

                        _pickup.transform.parent = this.transform;
                        isGrabbing = true;
                        return;
                    }
                }
                i++;
            }
        }
    }

    private void OnDrawGizmos()
    {
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
