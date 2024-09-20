using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchTrigger : MonoBehaviour
{
    // Reference to the SpawnBehavior script (set this in the inspector)
    private SpawnBehavior _spawnBehavior;

    void Start()
    {
        // Get the SpawnBehavior component from the object this script is attached to
        _spawnBehavior = GetComponent<SpawnBehavior>();

        if (_spawnBehavior == null)
        {
            Debug.LogError("SpawnBehavior component not found! Please attach it to the same GameObject.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that triggered the collider has the HumanNPC tag
        if (other.CompareTag("HumanNPC"))
        {
            // Log that the human NPC has reached the church
            Debug.Log("Human NPC reached the church, Exorcist should spawn.");

            // Call the SpawnExorcist method from the SpawnBehavior script
            _spawnBehavior.SpawnExorcist();
        }
    }
}
