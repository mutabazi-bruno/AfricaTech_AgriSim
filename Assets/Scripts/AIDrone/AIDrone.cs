using UnityEngine;
using System.Collections.Generic; // <-- Required to use Lists!

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

    // 🚨 NEW: Keeps track of unique plant IDs so it counts every individual plant once per lap!
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
            
            // 🚨 RESET EVERYTHING FOR THE NEXT LAP:
            healthyPlantsFound = 0;
            sickPlantsFound = 0;
            scannedPlantIDs.Clear(); // Wipe the list clean so it can scan them again next pass!
        }

        // --- Downward Multi-Target Scan Analysis ---
        // We use RaycastAll to pierce through multiple plants sitting in the same column!
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

                    // If we haven't counted this specific plant on this flight pass yet
                    if (!scannedPlantIDs.Contains(plantID))
                    {
                        scannedPlantIDs.Add(plantID); // Mark it as counted!

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