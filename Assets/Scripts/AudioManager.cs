using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX")]
    [Tooltip("Единый источник для коротких звуков (клики)")]
    [SerializeField] private AudioSource sfxSource;

    // Словарь: Ключ - сам звук (AudioClip), Значение - компонент AudioSource, который его играет
    private readonly Dictionary<AudioClip, AudioSource> loopingSources = new();

    [SerializeField] public AudioClip buttonSound;
    [SerializeField] public AudioClip buySound;
    [SerializeField] public AudioClip coinSound;
    [SerializeField] public AudioClip crankSound;
    [SerializeField] public AudioClip documentTakeSound;
    [SerializeField] public AudioClip OST;
    [SerializeField] public AudioClip RecyclerSound;
    [SerializeField] public AudioClip ShradderSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayLoopingSound(OST, 0.15f);
    }

    /// <summary>
    /// Проигрывает короткий звук поверх остальных (клики мышкой, эффекты)
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Включает зацикленный звук (фоновая музыка или долгий процесс шредера)
    /// </summary>
    public void PlayLoopingSound(AudioClip clip, float volume)
    {
        if (clip == null) return;

        if (!loopingSources.TryGetValue(clip, out AudioSource source) || source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0f;
            loopingSources[clip] = source;
        }

        if (source.isPlaying)
            return;

        source.clip = clip;
        source.loop = true;
        source.volume = volume;
        source.Play();
    }

    /// <summary>
    /// Останавливает ТОЛЬКО переданный зацикленный звук
    /// </summary>
    public void StopLoopingSound(AudioClip clip)
    {
        if (clip == null) return;

        if (loopingSources.TryGetValue(clip, out AudioSource source) && source != null)
            source.Stop();
    }
}
