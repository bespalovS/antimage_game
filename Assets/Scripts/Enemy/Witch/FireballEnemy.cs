using UnityEngine;

public class FireballEnemy : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballSpawnPoint;

    public void ShootFireball()
    {
        Vector2 dir = (Player.Instance.transform.position - fireballSpawnPoint.position).normalized;

        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        fireball.GetComponent<Fireball>().SetDirection(dir);
    }

}
