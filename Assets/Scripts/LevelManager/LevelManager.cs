using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BeltFeeder output_belt;
    public BeltFeeder input_belt1;
    public BeltFeeder input_belt2;

    public int level = 0;


    public void Start()
    {
        StartLevel();
    }

    public void DisablePaths()
    {
        output_belt.is_active = false;
        input_belt1.is_active = false;
        input_belt2.is_active = false;
    }

    public void StartLevel()
    {
        output_belt.trolly_spawn_timer = 10;
        output_belt.is_active = true;
        output_belt.SetSpeed(2);
        // set required family 
        output_belt.SetRequiredFamily(2);

        input_belt1.trolly_spawn_timer = 5;
        // add to spawn 3 at the same time with random
        input_belt1.is_active = true;
        input_belt1.SetSpeed(3);
        input_belt1.SetInputFamily(2);
    }

}
