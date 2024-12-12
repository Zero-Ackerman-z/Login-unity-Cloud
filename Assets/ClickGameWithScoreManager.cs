using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class ClickGameWithScoreManager : MonoBehaviour
{
    public static ClickGameWithScoreManager instance;
    public TextMeshProUGUI scoreText;
    private int totalScore = 0;

    // URL base de tu servidor PHP
    private string baseUrl = "http://localhost/";
    private int userId = 34; // ID del usuario, debe ser dinámico
    private int gameId = 9; // ID del juego, debe ser dinámico
    private bool scoreExists = false; // Indica si ya existe un puntaje en el servidor

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Al comenzar, verificar si existe un puntaje registrado en el servidor
        StartCoroutine(CheckIfScoreExists());
        UpdateScoreText();
    }

    void Update()
    {
        // Detecta el clic y agrega puntos
        if (Input.GetMouseButtonDown(0)) // 0 es el botón izquierdo del ratón
        {
            AddPoints(10); // Suma 10 puntos por clic
        }
    }

    // Método para agregar puntos y actualizar la UI
    public void AddPoints(int points)
    {
        totalScore += points;
        scoreText.text = "Puntaje: " + totalScore.ToString();

        // Al agregar puntos, intentamos actualizar el puntaje en el servidor
        if (scoreExists)
        {
            StartCoroutine(UpdateScoreOnServer(totalScore));
        }
        else
        {
            StartCoroutine(CreateScoreOnServer(totalScore));
        }
    }

    // Método para resetear el puntaje local
    public void ResetScore()
    {
        totalScore = 0;
        scoreText.text = "Puntaje: " + totalScore.ToString();

        StartCoroutine(DeleteScoreOnServer());
    }

    // Verificar si ya existe un puntaje en el servidor
    private IEnumerator CheckIfScoreExists()
    {
        string url = baseUrl + "get_score.php?gameid=" + gameId + "&userid=" + userId;

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            if (!string.IsNullOrEmpty(response) && !response.Contains("error"))
            {
                scoreExists = true; // El puntaje ya existe en el servidor
                Debug.Log("Score exists for user.");
            }
            else
            {
                scoreExists = false; // No hay puntaje registrado
                Debug.Log("No existing score found for user.");
            }
        }
        else
        {
            Debug.Log("Error checking score existence: " + request.error);
        }
    }

    // Crear el puntaje en el servidor si no existe
    private IEnumerator CreateScoreOnServer(int score)
    {
        string url = baseUrl + "create_score.php";

        ScoreData scoreData = new ScoreData(userId, gameId, score, "PlayerName");
        string jsonData = JsonUtility.ToJson(scoreData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score created successfully: " + request.downloadHandler.text);
            scoreExists = true; // Ahora sabemos que existe
        }
        else
        {
            Debug.Log("Error creating score: " + request.error);
        }
    }

    // Actualizar el puntaje en el servidor
    private IEnumerator UpdateScoreOnServer(int score)
    {
        string url = baseUrl + "update_score.php";

        ScoreData scoreData = new ScoreData(userId, gameId, score, "PlayerName");
        string jsonData = JsonUtility.ToJson(scoreData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score updated successfully: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error updating score: " + request.error);
        }
    }

    // Eliminar el puntaje en el servidor (opcional)
    private IEnumerator DeleteScoreOnServer()
    {
        string url = baseUrl + "delete_score.php";

        // Crear el objeto JSON para enviar
        ScoreData scoreData = new ScoreData(userId, gameId, totalScore, "PlayerName");
        string jsonData = JsonUtility.ToJson(scoreData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score deleted successfully.");
        }
        else
        {
            Debug.Log("Error deleting score: " + request.error);
        }
    }

    // Actualizar el texto del puntaje en la UI
    private void UpdateScoreText()
    {
        scoreText.text = "Puntaje: " + totalScore.ToString();
    }

    // Clase para manejar los datos del puntaje
    [System.Serializable]
    public class ScoreData
    {
        public int userid;
        public int gameid;
        public int score;
        public string created_by;

        public ScoreData(int userid, int gameid, int score, string created_by)
        {
            this.userid = userid;
            this.gameid = gameid;
            this.score = score;
            this.created_by = created_by;
        }
    }
}
