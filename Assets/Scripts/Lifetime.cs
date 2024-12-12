using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    private float lifeTime;
    private ObjectSpawner spawner;

    public void Initialize(float time, ObjectSpawner objectSpawner)
    {
        lifeTime = time;
        spawner = objectSpawner;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            DestroyObject();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.DecreaseTime(-3f);
            Scoremanagerjuego1.Instance.AddPoints(100);
            AudioManager.Instance.PlayMushroomPickupSound();
            Destroy(gameObject);
        }
    }

    void DestroyObject()
    {
        // Llama al GameManager para reducir el tiempo
        GameManager.Instance.DecreaseTime(2f);
        AudioManager.Instance.PlayObjectDestroySound();


        spawner.OnObjectDestroyed();
        Destroy(gameObject);
    }
}
