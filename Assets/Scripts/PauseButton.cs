using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public PauseMenu pauseMenu; // Drag your PauseManager (PauseMenu script) here in Inspector

    // This function is called when button is clicked
    public void OnPauseClick()
    {
        if (pauseMenu != null)
        {
            pauseMenu.TogglePause(); // Calls Pause() from PauseMenu
            Debug.Log("Pause button clicked!");
        }
        else
        {
            Debug.LogError("PauseMenu is not assigned in PauseButton!");
        }
    }
}