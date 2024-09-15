using System.Collections;
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

    // Visibility and Cooldown timers
    private float _visibilityDuration = 5f; // Maximum duration of visibility
    private float _cooldownDuration = 5f;  // Fixed cooldown duration
    private float _visibilityStartTime = 0f; // Tracks when visibility started

    // Track if player is invisible, on cooldown, or currently visible
    [SerializeField]
    private bool _isInvisible = true;  // Player starts as invisible
    [SerializeField]
    private bool _isOnCooldown = false; // Whether the player is on cooldown
    [SerializeField]
    private bool _isVisible = false; // Whether the player is currently visible

    private Coroutine _cooldownCoroutine; // Reference to the running cooldown coroutine

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
        HandleVisibility();  // Updated method for handling visibility with fixed cooldown
        ToggleFloating();
        FirePlasmaShot();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    // Visibility handler with fixed 5-second cooldown
    private void HandleVisibility()
    {
        // If the player is not on cooldown and is not visible, they can become visible
        if (!_isOnCooldown && !_isVisible)
        {
            // Activate visibility when holding shift
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _playerRenderer.material = _opaqueMaterial; // Make player visible
                _visibilityStartTime = Time.time; // Record the start time
                _isInvisible = false;  // Player is visible
                _isVisible = true; // Player is currently visible
                Debug.Log("Player became visible.");
            }
        }

        // If the player is visible, check when to turn off visibility
        if (_isVisible)
        {
            // Stop visibility when releasing shift or after max visibility duration
            if (Input.GetKeyUp(KeyCode.LeftShift) || Time.time >= _visibilityStartTime + _visibilityDuration)
            {
                BecomeInvisible();  // Turn invisible and trigger fixed cooldown
            }
        }
    }

    // Method to turn the player invisible and start the fixed cooldown
    private void BecomeInvisible()
    {
        _playerRenderer.material = _transparentMaterial; // Make player invisible
        _isInvisible = true;  // Player is invisible
        _isVisible = false;  // Player is no longer visible

        Debug.Log("Cooldown started for: " + _cooldownDuration + " seconds.");

        // Start the cooldown coroutine
        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine); // Stop any existing cooldowns
        }
        _cooldownCoroutine = StartCoroutine(StartCooldownTimer());
    }

    // Coroutine to handle the fixed 5-second cooldown
    private IEnumerator StartCooldownTimer()
    {
        _isOnCooldown = true;

        // Wait for the fixed cooldown duration (5 seconds)
        yield return new WaitForSeconds(_cooldownDuration);

        _isOnCooldown = false; // Cooldown ends, player can become visible again
        Debug.Log("Cooldown ended. Player can become visible again.");
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
