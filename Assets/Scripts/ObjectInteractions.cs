using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractions : MonoBehaviour
{
    // Reference to the player object (assign in Unity Inspector)
    public GameObject playerObject;

    // Proximity distance for the interaction
    [SerializeField]
    private float proximityDistance = 5.0f;

    // Shake parameters
    private Vector3 originalPosition;  // To store the original position of the object
    private float shakeDuration = 2.0f;  // How long the shake lasts
    private float shakeSpeed = 10.0f;  // Speed of the shake
    private float shakeAmplitude = 0.5f;  // How far the object rocks back and forth

    void Start()
    {
        // Store the original position of the object at the start
        originalPosition = transform.position;

        // Optional: Assign the player object automatically if not assigned in the Inspector
        if (playerObject == null)
        {
            playerObject = GameObject.FindWithTag("Player");
        }
    }

    void Update()
    {
        // Calculate distance between player and object
        float distance = Vector3.Distance(transform.position, playerObject.transform.position);

        // Check if player is within proximity distance
        if (distance <= proximityDistance)
        {
            Debug.Log("Player near object");

            // Wait for player to press the "H" key to trigger interaction
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("Player interacted with object");
                TriggerInteraction();  // Start the interaction (shake)
            }
        }
    }

    // Trigger the interaction (in this case, shaking)
    private void TriggerInteraction()
    {
        StartCoroutine(ShakeObject());  // Start the shaking coroutine
    }

    // Coroutine to handle the shaking behavior
    private IEnumerator ShakeObject()
    {
        float elapsedTime = 0.0f;  // Timer for how long the object has been shaking

        while (elapsedTime < shakeDuration)
        {
            // Calculate the new X position based on a sine wave for smooth shaking
            float newX = originalPosition.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmplitude;

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
