using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject[] collectableObjects; // Los prefabs de los objetos recolectables
    public Transform[] spawnPoints; // Dos puntos donde aparecerán los objetos
    public float spawnInterval = 2f; // Tiempo entre spawns

    void Start()
    {
        InvokeRepeating("SpawnCollectable", 1f, spawnInterval);
    }

    void SpawnCollectable()
    {
        int randomPoint = Random.Range(0, spawnPoints.Length);
        int randomObject = Random.Range(0, collectableObjects.Length);

        // Verificar que el objeto se spawnee dentro de la pantalla visible
        Vector3 spawnPosition = spawnPoints[randomPoint].position;

        // Instancia el objeto en la posición adecuada
        Instantiate(collectableObjects[randomObject], spawnPosition, Quaternion.identity);
    }
}
