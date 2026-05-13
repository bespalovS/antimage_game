using System.Collections;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Transform fill;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);
    [SerializeField] private float hideDelay = 2f;
    [SerializeField] private float barWidth = 1f;

    private Transform target;
    private int maxHealth;
    private SpriteRenderer[] renderers;
    private Coroutine hideCoroutine;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        SetVisible(false);
    }

    public void Setup(Transform enemyTransform, int max)
    {
        target = enemyTransform;
        maxHealth = max;
    }

    public void UpdateBar(int currentHealth)
    {
        float t = Mathf.Clamp01((float)currentHealth / maxHealth);

        fill.localScale = new Vector3(barWidth * t, fill.localScale.y, 1f);

        fill.localPosition = new Vector3(-barWidth / 2f * (1f - t), 0f, 0f);

        SetVisible(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        SetVisible(false);
    }

    private void SetVisible(bool value)
    {
        foreach (var sr in renderers)
            sr.enabled = value;
    }

    private void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + offset;
        else
            Destroy(gameObject);
    }
}