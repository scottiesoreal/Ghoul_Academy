using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 50f;  // Speed of rotation

    private Quaternion _leftRotation;
    private Quaternion _rightRotation;
    private Quaternion _forwardRotation;

    private Quaternion _targetRotation; // The direction NPC is currently rotating towards
    private float _rotationDelay = 2f;  // Time to pause after rotating
    private float _rotationTimer = 0f;

    private enum LookDirection { Forward, Left, Right }
    private LookDirection _currentDirection = LookDirection.Forward;

    // Startled behavior
    [SerializeField]
    private bool _isStartled = false; // Track if the NPC is startled
    private float _jumpForce = 5f;
    private Rigidbody _rb;

    // New NavMeshAgent variables
    private NavMeshAgent _navMeshAgent;
    public Transform[] exitPoints;  // Array to store exit points in the house
    public Transform churchLocation; // Reference to the church

    private bool _runToChurch = false; // Flag to indicate if running to church

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("NPC Rigidbody component is missing!");
        }

        // Define the rotation angles for forward, left, and right
        _forwardRotation = transform.rotation;
        _leftRotation = Quaternion.Euler(0, -90, 0);   // 90 degrees to the left
        _rightRotation = Quaternion.Euler(0, 90, 0);   // 90 degrees to the right
        _targetRotation = _forwardRotation;  // Start by facing forward

        // Initialize NavMeshAgent for runaway behavior
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing!");
        }
    }

    void Update()
    {
        // Smoothly rotate towards the target rotation if not startled
        if (!_isStartled)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

            // Check if the NPC has reached its target rotation
            if (Quaternion.Angle(transform.rotation, _targetRotation) < 1f)
            {
                // Pause before moving to the next rotation
                _rotationTimer += Time.deltaTime;
                if (_rotationTimer > _rotationDelay)
                {
                    SwitchRotationDirection();
                    _rotationTimer = 0f;
                }
            }
        }

        // If the NPC is startled, make it run to the nearest exit or the church
        if (_isStartled)
        {
            if (_runToChurch)
            {
                RunToChurch();  // New method to run to the church
            }
            else
            {
                RunToExit();
            }
        }
    }

    public void StartleJump()
    {
        if (_rb != null)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse); // Apply an upward force
            Debug.Log("NPC was startled and jumped.");
        }
    }

    // Startled NPC runaway behavior
    public void StartleNPC(bool runToChurch = false)
    {
        _isStartled = true;
        _runToChurch = runToChurch; // Decide if running to church or exit
        Debug.Log("NPC has been startled and is now running!");
    }

    // Make the NPC run to the nearest exit
    public void RunToExit()
    {
        Debug.Log("RunToExit function called!");

        // Find the nearest exit point
        Transform nearestExit = GetNearestExit();

        if (nearestExit != null)
        {
            // Set the destination for the NPC to run to the nearest exit
            Debug.Log("Running to the nearest exit: " + nearestExit.name);
            _navMeshAgent.SetDestination(nearestExit.position);
        }
    }

    // Make the NPC run to the church
    public void RunToChurch()
    {
        Debug.Log("NPC is running to the church at: " + churchLocation.position);
        _navMeshAgent.SetDestination(churchLocation.position);
    }

    // Find the closest exit from the array of exits
    private Transform GetNearestExit()
    {
        Transform nearestExit = null;
        float closestDistance = Mathf.Infinity;

        // Loop through all exit points and find the nearest one
        foreach (Transform exit in exitPoints)
        {
            float distance = Vector3.Distance(transform.position, exit.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestExit = exit;
            }
        }

        return nearestExit;
    }

    // Detect when the NPC reaches the exit point
    private void OnTriggerEnter(Collider other)
    {
        // Once the NPC reaches the exit (exit points should have a collider and "Exit" tag), it stops
        if (other.CompareTag("Exit"))
        {
            Debug.Log("NPC has reached the exit.");
            _navMeshAgent.isStopped = true;
            Debug.Log("NPC has exited the house!");
        }
    }

    private void SwitchRotationDirection()
    {
        // Switch between left, right, and forward if not startled
        if (_currentDirection == LookDirection.Forward)
        {
            _targetRotation = _leftRotation;
            _currentDirection = LookDirection.Left;
        }
        else if (_currentDirection == LookDirection.Left)
        {
            _targetRotation = _rightRotation;
            _currentDirection = LookDirection.Right;
        }
        else if (_currentDirection == LookDirection.Right)
        {
            _targetRotation = _forwardRotation;
            _currentDirection = LookDirection.Forward;
        }
    }
}
