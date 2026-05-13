using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public float playerX;
    public float playerY;
    public int currentHealth;
    public int currentPotions;
    public List<string> deadEnemies = new List<string>();
    public List<string> destroyedWalls = new List<string>();
    public int currentExp;
    public int currentExpRequired;
    public int skillPoints;
    public List<string> skillNames = new List<string>();
    public List<int> skillLevels = new List<int>();
    public int currentLevel;
}
