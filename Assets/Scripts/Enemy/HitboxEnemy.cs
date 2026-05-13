using UnityEngine;

public class HitboxEnemy : MonoBehaviour
{
    private EnemyEntity enemyEntity;

    private void Awake()
    {
        enemyEntity = GetComponentInParent<EnemyEntity>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, enemyEntity.GetContactDamage());
        }
    }
}
