using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject promptUI;
    [SerializeField] private GameObject flame;

    private bool isPlayerNear = false;
    private bool isActive = false;

    public static SavePoint ActiveSavePoint { get; private set; }

    private void Start()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
        promptUI.SetActive(false);
        flame.SetActive(false);
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        if (!isPlayerNear || isActive) return;

        if (ActiveSavePoint != null && ActiveSavePoint != this)
            ActiveSavePoint.Deactivate();

        Activate();
        promptUI.SetActive(false);

        GameManager.Instance.SaveGame();
    }

    public void Activate()
    {
        isActive = true;
        ActiveSavePoint = this;
        flame.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        flame.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            isPlayerNear = true;
            if (!isActive)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            isPlayerNear = false;
            promptUI.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnInteract -= GameInput_OnInteract;
    }
}