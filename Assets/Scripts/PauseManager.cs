using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("Panels")]
    public GameObject pauseMenuUI;      // Main pause panel
    public GameObject optionsMenuUI;    // Options panel

    [Header("Audio")]
    public AudioSource audioSource;     // 🎵 drag your AudioSource here
    public Slider volumeSlider;         // 🎚 drag your UI slider here (0-100)

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(false);

        if (volumeSlider != null && audioSource != null)
        {
            // Load saved volume or set default
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 75f); // 0-100
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (GameIsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // 🎵 Called by slider OnValueChanged
    public void SetVolume(float sliderValue)
    {
        if (audioSource != null)
        {
            // Convert slider (0-100) to logarithmic volume (0.0-1.0)
            float volume = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 100f) / 100f) * 20f;
            audioSource.volume = Mathf.Pow(10f, volume / 20f);
        }

        // Save slider value (0-100)
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
}