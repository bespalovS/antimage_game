using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner Instance { get; private set; }

    [SerializeField] private GameObject floatingTextPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnHealText(int amount, Vector3 position)
    {
        GameObject obj = Instantiate(floatingTextPrefab, position + Vector3.up * 2f, Quaternion.identity);
        obj.GetComponent<FloatingText>().Setup(amount, Color.green);
    }
}