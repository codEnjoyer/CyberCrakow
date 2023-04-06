using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myStateMachine
{
    public class Character : MonoBehaviour
    {
        float distToGround = 0;
        public StandingState standing;
        public SprintingState sprinting;
        public JumpingState jumping;
        public StateMachine movementSM;
        [Header("Movement")]
        public float walkingSpeed;
        public float sprintSpeed;

        public float groundDrag;

        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplyer;
        public bool readyToJump = true;

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

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            movementSM = new StateMachine();
            moveSpeed = walkingSpeed;
            distToGround = GetComponent<Collider>().bounds.extents.y;
            standing = new StandingState(this, movementSM);
            jumping = new JumpingState(this, movementSM);
            sprinting = new SprintingState(this, movementSM);
            movementSM.Initialize(standing);
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
            return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            //return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        }

        public void MovePlayer(float horizontalInput, float verticalInput,bool grounded,float moveSpeed)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            if (OnSlope() && !exitingSlope)
            {
                Debug.Log("on slope");
                rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
                rb.drag = groundDrag + 6;
                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }
            if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplyer, ForceMode.Force);
            rb.drag = groundDrag;
            rb.useGravity = !OnSlope();            
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
    }
}