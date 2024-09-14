using UnityEngine;

public class ElectronicBehavior : MonoBehaviour
{
    private bool _isPoweredOn = false;  // Tracks whether the object is powered on or off

    // Toggle the power state (on/off)
    public void TogglePower()
    {
        _isPoweredOn = !_isPoweredOn;  // Flip the power state

        if (_isPoweredOn)
        {
            Debug.Log("Power ON: " + gameObject.name);  // Placeholder: Log the power state
            // Future: Add actual behavior for turning on (e.g., turn on lights, display)
        }
        else
        {
            Debug.Log("Power OFF: " + gameObject.name);  // Placeholder: Log the power state
            // Future: Add actual behavior for turning off (e.g., turn off lights, display)
        }
    }
}
