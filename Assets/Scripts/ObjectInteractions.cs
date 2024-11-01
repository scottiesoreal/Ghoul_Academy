using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    public GameObject _playerObject;

    [SerializeField]
    private float _proximityDistance = 5.0f;
    [SerializeField]
    private float _pickupProximityDistance = 2.0f;

    private KineticBehavior _kineticBehavior;
    private ElectronicBehavior _electronicBehavior;

    private KleptoScript _detectedItem = null;
    private KleptoScript _heldItem = null;

    [SerializeField]
    private Transform _holdPosition;

    void Start()
    {
        if (_playerObject == null)
        {
            _playerObject = GameObject.FindWithTag("Player");
        }

        _kineticBehavior = GetComponent<KineticBehavior>();
        _electronicBehavior = GetComponent<ElectronicBehavior>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _playerObject.transform.position);

        // General proximity check
        if (distance <= _proximityDistance)
        {
            Debug.Log("Player is near: " + gameObject.name);

            HandleObjectInteractions();
        }

        // Pickup proximity check
        if (distance <= _pickupProximityDistance)
        {
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
        // Kinetic interaction
        if (Input.GetKeyDown(KeyCode.H) && _kineticBehavior != null)
        {
            Debug.Log("Triggered Kinetic Interaction on: " + gameObject.name);
            _kineticBehavior.TriggerKineticAction();
        }

        // Electronic interaction
        if (Input.GetKeyDown(KeyCode.T) && _electronicBehavior != null)
        {
            Debug.Log("Toggled power on: " + gameObject.name);
            _electronicBehavior.TogglePower();
        }

        // Toss interaction
        if (Input.GetKeyDown(KeyCode.G) && _kineticBehavior != null)
        {
            Debug.Log("Tossed object: " + gameObject.name);
            _kineticBehavior.TossObject();
        }
    }

    private void TryPickupItem()
    {
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
