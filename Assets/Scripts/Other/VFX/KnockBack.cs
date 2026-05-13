using System;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float knockBackForce = 4f;
    [SerializeField] private float knockBackMovingTimerMax = 0.3f;

    private float knockBackMovingTimer;

    private Rigidbody2D rb;

    public bool isGettingKnockBack { get; private set; }

    public event Action<Vector2> OnKnockBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        knockBackMovingTimer -= Time.deltaTime;
        if (knockBackMovingTimer < 0)
        {
            StopMoveBackMovement();
        }
    }

    public void GetKnockBack(Transform damageSource)
    {
        bool isBloking = Player.Instance.IsBlocking();

        if (!isBloking)
        {
            isGettingKnockBack = true;
            knockBackMovingTimer = knockBackMovingTimerMax;
            Vector2 difference = (transform.position - damageSource.position).normalized * knockBackForce / rb.mass;
            rb.AddForce(difference, ForceMode2D.Impulse);

            OnKnockBack?.Invoke(difference);
        }
    }

    public void GetKnockBackBlock(Transform damageSource)
    {
        bool isBloking = Player.Instance.IsBlocking();

        if (isBloking)
        {
            isGettingKnockBack = true;
            knockBackMovingTimer = knockBackMovingTimerMax;
            Vector2 difference = (transform.position - damageSource.position).normalized * (knockBackForce / rb.mass / 2);
            rb.AddForce(difference, ForceMode2D.Impulse);
        }
    }

    public void StopMoveBackMovement()
    {
        rb.linearVelocity = Vector2.zero;
        isGettingKnockBack = false;
    }

    public void GetKnockBackFromDirection(Vector2 dir)
    {
        isGettingKnockBack = true;
        knockBackMovingTimer = knockBackMovingTimerMax;

        rb.AddForce(dir.normalized * knockBackForce, ForceMode2D.Impulse);

        OnKnockBack?.Invoke(dir);
    }

    public void GetKnockBackFromDirection(Vector2 dir, float forceOverride)
    {
        isGettingKnockBack = true;
        knockBackMovingTimer = knockBackMovingTimerMax;

        rb.AddForce(dir.normalized * forceOverride, ForceMode2D.Impulse);

        OnKnockBack?.Invoke(dir);
    }

}
