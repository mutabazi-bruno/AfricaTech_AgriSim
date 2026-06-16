using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Text Fields")]
    public TextMeshProUGUI farmerUiText;
    public TextMeshProUGUI droneUiText;

    void Awake()
    {
        // Singleton pattern for easy access
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        farmerUiText.text = "Farmer Scanner: Ready";
        droneUiText.text = "Drone Feed: Patrolling...";
    }

    // Called by the Player Controller
    public void UpdateFarmerText(string status)
    {
        farmerUiText.text = "Local Scan: " + status;
    }

    // Called by the drone controller
    public void UpdateDroneFeed(string status)
    {
        droneUiText.text = "LIVE DRONE TELEMETRY: " + status;
    }
}