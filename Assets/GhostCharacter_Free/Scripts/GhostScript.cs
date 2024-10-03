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
        public float SpeedBase = 4.0f;
        public float SpeedMultiplier = 1.0f;
        public float RotationSpeed = 720.0f;
        public bool isFloating = false;  // Default is not floating, toggle with 'F'

        // Gravity variables
        private float gravity = -9.81f;
        private float verticalVelocity = 0f; // Tracks vertical movement, including gravity

        // dissolve
        [SerializeField] private SkinnedMeshRenderer[] MeshR; // For transparency effects
        private float Dissolve_value = 1;
        private bool DissolveFlg = false;  // Flag for dissolve effect
        private const int maxHP = 3;
        private int HP = maxHP;
        private Text HP_text;
        [SerializeField]
        private float fadeDuration = 2.0f; // Duration of fade effect
        [SerializeField]
        private float ghostTransparency = 0.2268818f; // Start partially transparent

        void Start()
        {
            Anim = this.GetComponent<Animator>();
            Ctrl = this.GetComponent<CharacterController>();
            HP_text = GameObject.Find("Canvas/HP").GetComponent<Text>();
            HP_text.text = "HP " + HP.ToString();
        }

        void ApplyTransparency(float transparencyValue)
        {
            // Apply the transparency value to the ghost's materials
            foreach (var meshRenderer in MeshR)
            {
                foreach (var material in meshRenderer.materials)
                {
                    material.SetFloat("_GhostTransparency", transparencyValue); // Set shader transparency
                }
            }
        }

        void Update()
        {
            // Movement and visibility control
            HandleMovement();
            HandleVisibility();

            // Handle attack and other gameplay elements
            PlayerAttack();

            // Toggle floating on/off
            ToggleFloating();
        }

        private void HandleMovement()
        {
            float moveHorizontal = 0;
            float moveVertical = 0;

            // Use arrow keys for movement
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveVertical = 1f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                moveVertical = -1f;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveHorizontal = -1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveHorizontal = 1f;
            }

            // Calculate the movement vector
            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            float speed = movement.magnitude;  // Speed based on movement

            // Set the Speed parameter in the Animator
            Anim.SetFloat("Speed", speed);

            if (speed > 0)
            {
                // Move and rotate character
                Ctrl.Move(movement * SpeedBase * SpeedMultiplier * Time.deltaTime);

                // Smoothly rotate the ghost in the direction of movement
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
            }

            // Floating mechanics (if enabled)
            if (isFloating)
            {
                if (Input.GetKey(KeyCode.Q)) // Float upwards
                {
                    verticalVelocity = SpeedBase; // Move upwards
                }
                else if (Input.GetKey(KeyCode.E)) // Float downwards
                {
                    verticalVelocity = -SpeedBase; // Move downwards
                }
                else
                {
                    verticalVelocity = 0f; // Stop vertical movement if no input
                }
            }
            else
            {
                ApplyGravity(); // Apply gravity when not floating
            }

            // Apply vertical movement (floating or gravity)
            Ctrl.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        }


        //---------------------------------------------------------------------
        // Apply Gravity (when floating is off)
        //---------------------------------------------------------------------
        private void ApplyGravity()
        {
            // Apply gravity if the ghost is not grounded and not floating
            if (!Ctrl.isGrounded)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
            else
            {
                verticalVelocity = 0f; // Reset vertical velocity when grounded
            }
        }

        //---------------------------------------------------------------------
        // Toggle floating on/off with 'F' key
        //---------------------------------------------------------------------
        private void ToggleFloating()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isFloating = !isFloating; // Toggle floating on or off
                if (!isFloating)
                {
                    // When floating is toggled off, apply gravity immediately to bring the character down
                    verticalVelocity = 0f; // Reset vertical velocity for smooth transition
                    ApplyGravity();
                }
            }
        }

        //---------------------------------------------------------------------
        // Handle Visibility (transparency)
        //---------------------------------------------------------------------
        private void HandleVisibility()
        {
            // Press LeftShift to make the ghost fade in (turn opaque)
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(FadeToOpaque());  // Start the fading effect
            }
        }

        private IEnumerator FadeToOpaque()
        {
            float elapsedTime = 0.0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;  // Keep track of time
                ghostTransparency = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeDuration); // Change transparency gradually

                // Apply the transparency value to the material
                foreach (var meshRenderer in MeshR)
                {
                    foreach (var material in meshRenderer.materials)
                    {
                        material.SetFloat("_GhostTransparency", ghostTransparency); // Set shader transparency
                    }
                }
                yield return null;  // Wait until next frame
            }
        }

        //---------------------------------------------------------------------
        // Handle Attack Logic with Trigger
        //---------------------------------------------------------------------
        private void PlayerAttack()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //Debug.Log("Player attack");
                Anim.SetTrigger("Attack"); // Trigger attack
               
            }
        }

        // Reset the attack trigger after a short delay
       


        //---------------------------------------------------------------------
        // Handle Dissolve Logic (optional)
        //---------------------------------------------------------------------
        private void PlayerDissolve()
        {
            if (DissolveFlg)
            {
                Dissolve_value -= Time.deltaTime;
                foreach (var meshRenderer in MeshR)
                {
                    meshRenderer.material.SetFloat("_Dissolve", Dissolve_value); // Dissolve shader effect
                }

                if (Dissolve_value <= 0)
                {
                    Ctrl.enabled = false; // Disable control on full dissolve
                }
            }
        }
    }
}
