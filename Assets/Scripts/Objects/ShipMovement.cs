using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ShipMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 20f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float verticalSmoothSpeed = 5f;
    [SerializeField] private float turnSpeed = 10f;

    [Header("Visual Banking")]
    [SerializeField] private Transform visualModel;
    [SerializeField] private float maxRoll = 30f;
    [SerializeField] private float maxPitch = 30f;
    [SerializeField] private float rotationSmoothing = 180f;

    [Header("Debug Values")]
    [SerializeField] private TMP_Text debugText;

    private InputSystem_Actions inputActions;
    private Quaternion visualModelNeutralRotation;
    private bool throttleActive;
    private bool brakeActive;
    private float currentSpeed = 0f;
    private float currentVerticalSpeed = 0f;
    private ShipShooting shipShooting;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        shipShooting = GetComponent<ShipShooting>();

        if (visualModel != null)
        {
            visualModelNeutralRotation = visualModel.localRotation;
        }
    }

    private void OnEnable()
    {
        inputActions.Ship.Throttle.performed += ToggleThrottle;
        inputActions.Ship.Brake.performed += ToggleBrake;
        inputActions.Ship.Enable();
    }

    private void OnDisable()
    {
        inputActions.Ship.Throttle.performed -= ToggleThrottle;
        inputActions.Ship.Brake.performed -= ToggleBrake;
        inputActions.Ship.Disable();
    }

    private void Update()
    {
        Vector2 aim = inputActions.Ship.Aim.ReadValue<Vector2>();

        UpdatecurrentSpeed();

        transform.Rotate(0f, aim.x * turnSpeed * Time.deltaTime, 0f, Space.Self);

        float speedRatio = currentSpeed / maxSpeed;
        float targetVerticalSpeed = aim.y * verticalSpeed * speedRatio;
        currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, targetVerticalSpeed, verticalSmoothSpeed * Time.deltaTime);

        Vector3 movement = transform.forward * currentSpeed;
        movement += Vector3.up * currentVerticalSpeed;
        transform.position += movement * Time.deltaTime;

        UpdateVisualBanking(aim.x, aim.y);
        UpdateDebugText();
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

    private void UpdatecurrentSpeed()
    {
        if (throttleActive)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        }
        else if (brakeActive)
        {
            currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.deltaTime, 0f);
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

    private void UpdateDebugText()
    {
        float currentCharge = shipShooting != null ? shipShooting.CurrentCharge : 0f;
        bool isCharging = shipShooting != null && shipShooting.IsCharging;
        bool hasChargeShot = shipShooting != null && shipShooting.HasChargeShot;

        debugText.text = 
        "Ship Values:\n" +
        $"Current Speed: {currentSpeed}\n" +
        $"Position: {transform.position}\n" +
        $"Rotation: {transform.rotation}\n" +
        $"Current Charge: {currentCharge}\n" +
        $"Is Charging: {isCharging}\n" +
        $"Has Charge Shot: {hasChargeShot}\n" +
        "";
    }
}
