using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource backgroundMusicSource; // Fuente para la música de fondo
    public AudioSource mushroomPickupSource; // Fuente para recoger hongos
    public AudioSource objectDestroySource; // Fuente para la destrucción de objetos

    public AudioClip backgroundMusic; // Música de fondo
    public AudioClip alternativeMusic; // Música alternativa

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayMushroomPickupSound()
    {
        mushroomPickupSource.Play();
    }

    public void PlayObjectDestroySound()
    {
        objectDestroySource.Play();
    }

    public void SwitchToAlternativeMusic()
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = alternativeMusic;
        backgroundMusicSource.Play();
    }
}
