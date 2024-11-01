using UnityEngine;

public class ProximityDetection : MonoBehaviour
{
    public float proximityDistance = 5.0f;  // Adjust proximity distance as needed

    private void Update()
    {
        DetectNearbyObjects();
    }

    private void DetectNearbyObjects()
    {
        // Find all objects within the specified range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, proximityDistance);

        foreach (Collider hitCollider in hitColliders)
        {
            // Check if the object is interactable
            if (hitCollider.CompareTag("Interactable"))
            {
                Debug.Log("Detected nearby interactable object: " + hitCollider.gameObject.name);

                // Check for input to trigger kinetic action (shake)
                if (Input.GetKeyDown(KeyCode.H))
                {
                    // Try to get the KineticBehavior component on the object
                    KineticBehavior kineticBehavior = hitCollider.GetComponent<KineticBehavior>();
                    if (kineticBehavior != null)
                    {
                        kineticBehavior.TriggerKineticAction();  // Trigger the shake action
                    }
                    else
                    {
                        Debug.LogWarning("KineticBehavior not found on " + hitCollider.gameObject.name);
                    }
                }
            }
        }
    }
}
