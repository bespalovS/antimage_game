using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Image healthFillImage;

    [Header("Potions")]
    [SerializeField] private TextMeshProUGUI potionCountText;
    [SerializeField] private Image potionCooldownImage;

    [Header("Ground Slam")]
    [SerializeField] private Image slamCooldownImage;

    [Header("Dash")]
    [SerializeField] private Image dashCooldownImage;

    [SerializeField] private TextMeshProUGUI levelText;
    private void Update()
    {
        UpdateHealth();
        UpdatePotions();
        UpdateSlamCooldown();
        UpdateDashCooldown();

        levelText.text = "Уровень " + SkillSystem.Instance.CurrentLevel;
    }

    private void UpdateHealth()
    {
        float t = (float)Player.Instance.GetCurrentHealth() / Player.Instance.GetMaxHealth();
        healthFillImage.fillAmount = t;
    }

    private void UpdatePotions()
    {
        potionCountText.text = Player.Instance.GetCurrentPotions().ToString();

        float timer = Player.Instance.GetPotionCooldownTimer();
        float cooldown = Player.Instance.GetPotionCooldown();

        potionCooldownImage.fillAmount = timer / cooldown;
    }

    private void UpdateSlamCooldown()
    {
        float timer = PlayerCombat.Instance.GetSlamCooldownTimer();
        float cooldown = PlayerCombat.Instance.GetSlamCooldown();

        slamCooldownImage.fillAmount = timer / cooldown;
    }

    private void UpdateDashCooldown()
    {
        float timer = Player.Instance.GetDashCooldownTimer();
        float cooldown = Player.Instance.GetDashCooldownTime();

        dashCooldownImage.fillAmount = timer / cooldown;
    }
}
