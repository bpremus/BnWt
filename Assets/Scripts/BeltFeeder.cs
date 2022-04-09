using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltFeeder : MonoBehaviour
{
    [SerializeField] Trolly rail_trolly_prefab;

    [SerializeField] Interactable[] crate_prefabs;

    public float trolly_spawn_timer = 2f;
    public int max_spawn = 3;


    public void Update()
    {
        SpawnTrolly();
    }

    float _timer = 0;
    int _spawn = 0;
    public void SpawnTrolly()
    {

        if (_spawn >= max_spawn) return;

        _timer += Time.deltaTime;
        if (_timer > trolly_spawn_timer)
        {
            _timer = 0;
        }
        else
        {
            return;
        }

        ConveyerBeltPath path = GetComponent<ConveyerBeltPath>();
        if (path)
        {
            Vector3 start_position;
            Vector3 start_rotation;
            BeltNode.NodeType node_type;
            path.GetNodeAtDistance(0, out start_position, out start_rotation, out node_type);
            Trolly t = Instantiate(rail_trolly_prefab, start_position, rail_trolly_prefab.transform.rotation);
            t.SetPath(path);
            OnTrollySpawn(t);

            if (crate_prefabs.Length > 0)
            { 
                Interactable crate = Instantiate(crate_prefabs[0], t.transform.position, t.transform.rotation);
                t.SetChild(crate);
            }
            _spawn++;
        }
    }

    public void OnTrollySpawn(Trolly trolly)
    { 
        
    }
   
}
