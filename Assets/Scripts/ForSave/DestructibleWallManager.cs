using System.Collections.Generic;
using UnityEngine;

public class DestructibleWallManager : MonoBehaviour
{
    public static DestructibleWallManager Instance { get; private set; }

    private List<string> destroyedWalls = new List<string>();
    private List<string> deadEnemies = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterDestroyedWall(string wallName)
    {
        if (!destroyedWalls.Contains(wallName))
            destroyedWalls.Add(wallName);
    }

    public void RegisterDeadEnemy(string enemyName)
    {
        if (!deadEnemies.Contains(enemyName))
            deadEnemies.Add(enemyName);
    }

    public List<string> GetDeadEnemies() => deadEnemies;

    public List<string> GetDestroyedWalls() => destroyedWalls;

    public void RestoreDestroyedWalls(List<string> walls)
    {
        foreach (var wallName in walls)
        {
            foreach (var wall in FindObjectsByType<DestructibleWall>())
            {
                if (wall.gameObject.name == wallName)
                    Destroy(wall.gameObject);
            }
        }
    }
}