using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    // Is the door currently open or closed?
    private bool _isOpen = false;

    // This method is triggered when the player interacts with the door
    public void ToggleDoor()
    {
        if (_isOpen)
        {
            // Close the door
            Debug.Log("The door is now closed.");
        }
        else
        {
            // Open the door
            Debug.Log("The door is now open.");
        }

        _isOpen = !_isOpen; // Toggle the door state
    }

    // This method will simulate slamming the door
    public void SlamDoor()
    {
        Debug.Log("The door is slammed shut!");
        _isOpen = false; // Ensure the door is closed after slamming
    }
}
