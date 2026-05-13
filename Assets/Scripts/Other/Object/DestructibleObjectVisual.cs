using UnityEngine;

public class DestructibleObjectVisual : MonoBehaviour
{
    [SerializeField] private DestructibleObject destructibleObject;
    [SerializeField] private GameObject DestroyVineVFXPrefab;

    private void Start()
    {
        destructibleObject.OnDestructibleObjectTakeDamage += destructibleObject_OnDestructibleObjectTakeDamage;
    }

    private void destructibleObject_OnDestructibleObjectTakeDamage(object sender, System.EventArgs e)
    {
        ShowDestroyVFX();
    }

    private void ShowDestroyVFX()
    {
        Instantiate(DestroyVineVFXPrefab, destructibleObject.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        destructibleObject.OnDestructibleObjectTakeDamage -= destructibleObject_OnDestructibleObjectTakeDamage;
    }

}
