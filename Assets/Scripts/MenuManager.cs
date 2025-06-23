using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button playButton;
    [SerializeField] private AudioClip menuClickSound; // Sound to play when switching scenes
    [SerializeField] private AudioClip MainMusic; 

    private void Start()
    {
        // Play the main music when the menu starts
        AudioManager.Instance.playMusic(MainMusic, skipFade: true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() =>
        {
            navigator.Instance.GoToLevel1();
        });
    }

    public void switchScene(string SceneName) {
        AudioManager.Instance.playSound(menuClickSound); // Play sound when switching scenes
        SceneManager.LoadScene(SceneName);
    }
}
