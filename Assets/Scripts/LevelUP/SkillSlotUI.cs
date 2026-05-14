using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image dimOverlay;
    [SerializeField] private TextMeshProUGUI levelText;

    private SkillData skillData;

    public void Setup(SkillData data)
    {
        skillData = data;
        icon.sprite = data.icon;
        UpdateVisual();
    }

    public void OnClick()
    {
        if (SkillSystem.Instance.UpgradeSkill(skillData))
        {
            UpdateVisual();
            SkillPanelUI.Instance.UpdateSkillPoints();
            AudioManager.Instance.PlaySkillUpgrade();
        }
    }

    private void UpdateVisual()
    {
        int level = SkillSystem.Instance.GetSkillLevel(skillData);
        levelText.text = level + "/" + skillData.maxLevel;

        dimOverlay.gameObject.SetActive(level == 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillTooltip.Instance.Show(skillData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillTooltip.Instance.Hide();
    }
}