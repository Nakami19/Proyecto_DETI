using UnityEngine;
using UnityEngine.SceneManagement;

public class navigator : MonoBehaviour
{
    //variable para acceder a la clase
    public static navigator Instance;

    [Header("Scene Names")]
    public string mainMenuScene = "Main_Menu";
    public string level1Scene = "Level1";
    public string level2Scene = "Level2";
    public string gameOverScene = "Game_Over";

    void Awake()
    {
        if (Instance == null)
        {
            //Creo la instancia y digo que persista entre escenas
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
           // Si ya existe otra instancia, es destruida.
            Destroy(gameObject);
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void GoToLevel1()
    {
        SceneManager.LoadScene(level1Scene);
    }

    public void GoToLevel2()
    {
        SceneManager.LoadScene(level2Scene);
    }

    public void GoToGameOver()
    {
        SceneManager.LoadScene(gameOverScene);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
