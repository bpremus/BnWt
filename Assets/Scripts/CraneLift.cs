using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CraneLift : MonoBehaviour
{
    private Transform _startPoint, _endPoint;
    [SerializeField] Transform _landingPoint;
    // Later implement a way for the crane to bring in random presets, but for now just spawns the given object
    [SerializeField] PickupBehaviour _pickup;

    void Awake()
    {
        _startPoint = transform;
        _endPoint = _landingPoint;
        foreach (Transform obj in transform)
        {
            var newObj = Instantiate(_pickup.gameObject, obj.position, Quaternion.identity);
            newObj.transform.SetParent(obj);
        }

    }

    void Update()
    {
        StartCoroutine(Enter());
    }

    private IEnumerator Enter()
    {
        Sequence enterSequence = DOTween.Sequence();
        yield return enterSequence.Append(transform.DOMove(_endPoint.position, 2.0f)).
            Append(transform.DORotate(new Vector3(-2.0f, 0, 0), 0.8f)).SetLoops(5, LoopType.Yoyo).WaitForCompletion();
    }
}
