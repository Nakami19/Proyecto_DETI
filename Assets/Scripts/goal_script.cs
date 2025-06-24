using UnityEngine;

public class goal_script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           //Obtengo tiempo transcurrido
           float time = UIManager.Instance.GetTimeElapsed();
            if (time <= 0f) return; // evita división por cero
            //numero como base para la puntuacion
            float baseScore = 500;
            //Calculo la puntuacion
            int score = Mathf.RoundToInt(baseScore / time);
            int lives = GameManager.Instance.getLives();
            score = score * 100 + (lives * 100);
            Debug.Log(score);
            //Sumo a la puntuacion actual
            GameManager.Instance.score += score;
            UIManager.Instance.finishedLevelSequence();
            
        }
    }
}
