using UnityEngine;
using TMPro;

public class NoteUI : MonoBehaviour
{
    public static NoteUI Instance { get; private set; }

    [SerializeField] private GameObject noteScreen;
    [SerializeField] private TextMeshProUGUI noteText;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenNote(string text)
    {
        noteText.text = text;
        noteScreen.SetActive(true);
        Time.timeScale = 0f;
        GameInput.Instance.DisableGameplay();
    }

    public void CloseNote()
    {
        noteScreen.SetActive(false);
        Time.timeScale = 1f;
        GameInput.Instance.EnableGameplay();
    }
}