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

[System.Serializable]
public class PoseDefinition
{
    public int id;
    public string name;
    public string description;
    public bool require_feet_on_ground;
    public float foot_tolerance_y;
    public bool active;
    public List<AngleRequirement> angle_requirements;
}

[System.Serializable]
public class AngleRequirement
{
    public string angle;
    public float min_angle;
    public float max_angle;
    public string description;
}
