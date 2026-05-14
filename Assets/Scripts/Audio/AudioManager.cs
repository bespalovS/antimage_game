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
    [SerializeField] private AudioClip blockSound;

    [Header("Enemy")]
    [SerializeField] private AudioClip[] enemyHitSounds;
    [SerializeField] private AudioClip[] enemyDeathSounds;
    [SerializeField] private AudioClip[] enemyAttackSounds;

    [Header("UI")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip skillUpgradeSound;
    [SerializeField] private AudioClip saveSound;

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

    public void PlayFootstep() => PlayRandomSound(footstepSounds, 0.5f);
    public void PlayAttack() => PlayRandomSound(attackSounds);
    public void PlayHit() => PlayRandomSound(hitSounds);
    public void PlayDeath() => PlaySound(deathSound);
    public void PlayGroundSlam() => PlaySound(groundSlamSound);
    public void PlayDash() => PlaySound(dashSound);
    public void PlayPotion() => PlaySound(potionSound);
    public void PlayBlock() => PlaySound(blockSound);

    public void PlayEnemyHit() => PlayRandomSound(enemyHitSounds);
    public void PlayEnemyDeath() => PlayRandomSound(enemyDeathSounds);
    public void PlayEnemyAttack() => PlayRandomSound(enemyAttackSounds);

    public void PlayButtonClick() => PlaySound(buttonClickSound);
    public void PlaySkillUpgrade() => PlaySound(skillUpgradeSound);
    public void PlaySave() => PlaySound(saveSound);

    public void SetMusicVolume(float volume) => musicSource.volume = volume;
    public void SetSfxVolume(float volume) => sfxSource.volume = volume;
}