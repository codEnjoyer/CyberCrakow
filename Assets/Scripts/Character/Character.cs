﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace myStateMachine
{
    public class Character : MonoBehaviour
    {
        float distToGround = 0;
        public StandingState standing;
        public SprintingState sprinting;
        public JumpingState jumping;
        public WalkingState walking;
        public StateMachine movementSM;
        public SlidingState slide;
        public AirState air;
        [Header("Movement")]
        public float walkingSpeed;
        public float sprintSpeed;

        public float groundDrag;

        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplyer;
        public bool readyToJump = true;
        public float startYScale;
        public float slideYScale;
        public float slideForce = 5f;

        [Header("Keybinds")]
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode sprintKey = KeyCode.LeftShift;

        [Header("Ground Check")]
        public float playerHeight;
        public static LayerMask whatIsGround;


        [Header("Slope Handling")]
        public float maxSlopeAngle;
        private RaycastHit slopeHit;
        public bool exitingSlope;

        public Transform orientation;

        Vector3 moveDirection;

        Rigidbody rb;
        static List<Collider> groundTouchPoints = new List<Collider>();
        public float moveSpeed {get;set;}

        public bool IsJumpPressed { get; set; }
        public bool IsSprintPressed { get; set; }
        public bool IsDuckPressed { get; set; }
        public Vector2 playerInput { get; set; }
        [HideInInspector] public StaminaController staminaController;
        [HideInInspector] public HealthController healthController;
        public PlayerInput input;
        [SerializeField] Camera fpsCam; 

        private void Awake()
        {
            input = new PlayerInput();

            input.Player.Jump.performed += Jump_performed;
            input.Player.Sprint.performed += Sprint_performed;
        }


        private void Sprint_performed(InputAction.CallbackContext obj)
        {
            IsSprintPressed = true;
        }

        private void Jump_performed(InputAction.CallbackContext obj)
        {
            IsJumpPressed = true;
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }
        private void Start()
        {
            staminaController = GetComponent<StaminaController>();
            healthController = GetComponent<HealthController>();
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            movementSM = new StateMachine();
            moveSpeed = walkingSpeed;
            distToGround = GetComponent<Collider>().bounds.extents.y;
            standing = new StandingState(this, movementSM);
            jumping = new JumpingState(this, movementSM);
            sprinting = new SprintingState(this, movementSM);
            walking = new WalkingState(this, movementSM);
            air = new AirState(this, movementSM);
            slide = new SlidingState(this, movementSM);
            movementSM.Initialize(standing);
            startYScale = transform.localScale.y;
        }
        private void Update()
        {
            movementSM.CurrentState.HandleInput();

            movementSM.CurrentState.LogicUpdate();

        }

        private void FixedUpdate()
        {
            movementSM.CurrentState.PhysicsUpdate();
        }

        public bool CheckIfGrounded()
        {
            //Debug.Log(Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f));
            //return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
        }

        public void MovePlayer(float moveSpeed)
        {
            //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            moveDirection = orientation.forward * playerInput.y + orientation.right * playerInput.x;
            if (OnSlope() && !exitingSlope)
            {
                moveDirection = GetSlopeMoveDirection();
                Debug.DrawRay(transform.position, GetSlopeMoveDirection(),Color.green,1f);
                rb.AddForce(moveDirection * moveSpeed * 20f, ForceMode.Force);
                rb.drag = groundDrag + 6;
            }
            if (!OnSlope())
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
                rb.drag = groundDrag;
            }
            rb.useGravity = !OnSlope();            
        }
        public void AirMovement( float moveSpeed)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplyer, ForceMode.Force);
            rb.drag = 0;
            rb.useGravity = true;
            if (OnSlope()&& !exitingSlope)
            {
                rb.AddForce(-transform.up * 1, ForceMode.Impulse);
            }
        }
        public void SpeedControl()
        {
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                    rb.velocity = rb.velocity.normalized * moveSpeed;
            }
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }

        }

        public void Jump()
        {
            exitingSlope = true;
            readyToJump = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        public void ResetJump()
        {
            readyToJump = true;
            exitingSlope = false;
        }

        public void StopMovement()
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }
            return false;
        }
        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }
        public bool IdleCheck()
        {
            if (rb.velocity.magnitude == 0)
                return true;
            return false;
        }

        public void StartSlide()
        {
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            if (OnSlope())
                rb.drag -= 7;
            rb.AddForce(moveDirection.normalized * slideForce*4f, ForceMode.Impulse);
        }
        public void SlideMovement()
        {
            rb.AddForce(moveDirection.normalized * slideForce, ForceMode.Force);
            rb.AddForce(Vector3.down * 4f, ForceMode.Impulse);
            if (rb.velocity.magnitude <= 2f)
                movementSM.ChangeState(walking);
        }
    }
}