using UnityEngine;

public class WitchVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAi;
    [SerializeField] private EnemyEntity _enemyEntity;

    [SerializeField] private FireballEnemy fireballEnemy;
    [SerializeField] private Transform fireballSpawnPoint;
    [SerializeField] private float spawnPointOffsetX = 0.5f;

    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField][Range(0f, 1f)] private float attackVolume = 1f;
    [SerializeField] private AudioClip DeathSounds;
    [SerializeField][Range(0f, 1f)] private float DeathVolume = 1f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_MOVING = "IsMoving";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string TAKE_HIT = "TakeHit";
    private const string IS_DIE = "IsDie";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _enemyAi.OnEnemyAttack += enemyAi_OnEnemyAttack;
        _enemyEntity.OnTakeHit += enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath += enemyEntity_OnDeath;
    }

    private void Update()
    {
        Vector2 moveDir = _enemyAi.GetMovementVector();

        if (moveDir != Vector2.zero)
        {
            if (moveDir.x != 0)
            {
                spriteRenderer.flipX = moveDir.x < 0;

                Vector3 pos = fireballSpawnPoint.localPosition;
                pos.x = moveDir.x < 0 ? -spawnPointOffsetX : spawnPointOffsetX;
                fireballSpawnPoint.localPosition = pos;
            }
                
        }

        animator.SetBool(IS_MOVING, _enemyAi.IsMoving());
        animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAi.GetRoamingAnimationSpeed());
    }

    public void SpawnFireball()
    {
        fireballEnemy.ShootFireball();
    }

    private void enemyAi_OnEnemyAttack(object sender, System.EventArgs e)
    {
        Vector2 attackDir = _enemyAi.GetDirectionToPlayer();
        spriteRenderer.flipX = attackDir.x < 0;

        Vector3 pos = fireballSpawnPoint.localPosition;
        pos.x = attackDir.x < 0 ? -spawnPointOffsetX : spawnPointOffsetX;
        fireballSpawnPoint.localPosition = pos;

        animator.SetTrigger(ATTACK);
    }

    private void enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TAKE_HIT);
    }

    private void enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        spriteRenderer.sortingOrder = -1;
    }
    public void PlayAttackSound()
    {
        AudioManager.Instance.PlayRandomSound(attackSounds, attackVolume);
    }

    public void PlayDeathSound()
    {
        AudioManager.Instance.PlaySound(DeathSounds, DeathVolume);
    }

    private void OnDestroy()
    {
        _enemyAi.OnEnemyAttack -= enemyAi_OnEnemyAttack;
        _enemyEntity.OnTakeHit -= enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath -= enemyEntity_OnDeath;
    }

}
