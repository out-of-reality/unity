using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TMP_Text welcomeMessage;

    private string whoamiUrl = "http://127.0.0.1:17069/out_of_reality_api/whoami";

    void Start()
    {
        StartCoroutine(FetchPlayerName());
    }

    public void GoLevelOne()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FetchPlayerName()
    {
        string accessToken = PlayerPrefs.GetString("ACCESS_TOKEN");

        using (UnityWebRequest www = new UnityWebRequest(whoamiUrl, "GET"))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + accessToken);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                WhoAmIResponse response = JsonUtility.FromJson<WhoAmIResponse>(jsonResponse);

                welcomeMessage.text = "Bienvenido " + response.name;
            }
            else
            {
                welcomeMessage.text = "Error al cargar el nombre del jugador.";
            }
        }
    }
}

[System.Serializable]
public class WhoAmIResponse
{
    public string name;
}
