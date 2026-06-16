using UnityEngine;

public class KeepAudioAlive : MonoBehaviour
{
    private static KeepAudioAlive instance;

    void Awake()
    {
        // If an instance already exists, destroy this duplicate one
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set this object as the permanent master music player
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}