using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpin : MonoBehaviour
{
    [SerializeField]
    float speed = 20;
    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
