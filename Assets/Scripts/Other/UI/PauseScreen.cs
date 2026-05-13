using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen Instance { get; private set; }

    [SerializeField] private GameObject pauseScreenPanel;

    private bool isPaused = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPause += GameInput_OnPause;
        pauseScreenPanel.SetActive(false);
    }

    private void GameInput_OnPause(object sender, System.EventArgs e)
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    private void Pause()
    {
        isPaused = true;
        pauseScreenPanel.SetActive(true);
        Time.timeScale = 0f;
        GameInput.Instance.DisableGameplay();
    }

    public void Resume()
    {
        isPaused = false;
        pauseScreenPanel.SetActive(false);
        Time.timeScale = 1f;
        GameInput.Instance.EnableGameplay();
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPause -= GameInput_OnPause;
    }
}