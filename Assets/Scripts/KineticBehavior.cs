using System.Collections;
using UnityEngine;

public class KineticBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 originalPosition;

    [SerializeField]
    private float _tossForce = 50f;

    void Start()
    {
        // Store the original position of the object at the start
        originalPosition = transform.position;

        // Get Rigidbody component (must be attached to object)
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("Rigidbody NULL, tossing won't work. Attach Rigidbody component.");
        }
    }

    // This method can be called to test the interaction
    public void TriggerKineticAction()
    {
        Debug.Log("Item shook: " + gameObject.name); // Confirmation log for shaking trigger
        // StartCoroutine(ShakeObject());  // This is commented out for testing
    }

    public void TossObject()
    {
        if (rb != null)
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),   // Randomize horizontal direction
                1f,                      // Apply upward force
                Random.Range(-1f, 1f)    // Randomize forward/backward direction
            ).normalized;

            rb.AddForce(randomDirection * _tossForce);
            Debug.Log("Object tossed in direction: " + randomDirection);
        }
    }

    // Coroutine for shaking (commented out for now)
    /*
    private IEnumerator ShakeObject()
    {
        float elapsedTime = 0.0f;  
        while (elapsedTime < _shakeDuration)
        {
            float newX = originalPosition.x + Mathf.Sin(Time.time * _shakeSpeed) * _shakeAmplitude;
            transform.position = new Vector3(newX, originalPosition.y, originalPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }
    */
}
