using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Indicator : MonoBehaviour
{
    public float startSize = 1f; // Tamaño inicial del indicador
    public float minSize = 0.1f; // Tamaño mínimo del indicador
    public float shrinkRate = 0.1f; // Velocidad de reducción del tamaño
    public float timeToLive = 4f; // Tiempo total de vida del indicador

    private RectTransform rectTransform;
    private ScoreManager scoreManager;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        scoreManager = FindObjectOfType<ScoreManager>();

        // Iniciar el proceso de reducción del tamaño
        InvokeRepeating("Shrink", 0f, 0.1f);
        Invoke("DestroyIndicator", timeToLive);
    }

    void Shrink()
    {
        if (rectTransform != null)
        {
            float newSize = Mathf.Max(rectTransform.sizeDelta.x - shrinkRate, minSize);
            rectTransform.sizeDelta = new Vector2(newSize, newSize);

            if (rectTransform.sizeDelta.x <= minSize)
            {
                // El indicador ha llegado a su tamaño mínimo
                CancelInvoke("Shrink");
            }
        }
    }

    void DestroyIndicator()
    {
        // Destruir el indicador cuando el tiempo se acabe
        Destroy(gameObject);
    }

    public void OnClick()
    {
        // Calcular el puntaje basado en el tamaño actual del indicador
        float size = rectTransform.sizeDelta.x;
        int score = 0;

        if (size <= minSize)
        {
            score = 300;
        }
        else if (size <= minSize * 2)
        {
            score = 100;
        }
        else
        {
            score = 50;
        }
        if (scoreManager != null)
        {
            scoreManager.AddScore(score);
        }

        // Destruir el indicador al hacer clic
        Destroy(gameObject);
    }
}

