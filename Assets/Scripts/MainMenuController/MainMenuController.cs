using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;

    [Header("Settings Elements")]
    public Slider volumeSlider;
    public Toggle muteToggle;

    void Start()
    {
        // Set the slider and toggle to match the current system audio levels on start
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (muteToggle != null)
        {
            muteToggle.isOn = AudioListener.pause;
            muteToggle.onValueChanged.AddListener(SetMute);
        }
    }

    public void StartSimulation()
    {
        SceneManager.LoadScene(1); 
    }

    public void QuitSimulation()
    {
        Application.Quit();
        Debug.Log("Game Quit Checked!");
    }

    // Settings panel methods
    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void SetVolume(float value)
    {
        // Set global volume
        AudioListener.volume = value; 
    }

    public void SetMute(bool isMuted)
    {
        // Toggle game audio
        AudioListener.pause = isMuted; 
    }
}