using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class GhostScript : MonoBehaviour
    {
        private Animator Anim;
        private CharacterController Ctrl;
        private Vector3 MoveDirection = Vector3.zero;

        // Movement control variables
        public float _speedBase = 4.0f;
        public float _speedMultiplier = 1.0f;
        public float _rotationSpeed = 720.0f;
        public bool _isFloating = false;

        // Gravity variables
        private float _gravity = -9.81f;
        private float _verticalVelocity = 0f;

        // dissolve and transparency
        [SerializeField]
        private SkinnedMeshRenderer[] MeshR;
        private float _dissolve_value = 1;
        private bool _dissolveFlg = false;
        private const int _maxHP = 3;
        private int _HP = _maxHP;
        private Text _HPtext;

        // Transparency and fade control
        [SerializeField]
        private float _fadeDuration = 2.0f;
        [SerializeField]
        private float _ghostTransparency = 0.2268818f;

        // Visibility logic
        [SerializeField]
        private float _visibilityDuration = 5.0f;  // Duration of visibility
        [SerializeField]
        private float _cooldownDuration = 5.0f;    // Duration of cooldown
        private bool _isOnCooldown = false;
        [SerializeField]
        private bool _isVisible = false;  // To track if ghost is currently visible
        private Coroutine _visibilityCoroutine = null;
        private bool _manualFadeOut = false; // NEW: Tracks if manual fade-out has been triggered

        void Start()
        {
            Anim = this.GetComponent<Animator>();
            Ctrl = this.GetComponent<CharacterController>();
            _HPtext = GameObject.Find("Canvas/_HP").GetComponent<Text>();
            _HPtext.text = "_HP " + _HP.ToString();

            // Apply the initial _ghostTransparency value when the game starts
            ApplyTransparency(_ghostTransparency);
        }

        void Update()
        {
            // Handle movement and visibility logic
            HandleMovement();
            HandleVisibility();
            PlayerAttack();
            ToggleFloating();
        }

        private void HandleVisibility()
        {
            // NEW: Check if the ghost is visible and we want to make it invisible manually
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_isOnCooldown)
            {
                // If ghost is already visible, fade back out manually
                if (_isVisible)
                {
                    _manualFadeOut = true;
                }
                else if (!_isVisible)
                {
                    if (_visibilityCoroutine != null)
                    {
                        StopCoroutine(_visibilityCoroutine);  // Stop any existing coroutine
                    }

                    // Start the visibility coroutine to handle fading in and back out
                    _visibilityCoroutine = StartCoroutine(FadeToOpaqueAndThenBack());
                }
            }
        }

        private IEnumerator FadeToOpaqueAndThenBack()
        {
            _isVisible = true;

            // Fade to opaque
            float elapsedTime = 0.0f;
            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                _ghostTransparency = Mathf.Lerp(0.0f, 1.0f, elapsedTime / _fadeDuration);
                ApplyTransparency(_ghostTransparency);
                yield return null;
            }

            // Stay visible until duration or manual fade-out
            float visibilityTimer = 0f;
            while (visibilityTimer < _visibilityDuration && !_manualFadeOut)
            {
                visibilityTimer += Time.deltaTime;
                yield return null;
            }

            // Reset manual fade-out flag
            _manualFadeOut = false;

            // Fade back to transparent
            elapsedTime = 0.0f;
            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                _ghostTransparency = Mathf.Lerp(1.0f, 0.0f, elapsedTime / _fadeDuration);
                ApplyTransparency(_ghostTransparency);
                yield return null;
            }

            // Mark the ghost as no longer visible
            _isVisible = false;

            // Start cooldown
            _isOnCooldown = true;
            yield return new WaitForSeconds(_cooldownDuration);
            _isOnCooldown = false;
        }

        private void ApplyTransparency(float transparencyValue)
        {
            foreach (var meshRenderer in MeshR)
            {
                foreach (var material in meshRenderer.materials)
                {
                    material.SetFloat("_GhostTransparency", transparencyValue); // Apply transparency
                }
            }
        }

        private void HandleMovement()
        {
            float moveHorizontal = 0;
            float moveVertical = 0;

            // Movement input handling
            if (Input.GetKey(KeyCode.UpArrow)) { moveVertical = 1f; }
            else if (Input.GetKey(KeyCode.DownArrow)) { moveVertical = -1f; }

            if (Input.GetKey(KeyCode.LeftArrow)) { moveHorizontal = -1f; }
            else if (Input.GetKey(KeyCode.RightArrow)) { moveHorizontal = 1f; }

            // Calculate the movement vector
            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            float speed = movement.magnitude;

            // Set speed in the animator
            Anim.SetFloat("Speed", speed);

            if (speed > 0)
            {
                Ctrl.Move(movement * _speedBase * _speedMultiplier * Time.deltaTime);
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
            }

            // Floating mechanics
            if (_isFloating)
            {
                if (Input.GetKey(KeyCode.Q)) { _verticalVelocity = _speedBase; }
                else if (Input.GetKey(KeyCode.E)) { _verticalVelocity = -_speedBase; }
                else { _verticalVelocity = 0f; }
            }
            else
            {
                ApplyGravity();  // Apply gravity if not floating
            }

            // Apply vertical movement (floating or gravity)
            Ctrl.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (!Ctrl.isGrounded)
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
            else
            {
                _verticalVelocity = 0f;
            }
        }

        private void ToggleFloating()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _isFloating = !_isFloating;
                if (!_isFloating)
                {
                    _verticalVelocity = 0f;
                    ApplyGravity();
                }
            }
        }

        private void PlayerAttack()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Anim.SetTrigger("Attack");
            }
        }

        private void PlayerDissolve()
        {
            if (_dissolveFlg)
            {
                _dissolve_value -= Time.deltaTime;
                foreach (var meshRenderer in MeshR)
                {
                    meshRenderer.material.SetFloat("_Dissolve", _dissolve_value);
                }

                if (_dissolve_value <= 0)
                {
                    Ctrl.enabled = false;
                }
            }
        }

        // New method added to check visibility from other scripts
        public bool IsVisible()
        {
            return _isVisible;
        }
    }
}
