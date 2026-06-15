using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance; 

    public float moveSpeed = 5f;
    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("Scanner Settings")]
    public LineRenderer lineRenderer;
    public Transform scanPoint;
    public float scanDistance = 3f;
    private bool isScanning = false;

    [Header("UI Dashboard Panels")]
    public TextMeshProUGUI farmerStatusText;
    public GameObject droneReportPanel; // Drag your popup UI box panel here
    public TextMeshProUGUI droneReportText; // Drag the text inside that box here

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }

        if (droneReportPanel != null) droneReportPanel.SetActive(false);
    }

    void Update()
    {
        // 1. Movement logic
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }

        // 2. Read Toggle Input (Spacebar)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isScanning = !isScanning; // Toggle scan mode on/off
        }

        if (isScanning)
        {
            FireAIScanner();
        }
        else
        {
            if (lineRenderer != null) lineRenderer.enabled = false;
            if (farmerStatusText != null) farmerStatusText.text = "Farmer Scanner: Ready (Press Space to Scan)";
        }
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
                if (crop.cropStatus.Contains("CRITICAL"))
                {
                    SetLaserColor(Color.red); // Sick plant turns laser RED
                    if (farmerStatusText != null) farmerStatusText.text = "AI Warning: " + crop.cropStatus;
                }
                else
                {
                    SetLaserColor(Color.green); // Healthy plant turns laser GREEN
                    if (farmerStatusText != null) farmerStatusText.text = "AI Analytics: " + crop.cropStatus;
                }
            }
        }
        else
        {
            Vector3 maxDistancePoint = scanPoint.position + (transform.right * scanDistance);
            lineRenderer.SetPosition(1, maxDistancePoint);
            SetLaserColor(Color.white); // Empty space stays WHITE
            if (farmerStatusText != null) farmerStatusText.text = "AI Status: Scanning empty sector...";
        }
    }

    void SetLaserColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    // Called automatically by the drone when it finishes a complete lap!
    public void DisplayDroneReport(int healthyCount, int sickCount)
    {
        if (droneReportPanel == null || droneReportText == null) return;

        // Turn off player scanning while menu is open
        isScanning = false;
        if (lineRenderer != null) lineRenderer.enabled = false;

        // Fill out the text report inside the pop-up panel box
        droneReportText.text = $"<b><color=#00FF00>AI TELEMETRY LAP COMPLETE</color></b>\n\n" +
                               $"Healthy Crops Verified: {healthyCount}\n" +
                               $"Infections Detected: <color=red>{sickCount}</color>\n\n" +
                               $"Deploy the farmer to clear flagged targets!";
        
        droneReportPanel.SetActive(true); // Pop up on screen
        Time.timeScale = 0f; // Pause game physics until user accepts
    }

    // Connect this function to a UI Button inside your pop-up box panel
    public void CloseDroneReport()
    {
        if (droneReportPanel != null) droneReportPanel.SetActive(false);
        Time.timeScale = 1f; // Unpause the game simulation
    }
}