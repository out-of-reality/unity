using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseController : MonoBehaviour
{
    private GameObject canvasOptions;

    public GameObject ObjetoMenuPausa;
    public bool Pausa = false;
    public GameObject MenuSalir;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("options");
        if (obj != null)
        {
            Transform canvasTransform = obj.transform.Find("CanvasOptions");
            if (canvasTransform != null)
            {
                canvasOptions = canvasTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("No se encontro CanvasOptions como hijo de ObjectsBetweenScenes.");
            }
        }
        else
        {
            Debug.LogError("No se encontro ningun GameObject con el tag 'options'.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasOptions == null || !canvasOptions.activeSelf)
            {
                if (!Pausa)
                {
                    ActivarPausa();
                }
                else
                {
                    Resumir();
                }
            }
        }
    }

    private void ActivarPausa()
    {
        ObjetoMenuPausa.SetActive(true);
        Pausa = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        AudioSource[] sonidos = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in sonidos)
        {
            audio.Pause();
        }
    }

    public void Resumir()
    {
        ObjetoMenuPausa.SetActive(false);
        MenuSalir.SetActive(false);
        Pausa = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        AudioSource[] sonidos = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in sonidos)
        {
            audio.Play();
        }
    }

    public void IrAlMenu(string NombreMenu)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(NombreMenu);

        AudioSource[] sonidos = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in sonidos)
        {
            audio.Play();
        }
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}
