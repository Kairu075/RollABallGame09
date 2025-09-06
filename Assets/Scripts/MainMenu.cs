using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject categoryPanel;


    public void OpenCategories()
    {
        Debug.Log("Start Button Clicked!");
        mainMenuPanel.SetActive(false);
        categoryPanel.SetActive(true);
    }


    public void PlayGame(string sceneName)
    {
        Debug.Log("Loading Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void OpenOptions()
    {
        Debug.Log("Options menu opened!");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button Clicked!");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}