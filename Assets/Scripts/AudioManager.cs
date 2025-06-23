using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource, musicAudioSource;

    public static AudioManager Instance { get; private set; }

    [SerializeField] private float musicFadeDuration = 0.1f; // Duration of fade in seconds

    private Coroutine currentFadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void playSound(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip, 0.7f);
    }

    public void playMusic(AudioClip newClip, bool skipFade = false)
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);

        if (skipFade)
        {
            // Play instantly
            musicAudioSource.clip = newClip;
            musicAudioSource.volume = 0.5f;
            musicAudioSource.Play();
        }
        else
        {
            // Use fade-out then fade-in
            currentFadeCoroutine = StartCoroutine(FadeOutAndChangeMusic(newClip));
        }
    }

    private IEnumerator FadeOutAndChangeMusic(AudioClip newClip)
    {
        float startVolume = musicAudioSource.volume;

        // Step 1: Fade out current music
        float t = 0f;
        while (t < musicFadeDuration)
        {
            t += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / musicFadeDuration);
            yield return null;
        }

        musicAudioSource.Stop();
        musicAudioSource.clip = newClip;

        // Step 2: Play new music
        musicAudioSource.volume = 0f;
        musicAudioSource.Play();

        // Step 3: Fade in new music (optional)
        t = 0f;
        //while (t < musicFadeDuration)
        //{
        //    t += Time.deltaTime;
        //    musicAudioSource.volume = Mathf.Lerp(0f, startVolume, t / musicFadeDuration);
        //    yield return null;
        //}

        musicAudioSource.volume = startVolume;
    }
}
