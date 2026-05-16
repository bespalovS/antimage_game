using System;
using System.Collections;
using UnityEngine;

public class EnemyEntity : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private GameObject healthBarPrefab;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip[] deathSounds;

    [Header("Sound Volumes")]
    [SerializeField][Range(0f, 1f)] private float hitVolume = 1f;
    [SerializeField][Range(0f, 1f)] private float deathVolume = 1f;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    private int currentHealth;

    private BoxCollider2D boxCollider2D;
    private EnemyAI enemyAI;
    private EnemyHealthBar healthBar;

    private KnockBack knockBack;

    private bool isDead = false;

    public bool IsDead() => isDead;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemyAI = GetComponent<EnemyAI>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        currentHealth = stats.GetScaledHealth();

        if (healthBarPrefab != null)
        {
            GameObject bar = Instantiate(healthBarPrefab);
            healthBar = bar.GetComponent<EnemyHealthBar>();
            healthBar.Setup(transform, currentHealth);
        }

    }

    public void TakeDamage(int damage, Vector2 hitDir)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthBar?.UpdateBar(currentHealth);
        OnTakeHit?.Invoke(this, EventArgs.Empty);

        knockBack?.GetKnockBackFromDirection(hitDir);

        AudioManager.Instance.PlayRandomSound(hitSounds, hitVolume);

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            isDead = true;

            SkillSystem.Instance.AddExp(stats.GetScaledExp());

            if (GameManager.Instance != null)
                GameManager.Instance.RegisterDeadEnemy(gameObject.name);

            foreach (var col in GetComponents<Collider2D>())
                col.enabled = false;

            if (healthBar != null)
                Destroy(healthBar.gameObject);

            enemyAI.setDeathState();
            enemyAI.DisableHitbox();
            OnDeath?.Invoke(this, EventArgs.Empty);

            if (deathSounds != null)
                AudioManager.Instance.PlayRandomSound(deathSounds, deathVolume);

            TryDropPotion();
            StartCoroutine(FadeAndDestroy());
        }
    }

    public int GetContactDamage()
    {
        return stats.GetScaledDamage();
    }

    private IEnumerator FadeAndDestroy()
    {
        float delay = 8f;
        float fadeDuration = 3f;

        yield return new WaitForSeconds(delay);

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Color color = sr.color;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            sr.color = color;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void TryDropPotion()
    {
        if (stats.potionPrefab == null) return;

        if (UnityEngine.Random.value <= stats.potionDropChance)
        {
            GameObject potion = Instantiate(stats.potionPrefab, transform.position, Quaternion.identity);

            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            float force = 3f;

            if (potion.TryGetComponent(out Rigidbody2D rb))
            {
                rb.AddForce(randomDir * force, ForceMode2D.Impulse);
            }
        }
    }

}
