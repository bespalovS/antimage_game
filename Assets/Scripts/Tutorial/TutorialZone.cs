using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    [SerializeField][TextArea(3, 10)] private string tutorialText;
    [SerializeField] private Sprite keySprite;

    private bool isPlayerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerInside) return;

        if (other.TryGetComponent(out Player player))
        {
            isPlayerInside = true;
            TutorialUI.Instance.Show(tutorialText, keySprite);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isPlayerInside) return;

        if (other.TryGetComponent(out Player player))
        {
            isPlayerInside = false;
            TutorialUI.Instance.Hide();
            Destroy(gameObject);
        }
    }
}