using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trolly : MonoBehaviour
{
    [SerializeField]
    ConveyerBeltPath _mainPath;

    public float distance = 0;
    public float move_speed = 2f;
    public float move_timer = 2f;
    public float stop_timer = 3f;

    // added collider
    protected Collider col;
    private void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    public void SetPath(ConveyerBeltPath path)
    {
        _mainPath = path;
    }

    public void SetOnPath()
    {
        Vector3 postion;
        Vector3 direction;
        _mainPath.GetNodeAtDistance(distance, out postion, out direction);
        transform.position = postion;
    }

    public void Update()
    {
        SetOnPath();
        move_mehanics();
    }

    [SerializeField]
    float _timer = 0;
    public void move_mehanics()
    {
        _timer += Time.deltaTime;
       // if (_timer > move_timer)
       // {
       //      if (_timer > move_timer + stop_timer)
       //      {
       //          _timer = 0;
       //      }
       //  }
       //  else
       //  {
       distance += move_speed * Time.deltaTime;
       //  }
    }

    public void OnDestinationReach()
    {
       
    }

}
