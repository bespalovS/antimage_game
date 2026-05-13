using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour damagableobject;
    [SerializeField] private Material blinkMaterial;
    [SerializeField] private float blinkDuration = 0.1f;

    private float blinkTimer;
    private Material defaultMaterial;
    private SpriteRenderer spriteRenderer;
    private bool isBlinking;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        isBlinking = true;
    }

    private void Start()
    {
        if (damagableobject is Player)
        {
            (damagableobject as Player).OnFlashBlink += DamagebleObject_OnFlashBlink;
        }
    }

    private void DamagebleObject_OnFlashBlink(object sender, System.EventArgs e)
    {
        SetBlinkingMaterial();
    }

    void Update()
    {
        if (isBlinking)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer < 0)
            {
                SetDefaultMaterial();
            }
                
        }
    }

    private void SetBlinkingMaterial()
    {
        blinkTimer = blinkDuration;
        spriteRenderer.material = blinkMaterial;
    }

    private void SetDefaultMaterial ()
    {
        spriteRenderer.material = defaultMaterial;
    }

    public void StopBlinking()
    {
        SetDefaultMaterial();
        isBlinking = false;
    }

    private void OnDestroy()
    {
        if (damagableobject is Player)
        {
            (damagableobject as Player).OnFlashBlink -= DamagebleObject_OnFlashBlink;
        }
    }
}
