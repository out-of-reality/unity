using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelConfigResponse
{
    public bool success;
    public LevelData level_data;
    public string timestamp;
    public string message;
}

[Serializable]
public class LevelData
{
    public string level_config_name;
    public int level_number;
    public int? patient_id;
    public string patient_name;
    public MovementPoses movement_poses;
    public Timing timing;

    [NonSerialized] public Dictionary<string, PoseDefinition> coin_poses_dict = new();
}

[Serializable]
public class MovementPoses
{
    public PoseDefinition forward;
    public PoseDefinition backward;
    public PoseDefinition left;
    public PoseDefinition right;
}

[Serializable]
public class Timing
{
    public float coin_challenge_time_limit;
}
