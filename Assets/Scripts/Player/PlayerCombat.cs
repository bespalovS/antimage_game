using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private int slamDamage = 20;
    [SerializeField] private float slamCooldown = 5f;
    [SerializeField] private float slamRadius = 2.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float slamKnockBackForce = 20f;

    public static PlayerCombat Instance;

    private int comboStep;
    private bool canCombo;
    private Vector2 attackDir;
    private bool isAttacking;
    private bool canStartNewAttack = true;

    private Animator animator;

    private float slamCooldownTimer = 0f;
    private bool isSlaming = false;

    public float GetSlamCooldownTimer() => slamCooldownTimer;
    public float GetSlamCooldown() => slamCooldown;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        animator = PlayerVisual.Instance.Animator;
        GameInput.Instance.OnGroundSlam += GameInput_OnGroundSlam;

        isSlaming = false;
        isAttacking = false;
        canStartNewAttack = true;
    }

    private void Update()
    {
        if (canCombo && PlayerInputBuffer.Instance.ConsumeAttackBuffer())
        {
            ContinueCombo();
            canCombo = false;
        }

        if (slamCooldownTimer > 0)
            slamCooldownTimer -= Time.deltaTime;
    }

    public void AttackInput(Vector2 dir)
    {
        if (Player.Instance.IsBlocking())
            return;

        attackDir = dir;

        if (isAttacking)
        {
            PlayerInputBuffer.Instance.BufferAttack();
            return;
        }

        if (!canStartNewAttack)
            return;

        StartAttack();
    }

    private void StartAttack()
    {
        isAttacking = true;
        comboStep = 1;
        canStartNewAttack = false;

        PlayerVisual.Instance.PlayAttack(comboStep, attackDir);
        CameraShake.Instance.Shake(0.25f, 0.1f);
    }

    public void ContinueCombo()
    {
        if (!isAttacking) 
            return;

        comboStep++;
        comboStep = Mathf.Clamp(comboStep, 1, 2);

        PlayerVisual.Instance.PlayAttack(comboStep, attackDir);

        canCombo = false;
    }

    public void OpenComboWindow()
    {
        canCombo = true;
    }

    public void CloseComboWindow()
    {
        canCombo = false;
    }

    public void EndAttack()
    {
        if (canCombo)
            return;

        isAttacking = false;
        comboStep = 0;
        canStartNewAttack = true;
    }

    public float GetMovementModifier()
    {

        if (Player.Instance.IsAttackDashing)
            return 1f;

        if (isSlaming)
            return 0f;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsTag("Attack"))
            return 0.05f;

        return 1f;
    }

    public void InterruptAttack()
    {
        isAttacking = false;
        comboStep = 0;
        canCombo = false;
        canStartNewAttack = true;

        HitboxController.Instance.CloseHitbox();
        PlayerInputBuffer.Instance.Clear();
    }

    private void GameInput_OnGroundSlam(object sender, System.EventArgs e)
    {
        if (Player.Instance.IsBlocking()) return;
        if (slamCooldownTimer > 0) return;
        if (isSlaming) return;
        if (!Player.Instance.IsAlive()) return;

        InterruptAttack();
        StartCoroutine(SlamRoutine());
    }

    private IEnumerator SlamRoutine()
    {
        isSlaming = true;
        slamCooldownTimer = slamCooldown;

        PlayerVisual.Instance.PlayGroundSlam();

        yield return new WaitUntil(() => !isSlaming);
    }

    public void DealSlamDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, slamRadius, enemyLayer
        );

        List<IDamageable> alreadyHit = new List<IDamageable>();

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                if (alreadyHit.Contains(damageable)) continue;
                alreadyHit.Add(damageable);

                Vector2 dir = (hit.transform.position - transform.position).normalized;
                damageable.TakeDamage(slamDamage, dir);
                hit.GetComponent<KnockBack>()?.GetKnockBackFromDirection(dir, slamKnockBackForce);
            }
        }

        CameraShake.Instance.Shake(2f, 0.3f);
    }

    public void EndGroundSlam()
    {
        isSlaming = false;
    }

    //прокачка
    public void UpgradeSlamDamage(int amount)
    {
        slamDamage += amount;
    }

    public void UpgradeSlamCooldown(float amount)
    {
        slamCooldown = Mathf.Max(1f, slamCooldown - amount);
    }
    
    private void OnDestroy()
    {
        GameInput.Instance.OnGroundSlam -= GameInput_OnGroundSlam;
    }

}
