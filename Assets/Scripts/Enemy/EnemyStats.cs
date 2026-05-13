using UnityEngine;

[CreateAssetMenu()]
public class EnemyStats : ScriptableObject
{
    [Header("Name")]
    public string EnemyName;

    [Header("Health")]
    public int maxEnemyHealth;

    [Header("Damage")]
    public int EnemyDamage;

    [Header("Experience")]
    public int expReward = 20;

    [Header("Drop")]
    [Range(0f, 1f)] public float potionDropChance = 0.3f;
    public GameObject potionPrefab;

    public int GetScaledDamage()
    {
        if (GameManager.Instance == null) return EnemyDamage;
        return Mathf.RoundToInt(EnemyDamage * GameManager.Instance.CurrentDifficulty.enemyDamageMultiplier);
    }

    public int GetScaledHealth()
    {
        if (GameManager.Instance == null) return maxEnemyHealth;
        return Mathf.RoundToInt(maxEnemyHealth * GameManager.Instance.CurrentDifficulty.enemyHealthMultiplier);
    }

    public int GetScaledExp()
    {
        if (GameManager.Instance == null) return expReward;
        return Mathf.RoundToInt(expReward * GameManager.Instance.CurrentDifficulty.expMultiplier);
    }

}
