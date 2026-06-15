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
        // This sets up a clean "Singleton" pattern so any script can easily send data here
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

    // Called by the Autonomous Drone script wirelessly!
    public void UpdateDroneFeed(string status)
    {
        droneUiText.text = "LIVE DRONE TELEMETRY: " + status;
    }
}