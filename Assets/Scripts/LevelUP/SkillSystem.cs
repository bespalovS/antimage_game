using UnityEngine;
using System.Collections.Generic;

public class SkillSystem : MonoBehaviour
{
    public static SkillSystem Instance { get; private set; }

    [SerializeField] private SkillData[] skills;
    [SerializeField] private int expToNextLevel = 100;
    [SerializeField] private float expScalePerLevel = 1.5f;

    private Dictionary<SkillData, int> skillLevels = new Dictionary<SkillData, int>();
    private int skillPoints = 0;
    private int currentExp = 0;
    private int currentExpRequired;

    public int SkillPoints => skillPoints;
    public int CurrentExp => currentExp;
    public int CurrentExpRequired => currentExpRequired;
    public int CurrentLevel { get; private set; } = 1;

    private void Awake()
    {
        Instance = this;
        currentExpRequired = expToNextLevel;

        foreach (var skill in skills)
            skillLevels[skill] = 0;
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        while (currentExp >= currentExpRequired)
        {
            currentExp -= currentExpRequired;
            skillPoints++;
            CurrentLevel++;
            currentExpRequired = Mathf.RoundToInt(currentExpRequired * expScalePerLevel);
        }
    }

    public bool UpgradeSkill(SkillData skill)
    {
        if (skillPoints <= 0) return false;
        if (skillLevels[skill] >= skill.maxLevel) return false;

        skillLevels[skill]++;
        skillPoints--;
        ApplySkill(skill);
        return true;
    }

    public int GetSkillLevel(SkillData skill) => skillLevels[skill];

    public bool IsMaxLevel(SkillData skill) => skillLevels[skill] >= skill.maxLevel;

    private void ApplySkill(SkillData skill)
    {
        int level = skillLevels[skill];
        float value = skill.valuePerLevel * level;

        switch (skill.skillName)
        {
            case "Здоровье":
                Player.Instance.UpgradeMaxHealth(Mathf.RoundToInt(value));
                break;
            case "Урон":
                Player.Instance.UpgradeDamage(Mathf.RoundToInt(value));
                break;
            case "Урон удара по земле":
                PlayerCombat.Instance.UpgradeSlamDamage(Mathf.RoundToInt(value));
                break;
            case "Скорость":
                Player.Instance.UpgradeSpeed(value);
                break;
            case "Dash":
                Player.Instance.UpgradeDashDistance(value);
                break;
            case "Эффективность блока":
                Player.Instance.UpgradeBlockEfficiency(value);
                break;
            case "Эффективность зелий":
                Player.Instance.UpgradePotionEfficiency(Mathf.RoundToInt(value));
                break;
            case "Перезарядка зелий":
                Player.Instance.UpgradePotionCooldown(value);
                break;
            case "Перезарядка удара по земле":
                PlayerCombat.Instance.UpgradeSlamCooldown(value);
                break;
        }
    }

    public SkillData[] GetAllSkills() => skills;

    public void SaveData(SaveData data)
    {
        data.currentExp = currentExp;
        data.currentLevel = CurrentLevel;
        data.currentExpRequired = currentExpRequired;
        data.skillPoints = skillPoints;

        data.skillNames.Clear();
        data.skillLevels.Clear();

        foreach (var pair in skillLevels)
        {
            data.skillNames.Add(pair.Key.skillName);
            data.skillLevels.Add(pair.Value);
        }
    }

    public void LoadData(SaveData data)
    {
        currentExp = data.currentExp;
        CurrentLevel = data.currentLevel;
        currentExpRequired = data.currentExpRequired;
        skillPoints = data.skillPoints;

        for (int i = 0; i < data.skillNames.Count; i++)
        {
            foreach (var skill in skills)
            {
                if (skill.skillName == data.skillNames[i])
                {
                    skillLevels[skill] = data.skillLevels[i];

                    for (int j = 0; j < data.skillLevels[i]; j++)
                        ApplySkill(skill);

                    break;
                }
            }
        }
    }

}