using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;

    [SerializeField] private float groundDrag;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool _readyToJump = true;

    [Header("KeyBinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;

    [SerializeField] private LayerMask whatIsGround;
    private bool _grounded;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;

    private RaycastHit _slopeHit;
    private bool _exitingSlope;

    [SerializeField] private Transform orientation;

    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;

    private Vector3 _moveDirection;

    private Rigidbody _rb;

    private MovementState _movementState;

    private enum MovementState
    {
        Walking,
        Sprinting,
        Air
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, 
            playerHeight * 0.5f + 0.2f, whatIsGround);
        ProcessInput();

        StateHandler();

        if (_grounded)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;
        if (OnSlope())
            _rb.drag += 6;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void ProcessInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (!Input.GetKey(jumpKey) || !_readyToJump || !_grounded) return;

        _readyToJump = false;
        Jump();
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void StateHandler()
    {
        switch (_grounded)
        {
            case true when Input.GetKey(sprintKey):
                _movementState = MovementState.Sprinting;
                moveSpeed = sprintSpeed;
                break;
            case true:
                _movementState = MovementState.Walking;
                moveSpeed = walkSpeed;
                break;
            default:
                _movementState = MovementState.Air;
                break;
        }
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection() * (moveSpeed * 20f), ForceMode.Force);
            if (_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        switch (_grounded)
        {
            case true:
                _rb.AddForce(_moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
                break;
            case false:
                _rb.AddForce(_moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
                break;
        }

        _rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !_exitingSlope)
        {
            if (_rb.velocity.magnitude > moveSpeed)
                _rb.velocity = _rb.velocity.normalized * moveSpeed;
        }
        else
        {
            var velocity = _rb.velocity;
            var flatVel = new Vector3(velocity.x, 0f, velocity.z);

            if (!(flatVel.magnitude > moveSpeed)) return;
            var limitedVel = flatVel.normalized * moveSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        _exitingSlope = true;
        var velocity = _rb.velocity;
        velocity = new Vector3(velocity.x, 0f, velocity.z);
        _rb.velocity = velocity;

        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
        _exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (!Physics.Raycast(transform.position, Vector3.down,
                out _slopeHit, playerHeight * 0.5f + 0.3f)) return false;
        var angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
        return angle < maxSlopeAngle && angle != 0;

    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
    }
}