using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager5 : MonoBehaviour
{
    public void ResetScene()
    {
        // Obtiene el nombre de la escena actual y la vuelve a cargar
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
