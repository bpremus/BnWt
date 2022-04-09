using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConveyerBeltPath : MonoBehaviour
{
    [SerializeField]
    private List<BeltNode> _nodes = new List<BeltNode>();

    public float power = 0;
    public float move_speed = 2f;
    public float slow_speed = 1f;
    public float fast_speed = 3f;

    // should be added in order 
    public void BuildPath()
    {
        _nodes.Clear();
        BeltNode[] nodes = transform.GetComponentsInChildren<BeltNode>();
        for (int i = 0; i < nodes.Length; i++)
        {
            _nodes.Add(nodes[i]);
            if (i+1 < nodes.Length)
            {
                // do some processing of nodes
                nodes[i].name = "node_" + i;
                nodes[i].AddNode(nodes[i + 1]);    
            }
        }
    }

    public void Start()
    {
        BuildPath();
    }

    public void GetNodeAtDistance(float distance, out Vector3 nodePt, out Vector3 direction, out BeltNode.NodeType type)
    {
        nodePt = Vector3.zero;
        direction = Vector3.forward;
        type = BeltNode.NodeType.normal;

        float total_distance = 0;
        for (int i = 0; i < _nodes.Count; i++)
        {
            float next_distance = _nodes[i].GetDistance();
            total_distance += next_distance;
            if (distance < total_distance)
            {
                // we are on this node segment
                float curret_dist = (total_distance - next_distance) - distance;
                nodePt = _nodes[i].GetNodeAtDistance(curret_dist);
                type = _nodes[i].node_type;
                return;
            }
        }
        // at end 
        nodePt = _nodes[_nodes.Count-1].transform.position;
    }
}
