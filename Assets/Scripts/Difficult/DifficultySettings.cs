using UnityEngine;

[CreateAssetMenu()]
public class DifficultySettings : ScriptableObject
{
    public string difficultyName;
    public float enemyDamageMultiplier = 1f;
    public float enemyHealthMultiplier = 1f;
    public float expMultiplier = 1f;
}