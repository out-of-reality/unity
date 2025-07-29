using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections.Generic;

public class LevelConfigManager : MonoBehaviour
{
    private string baseUrl = "http://127.0.0.1:17069/out_of_reality_api";

    public static LevelData LoadedLevelData { get; private set; }

    public void GetLevelConfigForPatient(int levelNumber)
    {
        StartCoroutine(FetchLevelConfigForPatientCoroutine(levelNumber));
    }

    private IEnumerator FetchLevelConfigForPatientCoroutine(int levelNumber)
    {
        string endpoint = $"{baseUrl}/levels/{levelNumber}";
        string accessToken = PlayerPrefs.GetString("ACCESS_TOKEN");

        if (string.IsNullOrEmpty(accessToken))
        {
            Debug.LogError("Access Token is missing for authenticated level configuration request. Please log in first.");
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequest.Get(endpoint))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + accessToken);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                try
                {
                    LevelConfigResponse levelResponse = JsonUtility.FromJson<LevelConfigResponse>(jsonResponse);

                    if (levelResponse.success)
                    {
                        LevelData levelData = levelResponse.level_data;
                        ParseCoinPosesManually(levelData, jsonResponse);
                        Debug.Log($"Successfully retrieved Level {levelNumber} for Patient.");
                        LoadedLevelData = levelData;
                        LogLevelData(levelData, levelResponse.timestamp);
                    }
                    else
                    {
                        Debug.LogError($"API Success False (Patient Level {levelNumber}): {levelResponse.message}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error parsing JSON response (Patient Level {levelNumber}): {e.Message}\nJSON: {jsonResponse}");
                }
            }
            else
            {
                Debug.LogError($"HTTP Error (Patient Level {levelNumber}): {www.error}\nResponse: {www.downloadHandler.text}");
            }
        }
    }

    private void ParseCoinPosesManually(LevelData levelData, string fullJson)
    {
        var root = JSON.Parse(fullJson);
        var coinJson = root["level_data"]["coin_poses"];
        foreach (KeyValuePair<string, JSONNode> kv in coinJson)
        {
            var pose = JsonUtility.FromJson<PoseDefinition>(kv.Value.ToString());
            levelData.coin_poses_dict[kv.Key] = pose;
        }
    }

    private void LogLevelData(LevelData levelData, string responseTimestamp)
    {
        string mensaje = "Level Configuration Data:";
        mensaje += $"\nLevel Config Name: {levelData.level_config_name}";
        mensaje += $"\nLevel Number: {levelData.level_number}";
        mensaje += $"\nPatient Name: {levelData.patient_name ?? "N/A"}";
        mensaje += $"\nMovement Poses:";
        if (levelData.movement_poses != null)
        {
            mensaje += $"\n - Forward: {levelData.movement_poses.forward?.name ?? "None"}";
            mensaje += $"\n - Backward: {levelData.movement_poses.backward?.name ?? "None"}";
            mensaje += $"\n - Left: {levelData.movement_poses.left?.name ?? "None"}";
            mensaje += $"\n - Right: {levelData.movement_poses.right?.name ?? "None"}";
        }
        mensaje += $"\nCoin Poses:";
        foreach (var kv in levelData.coin_poses_dict)
        {
            mensaje += $"\n - {kv.Key}: {kv.Value?.name ?? "None"}";
        }
        mensaje += $"\nCoin Challenge Time Limit: {levelData.timing.coin_challenge_time_limit} seconds";
        mensaje += $"\nTimestamp: {responseTimestamp}";
        Debug.Log(mensaje);
    }
}
