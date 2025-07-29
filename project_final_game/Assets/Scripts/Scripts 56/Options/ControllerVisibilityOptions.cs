using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerVisibilityOptions : MonoBehaviour
{
    private GameObject canvasOptions;

    void Start()
    {
        // Encontrar el objeto con el tag "options"
        GameObject objBetweenScenes = GameObject.FindGameObjectWithTag("options");

        if (objBetweenScenes != null)
        {
            // Buscar el hijo llamado "CanvasOptions"
            Transform canvasTransform = objBetweenScenes.transform.Find("CanvasOptions");

            if (canvasTransform != null)
            {
                canvasOptions = canvasTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("No se encontro CanvasOptions dentro de ObjectsBetweenScenes.");
            }
        }
        else
        {
            Debug.LogError("No se encontro el GameObject con el tag 'options'.");
        }   
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     MostrarOpciones();
        // }
    }

    public void MostrarOpciones()
    {
        if (canvasOptions != null)
        {
            canvasOptions.SetActive(true);
        }
    }
}
