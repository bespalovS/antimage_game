using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private AudioClip fireballExplosinSound;
    [SerializeField][Range(0f, 1f)] private float fireballExplosinVolume = 1f;

    private void Start()
    {
        AudioManager.Instance.PlaySound(fireballExplosinSound, fireballExplosinVolume);
        Destroy(gameObject, lifetime);
    }

}
