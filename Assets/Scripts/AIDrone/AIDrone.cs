using UnityEngine;

public class AIDrone : MonoBehaviour
{
    public float speed = 2f;
    public float patrolDistance = 4f;
    
    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the left and right boundaries of its patrol route
        float rightBoundary = startPosition.x + patrolDistance;
        float leftBoundary = startPosition.x - patrolDistance;

        // Move the drone left or right automatically
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false; // Turn around
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true; // Turn around
            }
        }
    }
}