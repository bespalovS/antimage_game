using UnityEngine;

[CreateAssetMenu()]
public class SkillData : ScriptableObject
{
    public string skillName;
    [TextArea] public string description;
    public Sprite icon;
    public int maxLevel = 5;
    public float valuePerLevel;
}