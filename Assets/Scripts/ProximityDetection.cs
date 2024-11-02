using System.Collections;
using UnityEngine;

public class ProximityDetection : MonoBehaviour
{
    public float proximityDistance = 5.0f;  // Adjust proximity distance as needed
    public Material outlineMaterial;        // Assign OutlineMaterial in the Inspector

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

                Renderer objectRenderer = hitCollider.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    // Store the object's original material
                    Material originalMaterial = objectRenderer.material;

                    // Apply the outline material to make it glow
                    objectRenderer.material = outlineMaterial;

                    // Reset to the original material after proximity is lost
                    StartCoroutine(RevertMaterialAfterProximityLoss(hitCollider, objectRenderer, originalMaterial));
                }

                // Check for input to trigger kinetic action (shake)
                if (Input.GetKeyDown(KeyCode.H))
                {
                    // Try to get the KineticBehavior component on the object
                    KineticBehavior kineticBehavior = hitCollider.GetComponent<KineticBehavior>();
                    if (kineticBehavior != null)
                    {
                        Debug.Log("Shaking object: " + hitCollider.gameObject.name);
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

    private IEnumerator RevertMaterialAfterProximityLoss(Collider objectCollider, Renderer objectRenderer, Material originalMaterial)
    {
        // Wait for the ghost to leave the proximity
        while (Vector3.Distance(transform.position, objectCollider.transform.position) <= proximityDistance)
        {
            yield return null;  // Keep checking until the object is out of range
        }

        // Revert to the original material
        objectRenderer.material = originalMaterial;
    }
}
