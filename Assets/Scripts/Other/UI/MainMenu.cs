using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button loadButton;
    [SerializeField] private GameObject difficultyPanel;

    public void OnLoadButton()
    {
        if (GameManager.Instance.HasSave())
            GameManager.Instance.LoadGame();
    }

    public void startGame()
    {
        difficultyPanel.SetActive(true);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        loadButton.interactable = GameManager.Instance.HasSave();
    }
}
