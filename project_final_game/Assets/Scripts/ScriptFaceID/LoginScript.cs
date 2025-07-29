using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginScript : MonoBehaviour
{
    [Header("User Inputs")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    [Header("UI Elements")]
    public TMP_Text errorMessage;
    public WebcamScript webcamScript;

    private string odooUrl = "http://localhost:17069/out_of_reality_api/login";
    private string faceIdUrl = "http://localhost:17069/out_of_reality_api/faceid_login";

    private void Start()
    {
        errorMessage.gameObject.SetActive(false);
    }

    public void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        StartCoroutine(LoginCoroutine(email, password));
    }

    public void LoginWithFaceID()
    {
        StartCoroutine(FaceIDCoroutine());
    }

    IEnumerator LoginCoroutine(string email, string password)
    {
        string payload = JsonUtility.ToJson(new LoginData { username = email, password = password });

        using (UnityWebRequest www = new UnityWebRequest(odooUrl, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(payload);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                APIResponse response = JsonUtility.FromJson<APIResponse>(jsonResponse);

                if (!string.IsNullOrEmpty(response.access_token))
                {
                    PlayerPrefs.SetString("ACCESS_TOKEN", response.access_token);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    ShowErrorMessage("Credenciales incorrectas.");
                }
            }
            else
            {
                ShowErrorMessage("Error: " + www.error);
            }
        }
    }

    IEnumerator FaceIDCoroutine()
    {
        WebCamTexture cam = webcamScript.WebcamTexture;
        if (cam == null || !cam.isPlaying)
        {
            ShowErrorMessage("Webcam no disponible.");
            yield break;
        }

        Texture2D tex = new Texture2D(cam.width, cam.height);
        tex.SetPixels(cam.GetPixels());
        tex.Apply();

        byte[] imageBytes = tex.EncodeToPNG();
        string base64Image = Convert.ToBase64String(imageBytes);

        Destroy(tex);

        string payload = JsonUtility.ToJson(new FaceIDData { image = base64Image });

        using (UnityWebRequest www = new UnityWebRequest(faceIdUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(payload));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                APIResponse response = JsonUtility.FromJson<APIResponse>(jsonResponse);

                if (!string.IsNullOrEmpty(response.access_token))
                {
                    PlayerPrefs.SetString("ACCESS_TOKEN", response.access_token);
                    webcamScript.StopCamera();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else
                {
                    ShowErrorMessage("FaceID falló.");
                }
            }
            else
            {
                ShowErrorMessage("Error: " + www.error);
            }
        }
    }


    private void ShowErrorMessage(string message)
    {
        errorMessage.text = message;
        errorMessage.gameObject.SetActive(true);
    }
}

[Serializable]
public class LoginData
{
    public string username;
    public string password;
}

[Serializable]
public class FaceIDData
{
    public string image;
}

[Serializable]
public class APIResponse
{
    public int id;
    public string name;
    public string email;
    public string access_token;
}
