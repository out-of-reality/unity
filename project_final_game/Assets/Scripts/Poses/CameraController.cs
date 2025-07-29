using UnityEngine;

public class Cae : MonoBehaviour
{
    public GameObject target;
    public float xOffset, yOffset, zOffset;

    void Start()
    {  
    }

    void Update()
    {   
        transform.position = target.transform.position + new Vector3(xOffset, yOffset, zOffset);
        transform.LookAt(target.transform.position);
    }
}
