using UnityEngine;

public class DestroyObjectOnEnemyDeath : MonoBehaviour
{
    private EnemyEntity targetEnemy;

    public void SetTarget(EnemyEntity enemy)
    {
        targetEnemy = enemy;

        if (targetEnemy != null)
            targetEnemy.OnDeath += HandleDeath;
    }

    private void HandleDeath(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (targetEnemy != null)
            targetEnemy.OnDeath -= HandleDeath;
    }

}
