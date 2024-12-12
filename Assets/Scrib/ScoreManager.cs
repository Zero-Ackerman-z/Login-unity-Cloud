using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI comboScoreText;
    public TextMeshProUGUI cumulativeScoreText;
  
    private int comboScore = 0;
    private int cumulativeScore = 0;

    private float comboDisplayTime = 2f;

    // URL base de tu servidor PHP
    private string baseUrl = "http://localhost/";
    private int userId = 14; // ID del usuario
    private int gameId = 1; // ID del juego
    private bool scoreExists = false; // Indicador

    void Start()
    {
        UpdateCumulativeScoreText();
        HideComboScore();

        // Verificar si el puntaje ya existe en el servidor
        StartCoroutine(CheckIfScoreExists());
    }

    public void AddScore(int score)
    {
        comboScore = score;
        cumulativeScore += comboScore;

        comboScoreText.text = comboScore.ToString() + "X";
        comboScoreText.gameObject.SetActive(true);

        UpdateCumulativeScoreText();
        Invoke("HideComboScore", comboDisplayTime);

        // Enviar el puntaje al servidor
        if (scoreExists)
        {
            StartCoroutine(UpdateScoreOnServer(cumulativeScore));
        }
        else
        {
            StartCoroutine(CreateScoreOnServer(cumulativeScore));
        }
    }

    void UpdateCumulativeScoreText()
    {
        cumulativeScoreText.text = cumulativeScore.ToString();
    }

    void HideComboScore()
    {
        comboScoreText.gameObject.SetActive(false);
    }

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
                scoreExists = true; 
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
            scoreExists = true; 
        }
        else
        {
            Debug.Log("Error creating score: " + request.error);
        }
    }

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
    private IEnumerator DeleteScoreOnServer()
    {
        string url = baseUrl + "delete_score.php";
        ScoreData scoreData = new ScoreData(userId, gameId, cumulativeScore, "PlayerName");
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
    private IEnumerator GetHighestScoresFromServer()
    {
        string url = baseUrl + "highest_scores.php";  
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log("Scores listed: " + response);
            ScoreList scoreList = JsonUtility.FromJson<ScoreList>(response);
            if (scoreList.items.Length > 0)
            {
                foreach (Score score in scoreList.items)
                {
                    Debug.Log("UserID: " + score.userid + ", Score: " + score.score);
                }
            }
            else
            {
                Debug.Log("No high scores found.");
            }
        }
        else
        {
            Debug.Log("Error fetching scores: " + request.error);
        }
    }
    [System.Serializable]
    public class ScoreList
    {
        public Score[] items;
    }
    [System.Serializable]
    public class Score
    {
        public int userid;
        public int score;
    }
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