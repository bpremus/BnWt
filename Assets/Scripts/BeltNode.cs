using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltNode : MonoBehaviour
{
    [SerializeField]
    private float _radius = 3f;

    [SerializeField]
    private BeltNode _nextNode;

    private float _distance = 0;

    public enum NodeType { normal, slow, fast, wait};
    [SerializeField]
    public NodeType node_type;

    public bool show_goods_bubble = false;

    public void AddNode(BeltNode node)
    {
        _nextNode = node;
        _distance = Vector3.Distance(transform.position, node.transform.position);
    }

    public float GetDistance()
    {
        return _distance;
    }

    public Vector3 GetNodeAtDistance(float distance)
    {
        Vector3 pt = transform.position;
        Vector3 dir = (transform.position - _nextNode.transform.position).normalized;
        return pt + dir * distance;
    }

    private void OnDrawGizmos()
    {
        if (_nextNode)
        {
            Gizmos.color = Color.green;
            if (node_type == NodeType.slow)
            {
                Gizmos.color = Color.yellow;
            }
            if (node_type == NodeType.fast)
            {
                Gizmos.color = Color.cyan;
            }
            if (node_type == NodeType.wait)
            {
                Gizmos.color = Color.blue;
            }
            Gizmos.DrawLine(transform.position, _nextNode.transform.position);
        }
        else 
        {
            // Last node in line
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
