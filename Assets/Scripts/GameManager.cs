using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Otros manejadores")]
    private Character player;
    //private UIManager uiManager;

    [Header("Variables del GameManager")]
    private int currentHealth = 3; // Number of lives the player has
    public int score;
    private int time;
    private string rank;

    public static GameManager Instance { get; private set; }
    #endregion

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Object.FindFirstObjectByType<Character>();
        //uiManager = Object.FindFirstObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, 3); // Ensure health does not go below 0 or above 3

        UIManager.Instance.UpdateHealthUI(currentHealth);
        UIManager.Instance.PlayerIconDamage();

        if (currentHealth == 0) {
            //reinicio puntaje
            score = 0;
            //vuelvo al menu
            navigator.Instance.GoToMainMenu();
        }

    }
    public void resetHP()
    { 
        currentHealth = 3;
    }

    public int getLives()
    { 
        return currentHealth;
    }

    public int getScore()
    {
        return score;
    }

    public string getRank()
    {
        if (score > 1000)
        {
            rank = "S";
        }
        else if (score > 700)
        {
            rank = "A";
        }
        else if (score > 500)
        {
            rank = "B";
        }
        else 
        {
            rank = "C";
        }
            return rank;
    }
}
