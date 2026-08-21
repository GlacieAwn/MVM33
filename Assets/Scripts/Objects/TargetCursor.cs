using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float moveSpeed = 500f;
    [SerializeField] private float horizontalRange = 300f; // random value for now, will tweak later
    [SerializeField] private float verticalRange = 200f; // random value for now, will tweak later

    private InputSystem_Actions input;

    private RectTransform rectTransform;

    private Vector2 cursorVelocity;

	private void Awake()
	{
        input = new InputSystem_Actions();
		rectTransform = GetComponent<RectTransform>();
	}

    private void OnEnable()
    {
        input.Enable();
    }

    private void Update()
    {
        Vector2 aimInput = input.Player.Aim.ReadValue<Vector2>();
        Debug.Log(aimInput);

        rectTransform.anchoredPosition += aimInput * moveSpeed * Time.deltaTime;

        Vector2 targetPosition = new Vector2(aimInput.x * horizontalRange, -aimInput.y * verticalRange);

        rectTransform.anchoredPosition = Vector2.SmoothDamp(rectTransform.anchoredPosition, targetPosition, ref cursorVelocity, 0.1f);
    }
}
