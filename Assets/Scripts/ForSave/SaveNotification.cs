using System.Collections;
using TMPro;
using UnityEngine;

public class SaveNotification : MonoBehaviour
{
    public static SaveNotification Instance { get; private set; }

    [SerializeField] private GameObject notificationObject;
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private float fadeDuration = 1f;

    private TextMeshProUGUI tmp;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        Instance = this;
        tmp = notificationObject.GetComponent<TextMeshProUGUI>();
        notificationObject.SetActive(false);
    }

    public void Show()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        notificationObject.SetActive(true);
        Color color = tmp.color;
        color.a = 1f;
        tmp.color = color;

        yield return new WaitForSeconds(displayTime);

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            tmp.color = color;
            yield return null;
        }

        notificationObject.SetActive(false);
    }
}