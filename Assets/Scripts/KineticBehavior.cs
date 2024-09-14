using System.Collections;
using UnityEngine;

public class KineticBehavior : MonoBehaviour
{
    private Rigidbody rb;   
    // Shake parameters (these can be adjusted per object)
    private Vector3 originalPosition;
    [SerializeField]
    private float _shakeDuration = 2.0f;  // How long the shake lasts
    [SerializeField]
    private float _shakeSpeed = 10.0f;    // Speed of the shake
    [SerializeField]
    private float _shakeAmplitude = 0.5f; // How far the object rocks back and forth

    //applying force for tossing object kinetically
    [SerializeField]
    private float _tossForce = 50f;

    void Start()
    {
        // Store the original position of the object at the start
        originalPosition = transform.position;
        
        //Get rigidbody component (must be attached to object)
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("Rigidbody NULL, tossing won't work. Attacj Rigidbody component");
        }

    }

    // This method can be called from ObjectInteractions.cs
    public void TriggerKineticAction()
    {
        StartCoroutine(ShakeObject());  // Start the shaking coroutine
    }

    public void TossObject()
    {
        if (rb != null)
        {
            // Create a random direction for the toss
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),   // Randomize horizontal direction (left/right)
                1f,                      // Always apply some upward force
                Random.Range(-1f, 1f)    // Randomize forward/backward direction
            ).normalized;                // Normalize to keep direction consistent

            // Apply a random force based on the _tossForce value and random direction
            rb.AddForce(randomDirection * _tossForce);

            Debug.Log("Object tossed in direction: " + randomDirection);
        }
    }

    // Coroutine to handle the shaking behavior
    private IEnumerator ShakeObject()
    {
        float elapsedTime = 0.0f;  // Timer for how long the object has been shaking

        while (elapsedTime < _shakeDuration)
        {
            // Calculate the new X position based on a sine wave for smooth shaking
            float newX = originalPosition.x + Mathf.Sin(Time.time * _shakeSpeed) * _shakeAmplitude;

            // Apply the new position, keeping Y and Z unchanged
            transform.position = new Vector3(newX, originalPosition.y, originalPosition.z);

            // Increase elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // After shaking, return the object to its original position
        transform.position = originalPosition;
    }
}
