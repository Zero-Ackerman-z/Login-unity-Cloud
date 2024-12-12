using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab; // Asigna el prefab desde el Inspector
    public float spawnInterval = 2f; // Intervalo entre cada instanciaci�n
    public float destroyTime = 4f; // Tiempo despu�s del cual el prefab ser� destruido

    // Limites definidos manualmente
    public float minX = -10f; // L�mite izquierdo
    public float maxX = 10f;  // L�mite derecho
    public float minY = -5f;  // L�mite inferior
    public float maxY = 5f;   // L�mite superior

    void Start()
    {
        StartCoroutine(SpawnPrefabs());
    }

    IEnumerator SpawnPrefabs()
    {
        while (true)
        {
            Vector3 spawnPosition = GetRandomPositionInLimits();
            GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);

            // Destruir el prefab despu�s de un tiempo si no se ha hecho clic en �l
            Destroy(spawnedPrefab, destroyTime);

            // Esperar el intervalo antes de instanciar el siguiente prefab
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Vector3 GetRandomPositionInLimits()
    {
        // Generar una posici�n aleatoria dentro de los l�mites definidos
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Asumir z = 0 para 2D, ajustar si es 3D
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

        return spawnPosition;
    }
}
