using UnityEngine;

public class PlayerInputBuffer : MonoBehaviour
{
    public static PlayerInputBuffer Instance;

    private bool attackBuffered;
    private float bufferTime = 0.2f;
    private float bufferTimer;

    private void Awake()
    {
        Instance = this;
    }

    public void BufferAttack()
    {
        attackBuffered = true;
        bufferTimer = bufferTime;
    }

    public bool ConsumeAttackBuffer()
    {
        if (attackBuffered && bufferTimer > 0)
        {
            attackBuffered = false;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (bufferTimer > 0)
            bufferTimer -= Time.deltaTime;

        if (bufferTimer <= 0)
            attackBuffered = false;
    }

    public void Clear()
    {
        attackBuffered = false;
    }
}
