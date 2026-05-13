using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPanelUI : MonoBehaviour
{
    public static SkillPanelUI Instance { get; private set; }

    [SerializeField] private GameObject skillPanel;
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform skillGrid;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private Image expFill;
    [SerializeField] private TextMeshProUGUI expText;

    private bool isOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        skillPanel.SetActive(false);
        GameInput.Instance.OnSkillPanel += GameInput_OnSkillPanel;
        SpawnSkillSlots();
    }

    private void GameInput_OnSkillPanel(object sender, System.EventArgs e)
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    private void SpawnSkillSlots()
    {
        foreach (var skill in SkillSystem.Instance.GetAllSkills())
        {
            GameObject slot = Instantiate(skillSlotPrefab, skillGrid);
            slot.GetComponent<SkillSlotUI>().Setup(skill);
        }
    }

    public void Open()
    {
        isOpen = true;
        skillPanel.SetActive(true);
        Time.timeScale = 0f;
        GameInput.Instance.DisableGameplay();
        UpdateSkillPoints();
        UpdateExpBar();
    }

    public void Close()
    {
        isOpen = false;
        skillPanel.SetActive(false);
        Time.timeScale = 1f;
        GameInput.Instance.EnableGameplay();
    }

    public void UpdateSkillPoints()
    {
        skillPointsText.text = "Очки навыков: " + SkillSystem.Instance.SkillPoints;
        UpdateExpBar();
    }

    private void UpdateExpBar()
    {
        float t = (float)SkillSystem.Instance.CurrentExp / SkillSystem.Instance.CurrentExpRequired;
        expFill.fillAmount = t;
        expText.text = SkillSystem.Instance.CurrentExp + "/" + SkillSystem.Instance.CurrentExpRequired;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnSkillPanel -= GameInput_OnSkillPanel;
    }
}