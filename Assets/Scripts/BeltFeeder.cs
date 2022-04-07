using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltFeeder : MonoBehaviour
{
    [SerializeField] Trolly rail_trolly_prefab;

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
            path.GetNodeAtDistance(0, out start_position, out start_rotation);
            Trolly t = Instantiate(rail_trolly_prefab, start_position, rail_trolly_prefab.transform.rotation);
            t.SetPath(path);
            OnTrollySpawn(t);

            _spawn++;
        }
    }

    public void OnTrollySpawn(Trolly trolly)
    { 
        
    }
   
}
