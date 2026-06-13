using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // <-- This allows us to control TextMeshPro objects in code!

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("Scanner Settings")]
    public LineRenderer lineRenderer;
    public Transform scanPoint;
    public float scanDistance = 3f;

    [Header("UI Reference")]
    public TextMeshProUGUI uiScanText; // <-- Empty slot for our UI Text

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }

        // Set initial UI text
        if (uiScanText != null)
        {
            uiScanText.text = "AI Scanner Status: System Standby";
        }
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }

        FireAIScanner();
    }

    void FireAIScanner()
    {
        if (scanPoint == null || lineRenderer == null) return;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, scanPoint.position);

        RaycastHit2D hit = Physics2D.Raycast(scanPoint.position, transform.right, scanDistance);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);

            Crop crop = hit.collider.GetComponent<Crop>();
            if (crop != null)
            {
                // Update our UI Text instantly on screen!
                if (uiScanText != null)
                {
                    uiScanText.text = "AI Analytics: " + crop.cropStatus;
                }
            }
        }
        else
        {
            Vector3 maxDistancePoint = scanPoint.position + (transform.right * scanDistance);
            lineRenderer.SetPosition(1, maxDistancePoint);

            // Reset UI text when scanning empty space
            if (uiScanText != null)
            {
                uiScanText.text = "AI Scanner Status: Scanning Field...";
            }
        }
    }
}