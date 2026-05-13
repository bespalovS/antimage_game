using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[SelectionBase]

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private int maxhealth = 100;
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashCooldownTime = 1f;
    [SerializeField] private int maxPotions = 5;
    [SerializeField] private int potionHealAmount = 40;
    [SerializeField] private float potionCooldown = 60f;

    //геттеры для UI
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxhealth;
    public int GetCurrentPotions() => currentPotions;
    public int GetMaxPotions() => maxPotions;
    public float GetPotionCooldownTimer() => potionCooldownTimer;
    public float GetPotionCooldown() => potionCooldown;
    public bool IsDashing() => isDashing;
    public float GetDashCooldownTime() => dashCooldownTime;
    public float GetDashCooldownTimer() => dashCooldownTimer;

    //прокачка
    private int bonusDamage = 0;
    private float bonusSpeed = 0f;
    private float bonusDashDistance = 0f;
    private float blockEfficiency = 1.5f;
    private int bonusPotionHeal = 0;
    private float potionCooldownReduction = 0f;
    //

    private Rigidbody2D rb;
    private KnockBack knockBack;

    private bool isRunning = false;
    private bool isAlive;
    private bool isDashing;
    Vector2 inputVector;

    private Vector2 attackDir;
    public Vector2 LastMoveDir;

    private int currentHealth;
    private int currentPotions;
    private float potionCooldownTimer = 0f;

    private bool canTakeDamage;
    private bool isPlayerBlocked;

    private float dashSpeedMultiplier = 1f;
    private float dashCooldownTimer = 0f;

    private bool isAttackDashing;
    public bool IsAttackDashing => isAttackDashing;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        knockBack = rb.GetComponent<KnockBack>();

        currentHealth = maxhealth;
        canTakeDamage = true;
        isAlive = true;
        currentPotions = 1;
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += GameInput_OnPlayerDash;
        GameInput.Instance.OnBlockChanged += GameInput_OnBlockChanged;
        GameInput.Instance.OnPlayerHeal += GameInput_OnPlayerHeal;
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (potionCooldownTimer > 0)
            potionCooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (knockBack.isGettingKnockBack || isAttackDashing)
            return;

        HandleMovement();
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        Vector2 dir = GetMouseDirection();
        PlayerCombat.Instance.AttackInput(dir);
    }

    private void GameInput_OnBlockChanged(bool value)
    {
        isPlayerBlocked = value;
        isRunning = false;
        PlayerVisual.Instance.SetBlock(value, GetMouseDirection());

        if (value)
        {
            PlayerCombat.Instance.InterruptAttack();
        }
    }

    private void HandleMovement()
    {
        if (isPlayerBlocked)
            return;

        float combatSpeedMultiplier = PlayerCombat.Instance.GetMovementModifier();

        float finalSpeed = movingSpeed * dashSpeedMultiplier * combatSpeedMultiplier;

        rb.MovePosition(
            rb.position + inputVector * (finalSpeed * Time.fixedDeltaTime)
        );

        if (inputVector.magnitude > 0.1f)
        {
            isRunning = true;
            LastMoveDir = inputVector;
        }
        else
        {
            isRunning = false;
        }

    }

    public void AttackDash(Vector2 dir)
    {
        StartCoroutine(AttackDashRoutine(dir));
    }

    public IEnumerator AttackDashRoutine(Vector2 dir)
    {
        isAttackDashing = true;

        float time = 0f;
        float duration = 0.04f;
        float speed = 50f;

        while (time < duration)
        {
            time += Time.deltaTime;

            if (isDashing)
            {
                isAttackDashing = false;
                yield break;
            }

            rb.MovePosition(
                rb.position + dir.normalized * speed * Time.deltaTime
            );

            yield return null;
        }

        isAttackDashing = false;
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(mouse);

        return ((Vector2)world - (Vector2)transform.position).normalized;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public bool IsBlocking()
    {
        return isPlayerBlocked;
    }

    private void GameInput_OnPlayerDash(object sender, EventArgs e)
    {
        if (!isRunning)
            return;

        Dash();
    }

    private void Dash()
    {
        if (!isDashing)
        {
            PlayerCombat.Instance.InterruptAttack();
            StartCoroutine(DashRoutine());
            PlayerVisual.Instance.Dash(inputVector);
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        dashSpeedMultiplier = dashSpeed;
        canTakeDamage = false;
        dashCooldownTimer = dashTime + 0.5f + dashCooldownTime;

        yield return new WaitForSeconds(dashTime);
        dashSpeedMultiplier = 0.4f;

        yield return new WaitForSeconds(0.5f);
        dashSpeedMultiplier = 1f;
        canTakeDamage = true;

        yield return new WaitForSeconds(dashCooldownTime);
        isDashing = false;
    }

    public Vector2 GetMovementVector()
    {
        return LastMoveDir;
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (canTakeDamage && isAlive)
        {
            canTakeDamage = false;

            if (isPlayerBlocked)
            {
                currentHealth = Mathf.Max(0, currentHealth -= damage / Mathf.RoundToInt(blockEfficiency));
                knockBack.GetKnockBackBlock(damageSource);
            }
            else
            {
                currentHealth = Mathf.Max(0, currentHealth -= damage);
                OnFlashBlink?.Invoke(this, EventArgs.Empty);
                knockBack.GetKnockBack(damageSource);
            }

            PlayerCombat.Instance.InterruptAttack();

            StartCoroutine(DamageRecovery());

            CameraShake.Instance.Shake(1f, 0.2f);

            isPlayerBlocked = false;
            PlayerVisual.Instance.SetBlock(false, Vector2.zero);
        }

        DetectDeath();
    }

    private IEnumerator DamageRecovery ()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void GameInput_OnPlayerHeal(object sender, EventArgs e)
    {
        UsePotion();
    }

    private void UsePotion()
    {
        if (currentPotions <= 0) return;
        if (currentHealth >= maxhealth) return;
        if (!isAlive) return;
        if (potionCooldownTimer > 0) return;

        currentPotions--;
        potionCooldownTimer = potionCooldown;

        int healAmount = Mathf.Min(potionHealAmount, maxhealth - currentHealth);
        currentHealth = Mathf.Min(maxhealth, currentHealth + potionHealAmount);

        FloatingTextSpawner.Instance.SpawnHealText(healAmount, transform.position);
    }

    public bool PickUpPotion()
    {
        if (currentPotions >= maxPotions) return false;

        currentPotions++;
        return true;
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0 && isAlive) 
        {
            isAlive = false;
            knockBack.StopMoveBackMovement();
            GameInput.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
        
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void SetLookDirection(Vector2 dir)
    {
        LastMoveDir = dir;
    }

    public void LoadData(int health, int potions)
    {
        currentHealth = health;
        currentPotions = potions;
    }

    //прокачка
    public void UpgradeMaxHealth(int amount)
    {
        maxhealth += amount;
        currentHealth += amount;
    }

    public void UpgradeDamage(int amount)
    {
        bonusDamage += amount;
    }

    public void UpgradeSpeed(float amount)
    {
        bonusSpeed += amount;
        movingSpeed += amount;
    }

    public void UpgradeDashDistance(float amount)
    {
        bonusDashDistance += amount;
    }

    public void UpgradeBlockEfficiency(float amount)
    {
        blockEfficiency += amount;
    }

    public void UpgradePotionEfficiency(int amount)
    {
        bonusPotionHeal += amount;
        potionHealAmount += amount;
    }

    public void UpgradePotionCooldown(float amount)
    {
        potionCooldownReduction += amount;
        potionCooldown = Mathf.Max(5f, potionCooldown - amount);
    }

    public int GetDamage()
    {
        return damage + bonusDamage;
    }
    //прокачка конец

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash -= GameInput_OnPlayerDash;
        GameInput.Instance.OnBlockChanged -= GameInput_OnBlockChanged;
        GameInput.Instance.OnPlayerHeal -= GameInput_OnPlayerHeal;
    }
}