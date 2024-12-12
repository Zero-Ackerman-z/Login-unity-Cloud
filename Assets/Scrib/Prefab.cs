using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Prefab : MonoBehaviour
{
    public SpriteRenderer indicatorSpriteRenderer; // Asigna el SpriteRenderer del indicador
    public Sprite indicatorSprite; // Sprite para el indicador
    public float initialSize = 1f; // Tama�o inicial del indicador
    public float minSize = 0.1f; // Tama�o m�nimo del indicador
    public float shrinkSpeed = 0.1f; // Velocidad a la que el indicador se reduce
    public float lifeTime = 4f; // Tiempo de vida del prefab

    private ScoreManager scoreManager; // Referencia al ScoreManager
    private float creationTime; // Tiempo de creaci�n del prefab

    void Start()
    {
        // Configurar el SpriteRenderer con el sprite del indicador
        indicatorSpriteRenderer.sprite = indicatorSprite;
        indicatorSpriteRenderer.transform.localScale = Vector3.one * initialSize; // Configurar el tama�o inicial del SpriteRenderer
        creationTime = Time.time; // Registrar el tiempo de creaci�n

        // Encontrar al ScoreManager en la escena
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        // Reducir el tama�o del SpriteRenderer con el tiempo
        float elapsedTime = Time.time - creationTime;
        float remainingLife = lifeTime - elapsedTime;

        if (remainingLife > 0)
        {
            // Reducir tama�o del SpriteRenderer basado en el tiempo transcurrido
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
        // Calcular el puntaje basado en el tama�o del indicador
        float sizeRatio = (indicatorSpriteRenderer.transform.localScale.x - minSize) / (initialSize - minSize);
        int score = CalculateScore(sizeRatio);

        if (scoreManager != null)
        {
            // A�adir el puntaje al ScoreManager
            scoreManager.AddScore(score);
        }

        // Destruir el prefab cuando se haga clic en �l
        Destroy(gameObject);
    }

    int CalculateScore(float sizeRatio)
    {
        if (sizeRatio > 0.4f) return 50; // Puntaje m�ximo si el tama�o est� cerca del inicial
        if (sizeRatio > 0.1f) return 100;  // Puntaje medio si el tama�o est� a la mitad
        return 300;                         // Puntaje m�nimo si el tama�o est� en el m�nimo
    }
}




