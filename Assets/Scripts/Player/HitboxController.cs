using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public static HitboxController Instance { get; private set; }

    [SerializeField] private GameObject hitboxUp;
    [SerializeField] private GameObject hitboxDown;
    [SerializeField] private GameObject hitboxLeft;
    [SerializeField] private GameObject hitboxRight;

    private GameObject activeHitbox;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenHitbox(Vector2 dir)
    {
        CloseHitbox();

        activeHitbox = ResolveHitbox(dir);
        activeHitbox?.SetActive(true);
    }

    public void CloseHitbox()
    {
        activeHitbox?.SetActive(false);
        activeHitbox = null;
    }

    private GameObject ResolveHitbox(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            return dir.x >= 0f ? hitboxRight : hitboxLeft;
        else
            return dir.y >= 0f ? hitboxUp : hitboxDown;
    }
    public void DealDamage(Vector2 dir)
    {
        activeHitbox?.GetComponent<Hitbox>()?.DealDamage(dir);
    }
}
