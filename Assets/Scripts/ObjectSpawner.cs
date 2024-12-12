using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab; // El prefab que deseas instanciar
    public Transform[] spawnPoints; // Puntos donde puede aparecer
    public float lifeTime = 5f; // Tiempo de vida del objeto

    private GameObject currentObject;

    void Start()
    {
        SpawnNewObject();
    }

    void Update()
    {
        if (currentObject == null)
        {
            SpawnNewObject();
        }
    }

    void SpawnNewObject()
    {
        Vector3 spawnPosition = GetRandomSpawnPoint();
        currentObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        currentObject.GetComponent<Lifetime>().Initialize(lifeTime, this);
    }

    Vector3 GetRandomSpawnPoint()
    {
        Vector3 spawnPosition;
        bool positionAvailable = false;

        do
        {
            // Escoge un punto aleatorio
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            spawnPosition = randomPoint.position;

            // Verifica que no haya otro objeto en ese punto
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 0.5f);
            positionAvailable = colliders.Length == 0;

        } while (!positionAvailable);

        return spawnPosition;
    }

    public void OnObjectDestroyed()
    {
        currentObject = null; // Permitir instanciar nuevamente en el próximo Update
    }
}



