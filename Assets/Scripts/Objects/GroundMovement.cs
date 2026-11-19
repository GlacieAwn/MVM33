using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class GroundMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float _walkSpeed = 5.0f;
    [SerializeField] private float _runSpeed = 10.0f;
    [SerializeField] private float _jumpForce = 15.0f;
    [SerializeField] private float _jumpBufferTime = 0.15f;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _deceleration = 15f;

    [Header("Assignments")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private TMP_Text debugText;


    private bool _isRunning = false;
    private bool _isGrounded;
    private float _jumpBufferCounter;
    private float groundCheckDistance = 0.2f;
    private LayerMask groundLayer;

    private Vector2 _movementInput;

    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;

    private InputSystem_Actions _inputActions;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Ground.Jump.performed += OnJump;
    }

    void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Ground.Jump.performed -= OnJump;

    }

    void Update()
    {
        _movementInput = _inputActions.Ground.Movement.ReadValue<Vector2>();
        //float jumpInput = _inputActions.Ground.Jump.ReadValue<float>();



        // Update Debug Text
        debugText.text =
        "Player Values:\n" +
        $"Position: {transform.position}\n" +
        $"Rotation: {transform.rotation}\n" +
        $"Grouned: {_isGrounded}" +
        "";

    }

    void FixedUpdate()
    {

        Vector3 moveDirection = (transform.forward * _movementInput.y) + (transform.right * _movementInput.x);

        float accelRate = (_movementInput.magnitude > 0.01f) ? _acceleration : _deceleration;
        
        // For now, Set the player's rotation to the camera. This ensures the player is rotating when the camera rotates, and that the player's forward matches the camera's forward. Will find a better solution to this later...
        Vector3 flatForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up);

        // Lock it only to the X axis, not the Y.
        if (flatForward.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
        }

        Vector3 targetVelocity = moveDirection * _walkSpeed;

        Vector3 currentHorizontal = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        Vector3 newHorizontal = Vector3.MoveTowards(currentHorizontal, targetVelocity, accelRate * Time.deltaTime);



        _rb.linearVelocity = new Vector3(newHorizontal.x, _rb.linearVelocity.y, newHorizontal.z);

        _jumpBufferCounter -= Time.fixedDeltaTime;
        if(_jumpBufferCounter > 0f && _isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _jumpBufferCounter = 0f;
        }

        _isGrounded = false;

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {

        _jumpBufferCounter = _jumpBufferTime;
        // Handle Jumping


    }



}
