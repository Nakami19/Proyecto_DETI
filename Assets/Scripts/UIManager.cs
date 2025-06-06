using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    #region Variables

    [Header("Variables de la UI")]
    [Header("Icono del Personaje")]
    public Image playerIcon;
    public float bobAmount = 10f;
    public float bobSpeed = 2f;
    public Color normalColor = Color.white;
    public Color damageColor = Color.red;
    public float shakeDuration = 0.5f;
    public float shakeAmount = 5f;
    private Vector3 initialIconPosition;

    [Header("Barra de Vida")]
    public Image healthBar;
    public Sprite health3;
    public Sprite health2;
    public Sprite health1;
    public Sprite health0;
    public Image LowHealthOverlay;

    [Header("Temporizador")]
    public TextMeshProUGUI timerText;
    private float timeElapsed = 0f;
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialIconPosition = playerIcon.rectTransform.localPosition; // Guarda la posición inicial del icono
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer(Time.deltaTime);

        float offset = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        playerIcon.rectTransform.localPosition = initialIconPosition + new Vector3(0, offset, 0);
    }

    public void UpdateTimer(float deltaTime)
    {
        timeElapsed += deltaTime;
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        int miliseconds = Mathf.FloorToInt((timeElapsed * 100f) % 100f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);
    }

    public void UpdateHealthUI(int lives)
    {
        switch (lives)
        {
            case 3:
                healthBar.sprite = health3;
                break;
            case 2:
                healthBar.sprite = health2;
                break;
            case 1:
                healthBar.sprite = health1;
                StartCoroutine(FadeInOverlay(1f)); // 1 second fade duration
                break;
            case 0:
                healthBar.sprite = health0;
                break;
        }
    }

    public void PlayerIconDamage() 
    {
        StartCoroutine(DamageEffects());
    }

    private IEnumerator DamageEffects()
    {
        float timer = 0f;
        playerIcon.color = damageColor; // Cambia el color del icono a rojo

        while(timer < shakeDuration)
        {
            float xOffset = Random.Range(-shakeAmount, shakeAmount);
            float yOffset = Random.Range(-shakeAmount, shakeAmount);
            playerIcon.rectTransform.localPosition = initialIconPosition + new Vector3(xOffset, yOffset, 0);
            timer += Time.deltaTime;
            yield return null; // Espera un frame
        }
        playerIcon.rectTransform.localPosition = initialIconPosition; // Resetea la posición del icono
        playerIcon.color = normalColor; // Vuelve al color normal
    }

    private IEnumerator FadeInOverlay(float duration)
    {
        float elapsed = 0f;
        Color color = LowHealthOverlay.color;
        color.a = 0f;
        LowHealthOverlay.color = color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            LowHealthOverlay.color = color;
            yield return null;
        }

        color.a = 1f; // Ensure fully opaque at the end
        LowHealthOverlay.color = color;
    }

}
