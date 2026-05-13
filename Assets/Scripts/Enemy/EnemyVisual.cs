using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAi;
    [SerializeField] private EnemyEntity _enemyEntity;
    private Animator animator;

    private const string IS_MOVING = "IsMoving";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string TAKE_HIT = "TakeHit";
    private const string IS_DIE = "IsDie";

    SpriteRenderer spriteRenderer;

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

    private void enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        spriteRenderer.sortingOrder = -1;
    }

    private void enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TAKE_HIT);
    }

    private void Update()
    {
        Vector2 moveDir = _enemyAi.GetMovementVector();

        if (moveDir != Vector2.zero)
        {
            animator.SetFloat("DirX", moveDir.x);
            animator.SetFloat("DirY", moveDir.y);
        }

        animator.SetBool(IS_MOVING, _enemyAi.IsMoving());
        animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAi.GetRoamingAnimationSpeed());
    }

    private void enemyAi_OnEnemyAttack(object sender, System.EventArgs e)
    {
        Vector2 attackDir = _enemyAi.GetDirectionToPlayer();
        animator.SetFloat("DirX", attackDir.x);
        animator.SetFloat("DirY", attackDir.y);
        animator.SetTrigger(ATTACK);
    }

    public void EnableHitbox()
    {
        _enemyAi.EnableHitbox();
    }

    public void DisableHitbox()
    {
        _enemyAi.DisableHitbox();
    }

    private void OnDestroy()
    {
        _enemyAi.OnEnemyAttack -= enemyAi_OnEnemyAttack;
        _enemyEntity.OnTakeHit -= enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath -= enemyEntity_OnDeath;
    }

}
