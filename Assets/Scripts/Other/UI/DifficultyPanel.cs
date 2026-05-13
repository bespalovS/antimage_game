using UnityEngine;

public class DifficultyPanel : MonoBehaviour
{
    [SerializeField] private DifficultySettings[] difficulties;

    public void OnEasyButton()
    {
        StartGame(0);
    }

    public void OnNormalButton()
    {
        StartGame(1);
    }

    public void OnHardButton()
    {
        StartGame(2);
    }

    public void OnBackButton()
    {
        gameObject.SetActive(false);
    }

    private void StartGame(int index)
    {
        GameManager.Instance.SetDifficulty(difficulties[index]);
        SceneTransition.Instance.LoadScene("Game");
    }
}