using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float _speedBase = 4f; // Base speed of the player
    [SerializeField]
    private float _floatHorizontalSpeed = 5f; // Speed when floating
    [SerializeField]
    private float _floatVerticalSpeed = 4f; // Vertical speed when floating

    [SerializeField]
    private GameObject _plasmaPrefab; // Plasma projectile prefab
    [SerializeField]
    private float _FireRate = 0.5f; // Rate of fire
    [SerializeField]
    private float _canFire = -1f; // Time when player can fire again

    private Rigidbody _rb;
    private Vector3 _inputDirection;
    [SerializeField]
    private bool _isFloating = false;

    // Materials for transparency
    [SerializeField]
    private Material _transparentMaterial;  // TransparentPlayer_mat
    [SerializeField]
    private Material _opaqueMaterial; // opaquePlayer_mat

    private Renderer _playerRenderer; // Player's renderer

    // Visibility timer
    private float _visibilityDuration = 5f; // Duration of visibility
    private float _visibilityEndTime = 0f; // Time when visibility ends

    // Track if player is invisible
    [SerializeField]
    private bool _isInvisible = true;  // Player starts as invisible

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogError("Rigidbody is NULL.");
        }
        else
        {
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Ensures that the player does not pass through walls/ceilings when floating
        }

        // Cache renderer reference at start
        _playerRenderer = GetComponent<Renderer>();

        // Set player as transparent from the start
        _playerRenderer.material = _transparentMaterial; // start invisible
    }

    void Update()
    {
        // Check for floating toggle and calculate movement
        CalculateMovement();
        ToggleVisibility();
        ToggleFloating();
        FirePlasmaShot();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    // Toggle visibility of player | player starts invisible, hold shift to make visible for 5 seconds max
    private void ToggleVisibility()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerRenderer.material = _opaqueMaterial; // make player visible
            _visibilityEndTime = Time.time + _visibilityDuration; // set visibility end time
            _isInvisible = false;  // Player is visible
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _playerRenderer.material = _transparentMaterial; // make player invisible
            _isInvisible = true;  // Player is invisible
        }

        // Automatically revert player to invisible after 5 seconds
        if (Time.time >= _visibilityEndTime)
        {
            _playerRenderer.material = _transparentMaterial;
            _isInvisible = true;  // Player is invisible
        }
    }

    // Public method for NPC to check invisibility status
    public bool IsPlayerInvisible()
    {
        return _isInvisible;
    }

    private void ToggleFloating()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _isFloating = !_isFloating; // Toggle state

            if (_isFloating)
            {
                // Turn off gravity when floating
                _rb.useGravity = false;
            }
            else
            {
                // Re-enable gravity when walking
                _rb.useGravity = true;
            }
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (_isFloating)
        {
            // Floating: Add vertical movement (up and down)
            float upwardInput = 0;

            if (Input.GetKey(KeyCode.E)) // Move up
            {
                upwardInput = _floatVerticalSpeed;
            }
            else if (Input.GetKey(KeyCode.Q)) // Move down
            {
                upwardInput = -_floatVerticalSpeed;
            }

            // Include movement along all three axes
            _inputDirection = new Vector3(horizontalInput, upwardInput, verticalInput);
        }
        else
        {
            // Walking: Move only on the X and Z axes
            _inputDirection = new Vector3(horizontalInput, 0, verticalInput);
        }
    }

    private void MovePlayer()
    {
        float currentSpeed = _isFloating ? _floatHorizontalSpeed : _speedBase;
        Vector3 movement = _inputDirection * currentSpeed * Time.deltaTime;

        // Move the player
        _rb.MovePosition(_rb.position + movement);
    }

    private void FirePlasmaShot()

    // F key plasma fire 
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(_plasmaPrefab, transform.position + new Vector3(0.3f, 0, 0), Quaternion.identity);
            _canFire = Time.time + _FireRate;
        }
    }
}
