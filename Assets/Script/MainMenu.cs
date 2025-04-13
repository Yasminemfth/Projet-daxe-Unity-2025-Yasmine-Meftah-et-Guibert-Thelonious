using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scènes")]
    public string levelToLoad = "scene principal"; 

    [Header("UI")]
    public GameObject settingsWindow;

    // lance le jeu
    public void PlayGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    // Quitte le jeu
    public void QuitGame()
    {
        Application.Quit();
    }
}
