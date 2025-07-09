using UnityEngine;
using System.Collections.Generic;

public enum AnguloArticular
{
    RIGHT_ELBOW,
    LEFT_ELBOW,
    RIGHT_SHOULDER,
    LEFT_SHOULDER,
    RIGHT_KNEE,
    LEFT_KNEE,
    RIGHT_HIP,
    LEFT_HIP,
    RIGHT_ANKLE,
    LEFT_ANKLE
}

public class LecturaPose : MonoBehaviour
{
    public LandmarkManager landmarkManager;

    private Dictionary<AnguloArticular, string[]> anguloPuntos = new Dictionary<AnguloArticular, string[]>()
    {
        { AnguloArticular.RIGHT_ELBOW, new string[] { "RIGHT_SHOULDER", "RIGHT_ELBOW", "RIGHT_WRIST" } },
        { AnguloArticular.LEFT_ELBOW, new string[] { "LEFT_SHOULDER", "LEFT_ELBOW", "LEFT_WRIST" } },
        { AnguloArticular.RIGHT_SHOULDER, new string[] { "RIGHT_HIP", "RIGHT_SHOULDER", "RIGHT_ELBOW" } },
        { AnguloArticular.LEFT_SHOULDER, new string[] { "LEFT_HIP", "LEFT_SHOULDER", "LEFT_ELBOW" } },
        { AnguloArticular.RIGHT_KNEE, new string[] { "RIGHT_HIP", "RIGHT_KNEE", "RIGHT_ANKLE" } },
        { AnguloArticular.LEFT_KNEE, new string[] { "LEFT_HIP", "LEFT_KNEE", "LEFT_ANKLE" } },
        { AnguloArticular.RIGHT_HIP, new string[] { "RIGHT_SHOULDER", "RIGHT_HIP", "RIGHT_KNEE" } },
        { AnguloArticular.LEFT_HIP, new string[] { "LEFT_SHOULDER", "LEFT_HIP", "LEFT_KNEE" } },
        { AnguloArticular.RIGHT_ANKLE, new string[] { "RIGHT_KNEE", "RIGHT_ANKLE", "RIGHT_FOOT_INDEX" } },
        { AnguloArticular.LEFT_ANKLE, new string[] { "LEFT_KNEE", "LEFT_ANKLE", "LEFT_FOOT_INDEX" } }
    };

    private Dictionary<AnguloArticular, float> angulosCalculados = new Dictionary<AnguloArticular, float>();

    private const float footFlatToleranceY = 0.08f;

    void Start()
    {
        foreach (AnguloArticular angulo in System.Enum.GetValues(typeof(AnguloArticular)))
        {
            angulosCalculados[angulo] = 0f;
        }
    }

    void Update()
    {
        if (landmarkManager == null) return;

        if (landmarkManager.landmarks.Count > 0)
        {
            CalcularTodosLosAngulos();
        }
    }

    private void CalcularTodosLosAngulos()
    {
        foreach (AnguloArticular angulo in System.Enum.GetValues(typeof(AnguloArticular)))
        {
            string[] puntos = anguloPuntos[angulo];

            Vector3 puntoA = ObtenerLandmark(puntos[0], out bool encontradoA);
            Vector3 puntoB = ObtenerLandmark(puntos[1], out bool encontradoB);
            Vector3 puntoC = ObtenerLandmark(puntos[2], out bool encontradoC);

            if (encontradoA && encontradoB && encontradoC)
            {
                puntoA.z /= 2f;
                puntoB.z /= 2f;
                puntoC.z /= 2f;
                Vector3 ab = puntoA - puntoB;
                Vector3 bc = puntoC - puntoB;
                float anguloCalculado = Vector3.Angle(ab, bc);
                angulosCalculados[angulo] = anguloCalculado;
            }
        }
    }

    private Vector3 ObtenerLandmark(string nombreLandmark, out bool encontrado)
    {
        Vector3 resultado = landmarkManager.Get(nombreLandmark);
        encontrado = resultado != Vector3.zero;
        return resultado;
    }

    private void RegistrarTodosLosAngulos()
    {
        string mensaje = "Ángulos articulares:\n";
        foreach (AnguloArticular angulo in System.Enum.GetValues(typeof(AnguloArticular)))
        {
            mensaje += $"{angulo}: {angulosCalculados[angulo]:F1}°\n";
        }
        mensaje += $"Pies en el suelo: {(PiesEnSuelo() ? "Sí" : "No")}\n";
        Debug.Log(mensaje);
    }

    private bool EnRango(float angulo, float min, float max)
    {
        return angulo >= min && angulo <= max;
    }

    private bool PiesEnSuelo()
    {
        Vector3 rHeel = ObtenerLandmark("RIGHT_HEEL", out bool rHeelFound);
        Vector3 lHeel = ObtenerLandmark("LEFT_HEEL", out bool lHeelFound);
        Vector3 rFootIndex = ObtenerLandmark("RIGHT_FOOT_INDEX", out bool rFootIndexFound);
        Vector3 lFootIndex = ObtenerLandmark("LEFT_FOOT_INDEX", out bool lFootIndexFound);

        if (!rHeelFound || !lHeelFound || !rFootIndexFound || !lFootIndexFound) return false;

        bool rightFootFlat = Mathf.Abs(rHeel.y - rFootIndex.y) < footFlatToleranceY;
        bool leftFootFlat = Mathf.Abs(lHeel.y - lFootIndex.y) < footFlatToleranceY;

        if (Mathf.Abs(rHeel.y - lHeel.y) > (5 * footFlatToleranceY)) return false;

        return rightFootFlat && leftFootFlat;
    }

    public bool IsPoseCurrentlyDetected(PoseDefinition requiredPose)
    {
        if (requiredPose == null)
        {
            return false;
        }

        bool allAngleRequirementsMet = true;
        if (requiredPose.angle_requirements != null)
        {
            foreach (var angleReq in requiredPose.angle_requirements)
            {
                AnguloArticular targetAngulo;
                if (System.Enum.TryParse(angleReq.angle, out targetAngulo))
                {
                    if (angulosCalculados.TryGetValue(targetAngulo, out float currentAngle))
                    {
                        if (!EnRango(currentAngle, angleReq.min_angle, angleReq.max_angle))
                        {
                            allAngleRequirementsMet = false;
                            break;
                        }
                    }
                    else
                    {
                        allAngleRequirementsMet = false;
                        break;
                    }
                }
                else
                {
                    Debug.LogWarning($"Unknown angle type: {angleReq.angle}. Skipping this requirement.");
                    allAngleRequirementsMet = false;
                    break;
                }
            }
        }

        bool feetOnGroundRequirementMet = true;
        if (requiredPose.require_feet_on_ground)
        {
            feetOnGroundRequirementMet = PiesEnSuelo();
        }

        return allAngleRequirementsMet && feetOnGroundRequirementMet;
    }

    public void Angles()
    {
        RegistrarTodosLosAngulos();
    }
}