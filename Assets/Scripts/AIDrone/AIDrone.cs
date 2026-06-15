using UnityEngine;

public class AIDrone : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 4f;
    public float scanDownDistance = 2f;
    
    private Vector3 startPosition;
    private bool movingRight = true;

    // Analytics counters
    private int healthyPlantsFound = 0;
    private int sickPlantsFound = 0;
    private GameObject lastScannedCrop;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float rightBoundary = startPosition.x + patrolDistance;
        float leftBoundary = startPosition.x - patrolDistance;

        // Track turning frames to know when a round trip completes
        bool justTurned = false;

        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
                justTurned = true; // Finished heading right
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
                justTurned = true; // Finished heading left (Complete round trip!)
            }
        }

        // If drone completed its patrol lap, display the data report popup!
        if (justTurned && (healthyPlantsFound > 0 || sickPlantsFound > 0))
        {
            PlayerController.instance.DisplayDroneReport(healthyPlantsFound, sickPlantsFound);
            // Reset counters for the next flight session
            healthyPlantsFound = 0;
            sickPlantsFound = 0;
        }

        // Downward Scan Analysis
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, scanDownDistance);
        Debug.DrawRay(transform.position, Vector2.down * scanDownDistance, Color.yellow);

        if (hit.collider != null)
        {
            Crop crop = hit.collider.GetComponent<Crop>();
            if (crop != null && hit.collider.gameObject != lastScannedCrop)
            {
                lastScannedCrop = hit.collider.gameObject; // Don't count the same plant twice in one pass

                if (crop.cropStatus.Contains("CRITICAL"))
                    sickPlantsFound++;
                else
                    healthyPlantsFound++;
            }
        }
        else
        {
            lastScannedCrop = null;
        }
    }
}