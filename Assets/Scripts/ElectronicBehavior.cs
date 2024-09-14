using UnityEngine;

public class ElectronicBehavior : MonoBehaviour
{
    private bool isPoweredOn = false;  // Tracks whether the object is powered on or off

    // Toggle the power state (on/off)
    public void TogglePower()
    {
        isPoweredOn = !isPoweredOn;  // Flip the power state

        if (isPoweredOn)
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
