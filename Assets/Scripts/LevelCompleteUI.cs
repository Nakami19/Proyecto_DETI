using System.Collections;
using TMPro; // if using TextMeshPro
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("UI Text Elements")]
    public TMP_Text titleText;
    public TMP_Text timeLabel;
    public TMP_Text timeValue;
    public TMP_Text livesLabel;
    public TMP_Text livesValue;
    public TMP_Text scoreLabel;
    public TMP_Text scoreValue;
    public TMP_Text rankLabel;
    public TMP_Text rankValue;

    [Header("Animation Settings")]
    public float revealDelay = 1f;
    public float countSpeed = 1000f;

    [Header("Music")]
    [SerializeField] private AudioClip resultsSong;

    public void ShowStats(float timeTaken, int lives, int score, string rank)
    {
        AudioManager.Instance.playMusic(resultsSong, true);
        StartCoroutine(AnimateStats(timeTaken, lives, score, rank));
    }

    IEnumerator AnimateStats(float timeTaken, int lives, int score, string rank)
    {
        // Hide all texts first
        titleText.gameObject.SetActive(false);
        timeLabel.gameObject.SetActive(false);
        timeValue.gameObject.SetActive(false);
        livesLabel.gameObject.SetActive(false);
        livesValue.gameObject.SetActive(false);
        scoreLabel.gameObject.SetActive(false);
        scoreValue.gameObject.SetActive(false);
        rankLabel.gameObject.SetActive(false);
        rankValue.gameObject.SetActive(false);

        // Wait a moment after title
        titleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(revealDelay);

        // TIME
        timeLabel.gameObject.SetActive(true);
        timeValue.gameObject.SetActive(true);
        float displayTime = 0f;
        while (displayTime < timeTaken)
        {
            displayTime += Time.deltaTime * 10f; // Speed up time animation
            timeValue.text = FormatTime(displayTime);
            yield return null;
        }
        timeValue.text = FormatTime(timeTaken);
        yield return new WaitForSeconds(revealDelay);

        // LIVES
        livesLabel.gameObject.SetActive(true);
        livesValue.gameObject.SetActive(true);
        int displayedLives = 0;
        while (displayedLives < lives)
        {
            displayedLives++;
            livesValue.text = displayedLives.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(revealDelay);

        // SCORE
        scoreLabel.gameObject.SetActive(true);
        scoreValue.gameObject.SetActive(true);
        int displayedScore = 0;
        while (displayedScore < score)
        {
            displayedScore += Mathf.CeilToInt(countSpeed * Time.deltaTime);
            displayedScore = Mathf.Min(displayedScore, score);
            scoreValue.text = displayedScore.ToString("D6");
            yield return null;
        }
        yield return new WaitForSeconds(revealDelay);

        // RANK
        rankLabel.gameObject.SetActive(true);
        rankValue.gameObject.SetActive(true);
        rankValue.text = rank;
        yield return new WaitForSeconds(3f);

        Scene currentScene = SceneManager.GetActiveScene();
        print(currentScene.name);

        //Ir al siguiente nivel
        if (currentScene.name == "Level2")
        {
            navigator.Instance.GoToMainMenu();
        }
        else 
        {
            navigator.Instance.GoToLevel2();
        }
            
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int milliseconds = Mathf.FloorToInt((t * 100f) % 100f);
        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}