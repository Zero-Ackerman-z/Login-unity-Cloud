using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public int points = 100; // Valor del objeto
    public AudioClip collectSound; // El sonido que se reproducirá al recoger
    private AudioSource sfxSource; // Fuente de audio

    void Start()
    {
        GameObject sfxObject = GameObject.Find("AudioManager/SFX");
        if (sfxObject != null)
        {
            sfxSource = sfxObject.GetComponent<AudioSource>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Añadir los puntos al puntaje
            ScoreManager4.Instance.AddPoints(points);

            // Reproducir el sonido de recolección si se ha configurado el clip y el AudioSource
            if (collectSound != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(collectSound);
            }

            // Destruir el objeto recolectado
            Destroy(gameObject);
        }
    }
}
