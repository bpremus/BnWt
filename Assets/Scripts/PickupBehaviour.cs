using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupBehaviour : MonoBehaviour
{
    public float jumpPower;
    public int pointValue;

    private void OnTriggerEnter(Collider other)
    {
        Trolly t = other.GetComponent<Trolly>();
        if (t) //Change later to check if trolly and pickup match type
        {
            ForkLiftBehavior player;
            if (transform.parent.TryGetComponent<ForkLiftBehavior>(out player)) // If the pickup is actually currently attached to the player
            {
                player.ChangeLiftState();
                player.GrabStatus(false);
                player.IncrementScore(SendPoints());
                Destroy(gameObject);
            }
        }
    }

    public int SendPoints()
    {
        return pointValue;
    }
}
