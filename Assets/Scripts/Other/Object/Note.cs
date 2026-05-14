using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField][TextArea(3, 10)] private string noteText;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private GameObject notePrompt;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color highlightColor = new Color(1f, 1f, 0.6f);

    private bool isPlayerNear = false;
    private Color defaultColor;

    private void Start()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
        defaultColor = spriteRenderer.color;
        notePrompt.SetActive(false);
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        if (isPlayerNear)
        {
            OpenNote();
            AudioManager.Instance.PlayNote();
        }
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.Instance.transform.position);
        bool playerNear = distance <= interactDistance;

        if (playerNear != isPlayerNear)
        {
            isPlayerNear = playerNear;
            notePrompt.SetActive(isPlayerNear);
            spriteRenderer.color = isPlayerNear ? highlightColor : defaultColor;
        }
    }

    private void OpenNote()
    {
        notePrompt.SetActive(false);
        spriteRenderer.color = defaultColor;
        NoteUI.Instance.OpenNote(noteText);
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnInteract -= GameInput_OnInteract;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }
}