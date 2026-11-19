using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ShipShooting : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform weaponSpawn;
    [SerializeField] private float projectileSpeed = 100f;
    [SerializeField] private float projectileLifetime = 5f;

    [Header("Charge Shot")]
    [SerializeField] private Projectile chargeProjectilePrefab;
    [SerializeField] private Transform chargeWeaponSpawn;
    [SerializeField] private bool hasChargeShot;
    [FormerlySerializedAs("chargeSpawnDelay")]
    [SerializeField] private float chargeVisualSpawnDelay = 0.2f;
    [SerializeField] private float chargeShotThreshold = 0.2f;
    [SerializeField] private float maxChargeTime = 1f;
    [SerializeField] private Vector3 minimumChargeScale = Vector3.one;
    [SerializeField] private Vector3 maximumChargeScale = Vector3.one * 3f;

    [SerializeField] private RectTransform targetCursor;
    [SerializeField] private Camera gameplayCamera;

    private const float AimRayDistance = 1000f;

    private InputSystem_Actions inputActions;
    private TargetCursor targetCursorController;
    private Projectile chargeProjectile;
    private float currentCharge;
    private bool isCharging;

    public float CurrentCharge => currentCharge;
    public bool IsCharging => isCharging;
    public bool HasChargeShot => hasChargeShot;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        if (targetCursor == null)
        {
            TargetCursor cursor = FindAnyObjectByType<TargetCursor>();
            if (cursor != null)
            {
                targetCursor = cursor.GetComponent<RectTransform>();
            }
        }

        if (targetCursor != null)
        {
            targetCursorController = targetCursor.GetComponent<TargetCursor>();
        }

        if (gameplayCamera == null)
        {
            gameplayCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        inputActions.Ship.Shoot.started += BeginCharge;
        inputActions.Ship.Shoot.canceled += ReleaseShot;
        inputActions.Ship.Enable();
    }

    private void OnDisable()
    {
        inputActions.Ship.Shoot.started -= BeginCharge;
        inputActions.Ship.Shoot.canceled -= ReleaseShot;
        inputActions.Ship.Disable();

        if (chargeProjectile != null)
        {
            Destroy(chargeProjectile.gameObject);
        }
    }

    private void Update()
    {
        if (!isCharging)
        {
            return;
        }

        currentCharge = Mathf.Min(currentCharge + Time.deltaTime, maxChargeTime);

        if (hasChargeShot && chargeProjectile == null && currentCharge >= chargeVisualSpawnDelay)
        {
            SpawnChargeVisual();
        }

        UpdateChargeVisual();
    }

    private void BeginCharge(InputAction.CallbackContext context)
    {
        isCharging = true;
        currentCharge = 0f;
    }

    private void ReleaseShot(InputAction.CallbackContext context)
    {
        if (!isCharging)
        {
            return;
        }

        isCharging = false;

        if (hasChargeShot && currentCharge >= chargeShotThreshold && chargeProjectilePrefab != null && chargeWeaponSpawn != null)
        {
            FireChargeShot();
        }
        else
        {
            if (chargeProjectile != null)
            {
                Destroy(chargeProjectile.gameObject);
            }

            FireNormalShot();
        }

        chargeProjectile = null;
    }

    private void SpawnChargeVisual()
    {
        if (chargeProjectilePrefab == null || chargeWeaponSpawn == null)
        {
            return;
        }

        chargeProjectile = Instantiate(chargeProjectilePrefab, chargeWeaponSpawn.position, chargeWeaponSpawn.rotation);
        UpdateChargeVisual();
    }

    private void UpdateChargeVisual()
    {
        if (chargeProjectile == null || chargeWeaponSpawn == null)
        {
            return;
        }

        float chargePercent = currentCharge / Mathf.Max(maxChargeTime, Mathf.Epsilon);
        chargeProjectile.transform.SetPositionAndRotation(chargeWeaponSpawn.position, Quaternion.LookRotation(transform.forward));
        chargeProjectile.transform.localScale = Vector3.Lerp(minimumChargeScale, maximumChargeScale, chargePercent);
    }

    private void FireChargeShot()
    {
        if (chargeProjectile == null)
        {
            chargeProjectile = Instantiate(chargeProjectilePrefab, chargeWeaponSpawn.position, Quaternion.LookRotation(transform.forward));
        }

        chargeProjectile.transform.SetPositionAndRotation(chargeWeaponSpawn.position, Quaternion.LookRotation(transform.forward));
        chargeProjectile.Launch(transform.forward, projectileSpeed, projectileLifetime);
    }

    private void FireNormalShot()
    {
        if (projectilePrefab == null || weaponSpawn == null || targetCursorController == null)
        {
            Debug.Log("FireNormalShot: missing reference");
            return;
        }

        Debug.Log("FireNormalShot: starting");

        Ray cursorRay = targetCursorController.GetAimRay();

        Debug.Log($"Aim ray: origin={cursorRay.origin}, direction={cursorRay.direction}");

        Vector3 targetPoint = cursorRay.GetPoint(AimRayDistance);
        Vector3 firingDirection = (targetPoint - weaponSpawn.position).normalized;

        Debug.Log($"Firing direction: {firingDirection}");

        Projectile projectile = Instantiate(
            projectilePrefab,
            weaponSpawn.position,
            Quaternion.LookRotation(firingDirection)
        );

        Debug.Log("Projectile instantiated");

        projectile.Launch(firingDirection, projectileSpeed, projectileLifetime);
    }
}
