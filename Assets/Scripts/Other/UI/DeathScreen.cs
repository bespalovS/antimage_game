using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen Instance { get; private set; }

    [SerializeField] private GameObject deathScreenPanel;
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        Instance = this;
        canvasGroup = deathScreenPanel.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        Invoke(nameof(Show), 1.5f);
    }

    private void Show()
    {
        deathScreenPanel.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        Time.timeScale = 0f;
    }

    public void OnRestartButton()
    {
        if (SaveSystem.HasSave())
            GameManager.Instance.LoadGame();
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnMainMenuButton()
    {
        SceneTransition.Instance.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        if (Player.Instance != null)
            Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }
}