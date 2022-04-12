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


    public float time_between_carts = 1;
    public float time_between_new_batch = 1;
    public int cycles_beween_truck = 6;
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
            if (total_cycels > cycles_beween_truck)
            {
                if (total_cycels % cycles_beween_truck == 0)
                {
                    truck_lane.SetRequiredFamily(random);
                    truck_lane.SpawnTrolly();
                }
            }

            BeltFeeder current_belt = input_belt1;
            if (total_cycels % 2 == 0)
            {
                current_belt = input_belt2;
            }

            total_cycels++;

            int order = Random.Range(1, 4);
            Debug.Log("order: " + order);
            if (order == 1)
            {

                current_belt.SetInputFamily(random);
                current_belt.SpawnTrolly();

                yield return new WaitForSeconds(time_between_carts);

                current_belt.SetInputFamily(random2);
                current_belt.SpawnTrolly();

                yield return new WaitForSeconds(time_between_carts);

                current_belt.SetInputFamily(random3);
                current_belt.SpawnTrolly();

            }
            if (order == 2)
            {
                current_belt.SetInputFamily(random2);
                current_belt.SpawnTrolly();

                yield return new WaitForSeconds(time_between_carts);

                current_belt.SetInputFamily(random);
                current_belt.SpawnTrolly();

                yield return new WaitForSeconds(time_between_carts);

                current_belt.SetInputFamily(random3);
                current_belt.SpawnTrolly();
            }
            if (order == 3)
            {
                current_belt.SetInputFamily(random2);
                current_belt.SpawnTrolly();

                yield return new WaitForSeconds(time_between_carts);

                current_belt.SetInputFamily(random3);
                current_belt.SpawnTrolly();

                yield return new WaitForSeconds(time_between_carts);

                current_belt.SetInputFamily(random);
                current_belt.SpawnTrolly();
            }

            yield return new WaitForSeconds(time_between_new_batch);



        }
        // yield return null;
    }


    public void DisablePaths()
    {
        output_belt.is_active = false;
        input_belt1.is_active = false;
        input_belt2.is_active = false;
    }


    public float belt_power = 1;
    public int enable_belt_two_at = 5;
    public int enable_truck_at = 10;

    public void StartLevel()
    {
      // output_belt.trolly_spawn_timer = 10;
        output_belt.is_active = true;
        output_belt.SetSpeed(2);
        // set required family 
     
      //  input_belt1.trolly_spawn_timer = 5;
        // add to spawn 3 at the same time with random
        input_belt1.is_active = true;
        input_belt1.SetSpeed(belt_power);

      //  input_belt2.trolly_spawn_timer = 5;
        // add to spawn 3 at the same time with random
        input_belt2.is_active = true;
        input_belt2.SetSpeed(belt_power);


       // truck_lane.trolly_spawn_timer = 5;
        truck_lane.is_active = true;
        truck_lane.SetSpeed(belt_power);


    }

}
