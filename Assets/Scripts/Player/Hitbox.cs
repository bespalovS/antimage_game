using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private List<IDamageable> targets = new List<IDamageable>();

    private void OnEnable()
    {
        targets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable dmg))
        {
            targets.Add(dmg);
        }
    }

    public void DealDamage(Vector2 attackDir)
    {

        int damage = Player.Instance.GetDamage();

        if (targets.Count == 0) return;

        IDamageable bestTarget = null;
        float bestScore = -Mathf.Infinity;
        Vector2 dirToTarget = Vector2.zero;

        foreach (var target in targets)
        {
            dirToTarget = ((MonoBehaviour)target).transform.position - transform.position;
            dirToTarget.Normalize();

            float dot = Vector2.Dot(attackDir.normalized, dirToTarget);

            if (dot > bestScore)
            {
                bestScore = dot;
                bestTarget = target;
            }
        }

        bestTarget?.TakeDamage(damage, dirToTarget);

        CameraShake.Instance.Shake(1.2f, 0.1f);
    }

}
