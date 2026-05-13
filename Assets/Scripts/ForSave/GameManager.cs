using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DifficultySettings CurrentDifficulty { get; private set; }

    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private DifficultySettings defaultDifficulty;

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

        data.deadEnemies = DestructibleWallManager.Instance.GetDeadEnemies();
        data.destroyedWalls = DestructibleWallManager.Instance.GetDestroyedWalls();

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

        Player.Instance.transform.position = new Vector3(
            pendingLoadData.playerX, pendingLoadData.playerY, 0f);

        foreach (var enemy in FindObjectsByType<EnemyEntity>())
        {
            if (pendingLoadData.deadEnemies.Contains(enemy.gameObject.name))
            {
                Destroy(enemy.gameObject);
            }
        }

        DestructibleWallManager.Instance.RestoreDestroyedWalls(pendingLoadData.destroyedWalls);

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

}