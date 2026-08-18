using UnityEngine;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardSpeed = 20f;
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;

    [Header("Visual Banking")]
    [SerializeField] private Transform visualModel;
    [SerializeField] private float maxRoll = 30f;
    [SerializeField] private float rotationSmoothing = 180f;

    private InputSystem_Actions inputActions;
    private Quaternion visualModelNeutralRotation;

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
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        Vector2 aim = inputActions.Player.Aim.ReadValue<Vector2>();

        Vector3 localMovement = new Vector3(
            aim.x * horizontalSpeed,
            aim.y * verticalSpeed,
            forwardSpeed);

        transform.Translate(localMovement * Time.deltaTime, Space.World);

        UpdateVisualBanking(aim.x);
    }

    private void UpdateVisualBanking(float aimX)
    {
        if (visualModel == null)
        {
            return;
        }

        // Negative Aim X is a left bank. Only the visual model rolls around local Z.
        Quaternion targetRotation = visualModelNeutralRotation * Quaternion.Euler(aimX * maxRoll, 0f, 0f);
        visualModel.localRotation = Quaternion.RotateTowards(
            visualModel.localRotation,
            targetRotation,
            rotationSmoothing * Time.deltaTime);
    }
}
