using UnityEngine;

public class HitboxControllerEnemy : MonoBehaviour
{
    [SerializeField] private Collider2D hitboxUp;
    [SerializeField] private Collider2D hitboxDown;
    [SerializeField] private Collider2D hitboxLeft;
    [SerializeField] private Collider2D hitboxRight;

    private void Awake()
    {
        DisableAll();
    }

    public void EnableHitbox(Vector2 dir)
    {
        DisableAll();

        if (dir == Vector2.up)
            hitboxUp.enabled = true;
        else if (dir == Vector2.down)
            hitboxDown.enabled = true;
        else if (dir == Vector2.left)
            hitboxLeft.enabled = true;
        else if (dir == Vector2.right)
            hitboxRight.enabled = true;
    }

    public void DisableAll()
    {
        hitboxUp.enabled = false;
        hitboxDown.enabled = false;
        hitboxLeft.enabled = false;
        hitboxRight.enabled = false;
    }
}
