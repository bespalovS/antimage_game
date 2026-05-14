using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Player")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip groundSlamSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip potionSound;

    [Header("UI")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip skillUpgradeSound;
    [SerializeField] private AudioClip noteSound;

    [Header("Enviroment")]
    [SerializeField] private AudioClip[] HitBrush;

    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.volume = 0.1f;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayRandomSound(AudioClip[] clips, float volume = 1f)
    {
        if (clips == null || clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        sfxSource.PlayOneShot(clip, volume);
    }
    
    public void PlayFootstep() => PlayRandomSound(footstepSounds, 0.2f);
    public void PlayAttack() => PlayRandomSound(attackSounds, 0.25f);
    public void PlayHit() => PlayRandomSound(hitSounds, 0.5f);
    public void PlayDeath() => PlaySound(deathSound, 0.8f);
    public void PlayGroundSlam() => PlaySound(groundSlamSound, 0.3f);
    public void PlayDash() => PlaySound(dashSound, 0.8f);
    public void PlayPotion() => PlaySound(potionSound, 0.8f);

    public void PlayButtonClick() => PlaySound(buttonClickSound);
    public void PlaySkillUpgrade() => PlaySound(skillUpgradeSound);
    public void PlayNote() => PlaySound(noteSound);

    public void PlayHitBrush() => PlayRandomSound(HitBrush, 0.7f);

    public void SetMusicVolume(float volume) => musicSource.volume = volume;
    public void SetSfxVolume(float volume) => sfxSource.volume = volume;
}