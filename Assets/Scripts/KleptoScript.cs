using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KleptoScript : MonoBehaviour
{
    //Add this script to pickup item

    public bool _isItemPickedUp = false; // Is the item picked up by the player ghost
    private Rigidbody _rb; // Rigidbody component of the item

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    //called when item is picked up
    public void OnPickup(Transform holdPosition)
    {
        _isItemPickedUp = true;
        transform.position = holdPosition.position;
        transform.SetParent(holdPosition);

        if (_rb != null)
        {
            _rb.isKinematic = true; // Disable physics interactions
        }

    }

    public void OnDrop()
    {
        _isItemPickedUp = false;
        transform.SetParent(null);

        if (_rb != null)
        {
            _rb.isKinematic = false; // Enable physics interactions
        }

    }
}
