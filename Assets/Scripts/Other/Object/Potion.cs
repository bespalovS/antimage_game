using System.Collections;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private float lifetime = 30f;
    [SerializeField] private float fadeDuration = 1f;

    private bool isPickedUp = false;

    private void Start()
    {
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(lifetime);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color color = sr.color;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            sr.color = color;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPickedUp) return;

        if (other.TryGetComponent(out Player player))
        {
            if (player.PickUpPotion())
            {
                isPickedUp = true;
                Destroy(gameObject);
            }
        }
    }
}
