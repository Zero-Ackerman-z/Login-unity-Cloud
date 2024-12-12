using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Prefab : MonoBehaviour
{
    public SpriteRenderer indicatorSpriteRenderer; // Asigna el SpriteRenderer del indicador
    public Sprite indicatorSprite; // Sprite para el indicador
    public float initialSize = 1f; // Tamaño inicial del indicador
    public float minSize = 0.1f; // Tamaño mínimo del indicador
    public float shrinkSpeed = 0.1f; // Velocidad a la que el indicador se reduce
    public float lifeTime = 4f; // Tiempo de vida del prefab

    private ScoreManager scoreManager; // Referencia al ScoreManager
    private float creationTime; // Tiempo de creación del prefab

    void Start()
    {
        // Configurar el SpriteRenderer con el sprite del indicador
        indicatorSpriteRenderer.sprite = indicatorSprite;
        indicatorSpriteRenderer.transform.localScale = Vector3.one * initialSize; // Configurar el tamaño inicial del SpriteRenderer
        creationTime = Time.time; // Registrar el tiempo de creación

        // Encontrar al ScoreManager en la escena
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        // Reducir el tamaño del SpriteRenderer con el tiempo
        float elapsedTime = Time.time - creationTime;
        float remainingLife = lifeTime - elapsedTime;

        if (remainingLife > 0)
        {
            // Reducir tamaño del SpriteRenderer basado en el tiempo transcurrido
            float newSize = Mathf.Max(minSize, initialSize * (remainingLife / lifeTime));
            indicatorSpriteRenderer.transform.localScale = Vector3.one * newSize;

            // Destruir el prefab cuando el tiempo de vida se agote
            if (remainingLife <= 0)
            {
                Debug.Log("Prefab Destroyed due to lifeTime expiration");
                Destroy(gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        // Calcular el puntaje basado en el tamaño del indicador
        float sizeRatio = (indicatorSpriteRenderer.transform.localScale.x - minSize) / (initialSize - minSize);
        int score = CalculateScore(sizeRatio);

        if (scoreManager != null)
        {
            // Añadir el puntaje al ScoreManager
            scoreManager.AddScore(score);
        }

        // Destruir el prefab cuando se haga clic en él
        Destroy(gameObject);
    }

    int CalculateScore(float sizeRatio)
    {
        if (sizeRatio > 0.4f) return 50; // Puntaje máximo si el tamaño está cerca del inicial
        if (sizeRatio > 0.1f) return 100;  // Puntaje medio si el tamaño está a la mitad
        return 300;                         // Puntaje mínimo si el tamaño está en el mínimo
    }
}




