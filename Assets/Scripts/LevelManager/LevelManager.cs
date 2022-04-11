using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BeltFeeder output_belt;
    public BeltFeeder input_belt1;
    public BeltFeeder input_belt2;
    public BeltFeeder truck_lane;

    public int level = 0;


    public void Start()
    {
        StartLevel();
        StartCoroutine(SpawnControl());
    }

    public void Update()
    {
       
    }

    int total_cycels = 0;
    public IEnumerator SpawnControl()
    {
        while (true)
        {
            int random  = Random.Range(1, 7);
            int random2 = Random.Range(1, 7);
            int random3 = Random.Range(1, 7);
            Debug.Log("family: " + random);
           // Debug.Log(random2);
           // Debug.Log(random3);

            output_belt.SetRequiredFamily(random);
            output_belt.SpawnTrolly();

            // every third loop add a truck
            if (total_cycels % 3 == 0)
            { 
                truck_lane.SetRequiredFamily(random);
                truck_lane.SpawnTrolly();
            }

            int order = Random.Range(1, 4);
            Debug.Log("order: " + order);
            if (order == 1)
            {

                input_belt1.SetInputFamily(random);
                input_belt1.SpawnTrolly();

                yield return new WaitForSeconds(1);

                input_belt1.SetInputFamily(random2);
                input_belt1.SpawnTrolly();

                yield return new WaitForSeconds(1);

                input_belt1.SetInputFamily(random3);
                input_belt1.SpawnTrolly();

            }
            if (order == 2)
            {
                input_belt1.SetInputFamily(random2);
                input_belt1.SpawnTrolly();

                yield return new WaitForSeconds(1);

                input_belt1.SetInputFamily(random);
                input_belt1.SpawnTrolly();

                yield return new WaitForSeconds(1);

                input_belt1.SetInputFamily(random3);
                input_belt1.SpawnTrolly();
            }
            if (order == 3)
            {
                input_belt1.SetInputFamily(random2);
                input_belt1.SpawnTrolly();

                yield return new WaitForSeconds(1);

                input_belt1.SetInputFamily(random3);
                input_belt1.SpawnTrolly();

                yield return new WaitForSeconds(1);

                input_belt1.SetInputFamily(random);
                input_belt1.SpawnTrolly();
            }

            yield return new WaitForSeconds(10);



        }
        // yield return null;
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
     
        input_belt1.trolly_spawn_timer = 5;
        // add to spawn 3 at the same time with random
        input_belt1.is_active = true;
        input_belt1.SetSpeed(3);


        truck_lane.trolly_spawn_timer = 5;
        truck_lane.is_active = true;
        truck_lane.SetSpeed(3);


    }

}
