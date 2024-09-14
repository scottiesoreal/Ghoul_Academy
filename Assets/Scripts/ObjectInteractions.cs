using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    // Reference to the player object (assign in Unity Inspector)
    public GameObject _playerObject;

    // Proximity distance for the interaction
    [SerializeField]
    private float _proximityDistance = 5.0f;

    // Cached component references
    private KineticBehavior _kineticBehavior;
    private ElectronicBehavior _electronicBehavior;
    private DoorBehavior _doorBehavior;

    void Start()
    {
        // Optional: Assign the player object automatically if not assigned in the Inspector
        if (_playerObject == null)
        {
            _playerObject = GameObject.FindWithTag("Player");
        }

        // Cache the component references
        _kineticBehavior = GetComponent<KineticBehavior>();
        _electronicBehavior = GetComponent<ElectronicBehavior>();
        _doorBehavior = GetComponent<DoorBehavior>();
    }

    void Update()
    {
        // Calculate distance between player and object
        float distance = Vector3.Distance(transform.position, _playerObject.transform.position);

        // Check if player is within proximity distance
        if (distance <= _proximityDistance)
        {
            Debug.Log("Player near object");

            // Check for kinetic interaction (shaking) with key 'H'
            if (Input.GetKeyDown(KeyCode.H) && _kineticBehavior != null)
            {
                _kineticBehavior.TriggerKineticAction();  // Trigger the shaking or physical interaction
            }

            // Check for electronic interaction (e.g., power toggle) with key 'T'
            if (Input.GetKeyDown(KeyCode.T) && _electronicBehavior != null)
            {
                _electronicBehavior.TogglePower();  // Trigger the power toggle
            }

            // Check for door interaction (opening/closing) with key 'O'
            if (Input.GetKeyDown(KeyCode.O) && _doorBehavior != null)
            {
                _doorBehavior.ToggleDoor();  // Trigger door opening/closing
            }

            // Check for tossing the object with key 'G'
            if (Input.GetKeyDown(KeyCode.G) && _kineticBehavior != null)
            {
                Debug.Log("Object tossed");
                _kineticBehavior.TossObject();  // Trigger tossing the object
            }

            // Check for slamming the door with key 'P'
            if (Input.GetKeyDown(KeyCode.P) && _doorBehavior != null)
            {
                _doorBehavior.SlamDoor();  // Trigger door slamming
            }
        }
    }
}
