using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //gere le statut pause du jeu
    public static bool GameIsPaused
    {
        get => _gameIsPaused;
        set
        {
            _gameIsPaused = value;
            Time.timeScale = value ? 0f : 1f;
        }
    }
    private static bool _gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsWindow;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;

        if (PlayerMovement.instance != null)
            PlayerMovement.instance.enabled = false;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;

        if (PlayerMovement.instance != null)
            PlayerMovement.instance.enabled = true;
    }

    public void LoadMainMenu()
    {
        GameIsPaused = false;
        SceneManager.LoadScene("main menu");
    }

    // pour les boutons du menu apause
    public void OpenPauseMenu()
    {
        Pause();
    }
}
