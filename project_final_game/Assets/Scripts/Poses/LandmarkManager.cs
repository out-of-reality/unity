using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public class LandmarkManager : MonoBehaviour
{
    public Dictionary<string, Vector3> landmarks = new Dictionary<string, Vector3>();

    public void UpdateLandmarksFromJson(string json)
    {
        var data = JSON.Parse(json);
        foreach (KeyValuePair<string, JSONNode> kv in data)
        {
            var arr = kv.Value.AsArray;
            if (arr.Count == 3)
            {
                landmarks[kv.Key] = new Vector3(arr[0].AsFloat, arr[1].AsFloat, arr[2].AsFloat);
            }
        }
    }

    public Vector3 Get(string name)
    {
        return landmarks.ContainsKey(name) ? landmarks[name] : Vector3.zero;
    }
}
