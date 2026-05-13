using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FlashBlink))]
public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private KnockBack knockBack;

    public static PlayerVisual Instance;

    private Animator animator;

    private FlashBlink flashBlink;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";

    private Vector2 currentAttackDir;

    private bool damageDealtThisAttack = false;

    public Animator Animator => animator;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        flashBlink = GetComponent<FlashBlink>();
    }

    private void Start()
    {
        knockBack.OnKnockBack += OnKnockBack;
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Update()
    {
        Vector2 moveDir = Player.Instance.GetMovementVector();

        animator.SetFloat("MoveX", moveDir.x);
        animator.SetFloat("MoveY", moveDir.y);
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
    }

    public void PlayAttack(int comboStep, Vector2 dir)
    {
        if (comboStep <= 0)
            return;

        currentAttackDir = dir;
        animator.SetFloat("AttackX", dir.x);
        animator.SetFloat("AttackY", dir.y);

        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.SetTrigger("Attack" + comboStep);
    }

    private void OnKnockBack(Vector2 dir)
    {
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);

        animator.SetTrigger("TakeHit");
    }

    public void Dash(Vector2 dir)
    {
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);

        animator.SetTrigger("Dash");
    }

    public void SetBlock(bool value, Vector2 dir)
    {
        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);

        animator.SetBool("IsBlocking", value);
    }

    public void OpenComboWindow()
    {
        PlayerCombat.Instance.OpenComboWindow();
    }

    public void CloseComboWindow()
    {
        PlayerCombat.Instance.CloseComboWindow();
    }

    public void StartAttack()
    {
        damageDealtThisAttack = false;

        Vector2 finalDir = GetCardinalDir(currentAttackDir);
        Player.Instance.SetLookDirection(finalDir);
        Player.Instance.AttackDash(currentAttackDir);
    }

    public void EndAttack()
    {
        PlayerCombat.Instance.EndAttack();
    }

    public void OpenHitbox()
    {
        HitboxController.Instance.OpenHitbox(currentAttackDir);
    }

    public void CloseHitbox()
    {
        HitboxController.Instance.CloseHitbox();
    }

    public void OnHitAnimationEnd()
    {
        PlayerCombat.Instance.InterruptAttack();
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        flashBlink.StopBlinking();
    }

    private Vector2 GetCardinalDir(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return new Vector2(Mathf.Sign(dir.x), 0);
        else
            return new Vector2(0, Mathf.Sign(dir.y));
    }
    public void DealDamage()
    {
        if (damageDealtThisAttack) return;
        damageDealtThisAttack = true;

        HitboxController.Instance.DealDamage(currentAttackDir);
    }

    private void OnDisable()
    {
        knockBack.OnKnockBack -= OnKnockBack;
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }

    public void PlayGroundSlam()
    {
        animator.SetTrigger("GroundSlam");
    }

    public void DealSlamDamage()
    {
        PlayerCombat.Instance.DealSlamDamage();
    }

    public void EndGroundSlam()
    {
        PlayerCombat.Instance.EndGroundSlam();
    }

}
