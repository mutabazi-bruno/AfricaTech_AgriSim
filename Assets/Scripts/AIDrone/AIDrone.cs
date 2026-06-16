using UnityEngine;
using System.Collections.Generic; // Need this for lists

public class AIDrone : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 4f;
    public float scanDownDistance = 3.5f;
    
    private Vector3 startPosition;
    private bool movingRight = true;

    // Analytics counters
    private int healthyPlantsFound = 0;
    private int sickPlantsFound = 0;

    // Tracks which plants we've already scanned this lap
    private List<int> scannedPlantIDs = new List<int>();

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float rightBoundary = startPosition.x + patrolDistance;
        float leftBoundary = startPosition.x - patrolDistance;
        bool justTurned = false;

        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
                justTurned = true;
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
                justTurned = true;
            }
        }

        if (justTurned && (healthyPlantsFound > 0 || sickPlantsFound > 0))
        {
            PlayerController.instance.DisplayDroneReport(healthyPlantsFound, sickPlantsFound);
            
            // Reset counts for next patrol
            healthyPlantsFound = 0;
            sickPlantsFound = 0;
            scannedPlantIDs.Clear(); // Clear scanned list for next pass
        }

        // Scan downward for crops
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, scanDownDistance);
        Debug.DrawRay(transform.position, Vector2.down * scanDownDistance, Color.yellow);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                Crop crop = hit.collider.GetComponent<Crop>();
                if (crop != null)
                {
                    int plantID = hit.collider.gameObject.GetHashCode();

                    // Only count each plant once per pass
                    if (!scannedPlantIDs.Contains(plantID))
                    {
                        scannedPlantIDs.Add(plantID); // Record this plant as scanned

                        if (crop.cropStatus.Contains("Fungal") || crop.cropStatus.Contains("Infection"))
                        {
                            sickPlantsFound++;
                        }
                        else
                        {
                            healthyPlantsFound++;
                        }
                    }
                }
            }
        }
    }
}