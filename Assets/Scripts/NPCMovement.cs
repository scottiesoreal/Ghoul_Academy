using UnityEngine;

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

    //startled behavior
    //isStartled = False;
    private float _jumpForce = 5f;
    private Rigidbody _rb;

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
    }

    void Update()
    {
        // Smoothly rotate towards the target rotation
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

    public void StartleJump()
    {
        if (_rb != null)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse); // Apply an upward force
            Debug.Log("NPC was startled");
        }
    }
    
    private void SwitchRotationDirection()
    {
        // Switch between left, right, and forward
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
