using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DifficultySettings CurrentDifficulty { get; private set; }

    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private DifficultySettings defaultDifficulty;

    private List<string> destroyedWalls = new List<string>();
    private List<string> deadEnemies = new List<string>();

    //private HashSet<string> deadEnemies = new HashSet<string>();

    private SaveData pendingLoadData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        CurrentDifficulty = defaultDifficulty;

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.playerX = Player.Instance.transform.position.x;
        data.playerY = Player.Instance.transform.position.y;
        data.currentHealth = Player.Instance.GetCurrentHealth();
        data.currentPotions = Player.Instance.GetCurrentPotions();

        data.deadEnemies = GameManager.Instance.GetDeadEnemies();
        data.destroyedWalls = GameManager.Instance.GetDestroyedWalls();

        SkillSystem.Instance.SaveData(data);
        SaveSystem.Save(data);
        
        SaveNotification.Instance.Show();
        Debug.Log("Game saved!");
    }

    public void LoadGame()
    {
        pendingLoadData = SaveSystem.Load();
        if (pendingLoadData == null) return;

        Time.timeScale = 1f;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        destroyedWalls = new List<string>(pendingLoadData.destroyedWalls);
        deadEnemies = new List<string>(pendingLoadData.deadEnemies);

        Player.Instance.transform.position = new Vector3(
            pendingLoadData.playerX, pendingLoadData.playerY, 0f);

        foreach (var enemy in FindObjectsByType<EnemyEntity>())
        {
            if (deadEnemies.Contains(enemy.gameObject.name))
                Destroy(enemy.gameObject);
        }

        foreach (var wall in FindObjectsByType<DestructibleWall>())
        {
            if (destroyedWalls.Contains(wall.gameObject.name))
                Destroy(wall.gameObject);
        }

        Player.Instance.LoadData(pendingLoadData.currentHealth, pendingLoadData.currentPotions);
        SkillSystem.Instance.LoadData(pendingLoadData);

        pendingLoadData = null;
    }

    public bool HasSave() => SaveSystem.HasSave();
    public bool HasPendingData() => pendingLoadData != null;

    public void ApplyPendingPlayerData(Player player)
    {
        if (pendingLoadData == null) return;
        player.LoadData(pendingLoadData.currentHealth, pendingLoadData.currentPotions);
    }

    public void SetDifficulty(DifficultySettings difficulty)
    {
        CurrentDifficulty = difficulty;
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

    public List<string> GetDestroyedWalls() => destroyedWalls;
    public List<string> GetDeadEnemies() => deadEnemies;
}