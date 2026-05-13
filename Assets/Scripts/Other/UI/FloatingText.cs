using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float moveSpeed = 1f;

    private TextMeshPro tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    public void Setup(int amount, Color color)
    {
        tmp.text = "+" + amount;
        tmp.color = color;
        StartCoroutine(AnimateAndDestroy());
    }

    private IEnumerator AnimateAndDestroy()
    {
        float time = 0f;
        Color startColor = tmp.color;

        while (time < lifetime)
        {
            time += Time.deltaTime;

            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            Color c = startColor;
            c.a = Mathf.Lerp(1f, 0f, time / lifetime);
            tmp.color = c;

            yield return null;
        }

        Destroy(gameObject);
    }
}