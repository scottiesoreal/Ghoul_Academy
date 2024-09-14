using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    // Reference to the player object (assign in Unity Inspector)
    public GameObject playerObject;

    // Proximity distance for the interaction
    [SerializeField]
    private float proximityDistance = 5.0f;

    void Start()
    {
        // Optional: Assign the player object automatically if not assigned in the Inspector
        if (playerObject == null)
        {
            playerObject = GameObject.FindWithTag("Player");
        }
    }

    void Update()
    {
        // Calculate distance between player and object
        float distance = Vector3.Distance(transform.position, playerObject.transform.position);

        // Check if player is within proximity distance
        if (distance <= proximityDistance)
        {
            Debug.Log("Player near object");

            // Check for kinetic interaction (shaking) with key 'H'
            if (Input.GetKeyDown(KeyCode.H))
            {
                KineticBehavior kineticBehavior = GetComponent<KineticBehavior>();
                if (kineticBehavior != null)
                {
                    kineticBehavior.TriggerKineticAction();  // Trigger the shaking or physical interaction
                }
            }

            // Check for electronic interaction (e.g., power toggle) with key 'T'
            if (Input.GetKeyDown(KeyCode.T))
            {
                ElectronicBehavior electronicBehavior = GetComponent<ElectronicBehavior>();
                if (electronicBehavior != null)
                {
                    electronicBehavior.TogglePower();  // Trigger the power toggle
                }
            }

            // Check for door interaction (opening/closing) with key 'O'
            if (Input.GetKeyDown(KeyCode.O))
            {
                DoorBehavior doorBehavior = GetComponent<DoorBehavior>();
                if (doorBehavior != null)
                {
                    doorBehavior.ToggleDoor();  // Trigger door opening/closing
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                KineticBehavior kineticBehavior = GetComponent<KineticBehavior>();
                if (kineticBehavior != null)
                {
                    Debug.Log("Object tossed");
                    kineticBehavior.TossObject();  // Trigger tossing the object
                }
            }

            // Check for slamming the door with key 'P'
            if (Input.GetKeyDown(KeyCode.P))
            {
                DoorBehavior doorBehavior = GetComponent<DoorBehavior>();
                if (doorBehavior != null)
                {
                    doorBehavior.SlamDoor();  // Trigger door slamming
                }
            }
        }
    }
}
