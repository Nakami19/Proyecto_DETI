using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private AudioClip menuClickSound; // Sound to play when switching scenes
    [SerializeField] private AudioClip MainMusic; 

    private void Start()
    {
        // Play the main music when the menu starts
        AudioManager.Instance.playMusic(MainMusic);
    }

    public void switchScene(string SceneName) {
        AudioManager.Instance.playSound(menuClickSound); // Play sound when switching scenes
        SceneManager.LoadScene(SceneName);
    }
}
