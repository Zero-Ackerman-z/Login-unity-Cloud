using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DBConn : MonoBehaviour
{
    private string url = "http://localhost/user_login.php";

    [SerializeField]private User user;
    [SerializeField]private ServerResponse response;

    public void Username(string username)
    {
        user.username = username;
    }

    public void Password(string password)
    {
        user.password = password;
    }

    public void Login()
    {
        StartCoroutine("LoginE");
    }

    IEnumerator LoginE()
    {        
        string jsonString = JsonUtility.ToJson(user);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Server Response: " + responseText); 
            response = JsonUtility.FromJson<ServerResponse>(responseText);

            Debug.Log("Response message: " + response.message);  

            if (response.message.Trim() == "Login successful") 
            {
                SceneHelper.LoadScene(1);
            }
            else
            {
                Debug.Log("Login Failed: " + response.message); 
            }
        }

    }
}

[System.Serializable]
public class User
{
    public string username;
    public string password;
}

[System.Serializable]
public class ServerResponse
{
    public string message;
}
