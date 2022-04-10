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

    public void StartLevel()
    {
        output_belt.trolly_spawn_timer = 10;
        output_belt.is_active = true;
        output_belt.SetSpeed(2);
    }

}
