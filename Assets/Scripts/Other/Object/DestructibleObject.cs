using System;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 1;

    public event EventHandler OnDestructibleObjectTakeDamage;
    private Vector3 originalPosition;

    public void TakeDamage(int damage, Vector2 hitDir)
    {
        health -= damage;

        StartCoroutine(Shake(hitDir));

        if (health <= 0)
        {
            OnDestructibleObjectTakeDamage?.Invoke(this, EventArgs.Empty);

            NavMeshSurfaceManagement.Instance?.RequestRebake();

            Destroy(gameObject);
        }
    }

    System.Collections.IEnumerator Shake(Vector2 hitDir)
    {
        float duration = 0.1f;
        float magnitude = 0.15f;

        originalPosition = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 offset = hitDir.normalized * UnityEngine.Random.Range(0.5f, 1f) * magnitude;

            transform.localPosition = originalPosition + (Vector3)offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

}
