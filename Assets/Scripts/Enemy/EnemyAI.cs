using UnityEngine;
using UnityEngine.AI;
using AntiMage.Utils;
using System;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanseMax = 7f;
    [SerializeField] private float roamingDistanseMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    [SerializeField] private bool isChasingEnemy = false;
    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpeedMultiplier = 2f;

    [SerializeField] private bool isAttackingEnemy = false;
    [SerializeField] private float attackingDistance = 1.5f;
    [SerializeField] private float attackRate = 1f;
    private float nextAttackTime = 0f;

    [SerializeField] private HitboxControllerEnemy hitboxController;
    private Vector2 attackDir;

    private NavMeshAgent navMeshAgent;
    private State currentState;
    private float roamingTimer;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private float roamingSpeed;
    private float chasingSpeed;

    private KnockBack knockBack;

    public event EventHandler OnEnemyAttack;

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attack,
        Death
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        knockBack = GetComponent<KnockBack>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        currentState = startingState;

        roamingSpeed = navMeshAgent.speed;
        chasingSpeed = navMeshAgent.speed * chasingSpeedMultiplier;
    }

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (knockBack.isGettingKnockBack)
            return;

        StateHandler();
    }

    public void setDeathState()
    {
        navMeshAgent.ResetPath();
        currentState = State.Death;

    }

    private void StateHandler ()
    {
        switch (currentState)
        {
            default:
            case State.Idle:
                break;

            case State.Roaming:
                roamingTimer -= Time.deltaTime;
                if (roamingTimer < 0)
                {
                    Roaming();
                    roamingTimer = roamingTimerMax;
                }
                CheckCurrentState();
                break;

            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;

            case State.Attack:
                AttackingTarget();
                CheckCurrentState();
                break;

            case State.Death:
                break;
        }
    }

    private void ChasingTarget()
    {
        navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    public float GetRoamingAnimationSpeed()
    {
        return navMeshAgent.speed / roamingSpeed;
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (isChasingEnemy)
        {
            if (Player.Instance.IsAlive() && distanceToPlayer <= chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (isAttackingEnemy)
        {
            if (distanceToPlayer < attackingDistance)
            {
                if (Player.Instance.IsAlive())
                    newState = State.Attack; 
                else
                    newState = State.Roaming;
            }
        }

        if (newState != currentState)
        {
            if (newState == State.Chasing)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = chasingSpeed;
            }
            else if ( newState == State.Roaming)
            {
                roamingTimer = 0f;
                navMeshAgent.speed = roamingSpeed;
            }
            else if ( newState == State.Attack)
            {
                navMeshAgent.ResetPath();
            }

            currentState = newState;
        }
    }

    private void AttackingTarget()
    {
        if (Time.time >= nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);

            nextAttackTime = Time.time + attackRate;
        }
    }

    public bool IsMoving()
    {
        if (navMeshAgent.velocity == Vector3.zero) {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Roaming()
    {
        roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanseMin, roamingDistanseMax);
    }

    public Vector2 GetMovementVector()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector2 moveDir = new Vector2(velocity.x, velocity.y);

        return moveDir;
    }

    public Vector2 GetDirectionToPlayer()
    {
        Vector2 dir = (Player.Instance.transform.position - transform.position).normalized;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            dir = new Vector2(Mathf.Sign(dir.x), 0);
        else
            dir = new Vector2(0, Mathf.Sign(dir.y));

        return dir;
    }

    public void EnableHitbox()
    {
        attackDir = GetDirectionToPlayer();
        hitboxController.EnableHitbox(attackDir);
    }

    public void DisableHitbox()
    {
        hitboxController.DisableAll();
    }

}
