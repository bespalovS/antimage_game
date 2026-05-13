using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillTooltip : MonoBehaviour
{
    public static SkillTooltip Instance { get; private set; }

    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform rectTransform;

    private void Awake()
    {
        Instance = this;
        tooltipPanel.SetActive(false);
    }

    public void Show(SkillData skill)
    {
        nameText.text = skill.skillName;
        descriptionText.text = skill.description;
        tooltipPanel.SetActive(true);
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            Mouse.current.position.ReadValue(),
            null,
            out mousePos
        );
        rectTransform.anchoredPosition = mousePos + new Vector2(15f, -15f);
    }
}