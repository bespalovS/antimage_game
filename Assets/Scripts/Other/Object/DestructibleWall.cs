using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    [SerializeField] private EnemyEntity targetEnemy;

    private void Start()
    {
        if (targetEnemy == null)
        {
            Debug.LogError("Target Enemy не назначен на " + gameObject.name);
            return;
        }

        targetEnemy.OnDeath += TargetEnemy_OnDeath;
    }

    private void TargetEnemy_OnDeath(object sender, System.EventArgs e)
    {
        DestructibleWallManager.Instance.RegisterDestroyedWall(gameObject.name);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (targetEnemy != null)
            targetEnemy.OnDeath -= TargetEnemy_OnDeath;
    }
}
