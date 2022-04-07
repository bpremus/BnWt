using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConveyerBeltPath : MonoBehaviour
{
    [SerializeField]
    private List<BeltNode> _nodes = new List<BeltNode>();
    
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

    public void GetNodeAtDistance(float distance, out Vector3 nodePt, out Vector3 direction)
    {
        nodePt = Vector3.zero;
        direction = Vector3.forward;

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
                return;
            }
        }
        // at end 
        nodePt = _nodes[_nodes.Count-1].transform.position;
    }
}
