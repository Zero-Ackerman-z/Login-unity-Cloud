using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject player; // Referencia al jugador.
    private Timer timer; // Referencia al Timer.
    [SerializeField] private ProgressData progressData; // Datos de progreso del juego.

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    void Start()
    {
        timer = FindObjectOfType<Timer>(); // Encuentra el Timer en la escena.
        LoadProgress(); // Carga progreso al iniciar.
    }

    void Update()
    {
        // Guardar progreso con tecla "S".
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveProgress();
        }

        // Cargar progreso con tecla "L".
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadProgress();
        }
    }

    public void SaveProgress()
    {
        progressData.remainingTime = timer.GetRemainingTime(); // Guarda el tiempo restante.
        progressData.playerPosition = player.transform.position; // Guarda la posición del jugador.

        // Convierte datos a JSON y guarda en un archivo.
        string json = JsonUtility.ToJson(progressData);
        SaveData.Save("progressdata_timer.json", json);
        Debug.Log("Progress saved");
    }

    public void LoadProgress()
    {
        string json = SaveData.Load("progressdata_timer.json");
        if (json != null)
        {
            progressData = JsonUtility.FromJson<ProgressData>(json);

            // Restaura el tiempo restante usando Timer.
            timer.SetRemainingTime(progressData.remainingTime);

            // Restaura la posición del jugador.
            player.transform.position = progressData.playerPosition;

            Debug.Log("Progress loaded");
        }
        else
        {
            Debug.Log("No save data found");
        }
    }

    // Método que se llama cuando el tiempo se agota
    public void OnTimeExpired()
    {
        timer.SetGameOver(true); // Establece el estado de fin de juego en el Timer.
        Time.timeScale = 0; // Pausa el juego.
        Debug.Log("Game Over!");
    }

    // Método para disminuir el tiempo
    public void DecreaseTime(float amount)
    {
        if (amount < 0) // Si el amount es negativo, significa que estamos sumando tiempo
        {
            timer.AddTime(-amount); // Se agrega tiempo
        }
        else // Si el amount es positivo, significa que estamos restando tiempo
        {
            timer.SetRemainingTime(timer.GetRemainingTime() - amount);
        }
    }


    private void OnApplicationQuit()
    {
        SaveData.Delete("progressdata_timer.json");
        Debug.Log("Progress data deleted on quit");
    }
    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}

