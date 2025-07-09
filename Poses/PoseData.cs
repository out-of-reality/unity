using System.Collections.Generic;

[System.Serializable]
public class PoseData
{
    public bool success;
    public List<PoseDefinition> poses;
    public int total_poses;
    public string timestamp;
    public string message;
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