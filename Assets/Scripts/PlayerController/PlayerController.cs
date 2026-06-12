using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private PlayerInput playerInput;
    private InputAction moveAction;

    [Header("Scanner Settings")]
    public LineRenderer lineRenderer;
    public Transform scanPoint;
    public float scanDistance = 3f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        
        // Make sure the laser is hidden at the start of the game
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
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

        // 2. AI Scanning Logic (Runs automatically every frame)
        FireAIScanner();
    }

    void FireAIScanner()
    {
        if (scanPoint == null || lineRenderer == null) return;

        // Enable the visual laser beam line
        lineRenderer.enabled = true;

        // Set the start of the laser right at our scan point
        lineRenderer.SetPosition(0, scanPoint.position);

        // Shoot a 2D Raycast straight out out from where the player is facing
        RaycastHit2D hit = Physics2D.Raycast(scanPoint.position, transform.right, scanDistance);

        if (hit.collider != null)
        {
            // If the ray hits a collider, stop the visual laser line exactly at the hit point
            lineRenderer.SetPosition(1, hit.point);

            // Check if the thing we hit has our Crop script component attached
            Crop crop = hit.collider.GetComponent<Crop>();
            if (crop != null)
            {
                // This console log proves the scanner is reading the crop data!
                Debug.Log("AI Scan Result: " + crop.cropStatus);
            }
        }
        else
        {
            // If the ray hits nothing, project the laser out into empty space at max distance
            Vector3 maxDistancePoint = scanPoint.position + (transform.right * scanDistance);
            lineRenderer.SetPosition(1, maxDistancePoint);
        }
    }
}