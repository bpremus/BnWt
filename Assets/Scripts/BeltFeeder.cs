using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltFeeder : MonoBehaviour
{
    [SerializeField] Trolly rail_trolly_prefab;

    [SerializeField] Interactable[] crate_prefabs;

    public float trolly_spawn_timer = 2f;
    public int max_spawn = 3;
    public bool is_active = false;

    public int required_family = -1;
    public int input_family = -1;
    public int randomize_count = 3; // how many with wrong family

    ConveyerBeltPath path;
    public void Awake()
    {
        path = GetComponent<ConveyerBeltPath>();
    }

    public void Update()
    {
        if (is_active == true)
        SpawnTrolly();
    }

    public void SetRequiredFamily(int family)
    {
        required_family = family;
    }

    public void SetInputFamily(int family)
    {
        input_family = family;
    }

    public void SetSpeed(float power)
    {
        path.power = power;
    }

    float _timer = 100;
    int _spawn = 0;
    public void SpawnTrolly()
    {

        if (max_spawn != 0 &&_spawn >= max_spawn) return;

        _timer += Time.deltaTime;
        if (_timer > trolly_spawn_timer)
        {
            _timer = 0;
        }
        else
        {
            return;
        }

        
        if (path)
        {
            Vector3 start_position;
            Vector3 start_rotation;
            BeltNode.NodeType node_type;
            bool show_goods = false; // not used now
            path.GetNodeAtDistance(0, out start_position, out start_rotation, out node_type, out show_goods);
            Trolly t = Instantiate(rail_trolly_prefab, start_position, rail_trolly_prefab.transform.rotation);
            t.SetPath(path);
            OnTrollySpawn(t);

            t.requered_family = required_family;

            if (crate_prefabs.Length > 0)
            {
                Interactable crate = Instantiate(crate_prefabs[0], t.transform.position, t.transform.rotation);
                SimpleCrate sc = crate.GetComponent<SimpleCrate>();
                if (sc)
                {
                    // add count based on _spawn 
                    // to randomize

                    sc.family = input_family;
                }
                t.SetChild(crate);           
            }
            _spawn++;
        }
    }

    public void OnTrollySpawn(Trolly trolly)
    { 
        
    }
   
}
