using System.Collections;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float transparentAmout = 0.5f;
    [SerializeField] private float fadeTime = 0.5f;

    SpriteRenderer spriteRenderer;

    private const float fullNonTransparent = 1.0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>())
        {
            if (collider is CapsuleCollider2D)
                StartCoroutine(fadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparentAmout));
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>())
        {
            if (collider is CapsuleCollider2D)
                StartCoroutine(fadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, fullNonTransparent));
        }
    }

    private IEnumerator fadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startTrancparencyAmount, float targetTrancparencyAmount)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startTrancparencyAmount, targetTrancparencyAmount, elapsedTime/fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);

            yield return null;
        }
    }

}
