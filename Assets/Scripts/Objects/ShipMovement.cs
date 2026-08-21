using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardSpeed = 0f;
    [SerializeField] private float maxForwardSpeed = 20f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 20f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float turnSpeed = 10f;

    [Header("Visual Banking")]
    [SerializeField] private Transform visualModel;
    [SerializeField] private float maxRoll = 30f;
    [SerializeField] private float maxPitch = 30f;
    [SerializeField] private float rotationSmoothing = 180f;

    private InputSystem_Actions inputActions;
    private Quaternion visualModelNeutralRotation;
    private bool throttleActive;
    private bool brakeActive;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        if (visualModel != null)
        {
            visualModelNeutralRotation = visualModel.localRotation;
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Throttle.performed += ToggleThrottle;
        inputActions.Player.Brake.performed += ToggleBrake;
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Throttle.performed -= ToggleThrottle;
        inputActions.Player.Brake.performed -= ToggleBrake;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        Vector2 aim = inputActions.Player.Aim.ReadValue<Vector2>();

        UpdateForwardSpeed();

        transform.Rotate(0f, aim.x * turnSpeed * Time.deltaTime, 0f, Space.Self);

        Vector3 movement = transform.forward * forwardSpeed;
        movement += Vector3.up * (aim.y * verticalSpeed);
        transform.position += movement * Time.deltaTime;

        UpdateVisualBanking(aim.x, aim.y);
    }

    private void ToggleThrottle(InputAction.CallbackContext context)
    {
        throttleActive = !throttleActive;
        brakeActive = false;
    }

    private void ToggleBrake(InputAction.CallbackContext context)
    {
        brakeActive = !brakeActive;
        throttleActive = false;
    }

    private void UpdateForwardSpeed()
    {
        if (throttleActive)
        {
            forwardSpeed = Mathf.Min(forwardSpeed + acceleration * Time.deltaTime, maxForwardSpeed);
        }
        else if (brakeActive)
        {
            forwardSpeed = Mathf.Max(forwardSpeed - deceleration * Time.deltaTime, 0f);
        }
    }

    private void UpdateVisualBanking(float aimX, float aimY)
    {
        if (visualModel == null)
        {
            return;
        }

        // Visual model rolls on X and pitches on Z.
        Quaternion targetRotation = visualModelNeutralRotation * Quaternion.Euler(aimX * maxRoll, 0f, -aimY * maxPitch);
        visualModel.localRotation = Quaternion.RotateTowards(visualModel.localRotation, targetRotation, rotationSmoothing * Time.deltaTime);
    }
}
