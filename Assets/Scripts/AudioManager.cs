using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudioSource, musicAudioSource;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
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

    public void playMusic(AudioClip clip)
    { 
        sfxAudioSource.PlayOneShot(clip, 0.5f);
    }
}
