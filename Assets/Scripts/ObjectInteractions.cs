using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    // Reference to the player object (assign in Unity Inspector)
    public GameObject _playerObject;

    // Proximity distances for interactions
    [SerializeField]
    private float _proximityDistance = 5.0f;      // General interaction proximity
    [SerializeField]
    private float _pickupProximityDistance = 2.0f; // Specific distance for item pickup

    // Cached component references for different interaction types
    private KineticBehavior _kineticBehavior;
    private ElectronicBehavior _electronicBehavior;
    private DoorBehavior _doorBehavior;

    // Reference for item interactions with KleptoScript
    private KleptoScript _detectedItem = null; // Currently detected item
    private KleptoScript _heldItem = null;     // Currently held item

    // Hold position for item
    [SerializeField]
    private Transform _holdPosition;

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

        // Check if player is within general proximity distance
        if (distance <= _proximityDistance)
        {
            Debug.Log("Player near object");

            // Handle other interactions
            HandleObjectInteractions();
        }

        // Check for item pickup/drop within closer pickup distance
        if (distance <= _pickupProximityDistance)
        {
            // Check for item pickup/drop with key 'E'
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_heldItem == null)
                {
                    TryPickupItem();
                }
                else
                {
                    DropItem();
                }
            }
        }
    }

    private void HandleObjectInteractions()
    {
        // Check for kinetic interaction (shaking) with key 'H'
        if (Input.GetKeyDown(KeyCode.H) && _kineticBehavior != null)
        {
            _kineticBehavior.TriggerKineticAction();
        }

        // Check for electronic interaction (e.g., power toggle) with key 'T'
        if (Input.GetKeyDown(KeyCode.T) && _electronicBehavior != null)
        {
            _electronicBehavior.TogglePower();
        }

        // Check for door interaction (opening/closing) with key 'O'
        if (Input.GetKeyDown(KeyCode.O) && _doorBehavior != null)
        {
            _doorBehavior.ToggleDoor();
        }

        // Check for tossing the object with key 'G'
        if (Input.GetKeyDown(KeyCode.G) && _kineticBehavior != null)
        {
            _kineticBehavior.TossObject();
        }

        // Check for slamming the door with key 'P'
        if (Input.GetKeyDown(KeyCode.P) && _doorBehavior != null)
        {
            _doorBehavior.SlamDoor();
        }
    }

    private void TryPickupItem()
    {
        // Detect nearby items with KleptoScript within pickup range
        Collider[] hitColliders = Physics.OverlapSphere(_playerObject.transform.position, _pickupProximityDistance);
        foreach (Collider hitCollider in hitColliders)
        {
            KleptoScript item = hitCollider.GetComponent<KleptoScript>();
            if (item != null && !item._isItemPickedUp)
            {
                _heldItem = item;
                _heldItem.OnPickup(_holdPosition);
                Debug.Log("Picked up item: " + _heldItem.name);
                break;
            }
        }
    }

    private void DropItem()
    {
        if (_heldItem != null)
        {
            _heldItem.OnDrop();
            Debug.Log("Dropped item: " + _heldItem.name);
            _heldItem = null;
        }
    }
}
