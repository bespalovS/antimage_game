using UnityEngine;

public class SpawnOnEnter : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private EnemyEntity targetEnemy;

    private bool hasSpawned;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSpawned) return;

        if (other.TryGetComponent(out Player player))
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        GameObject obj = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);

        var destroyScript = obj.GetComponent<DestroyObjectOnEnemyDeath>();
        destroyScript.SetTarget(targetEnemy);

        hasSpawned = true;
    }

}
