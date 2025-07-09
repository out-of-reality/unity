using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startCanvas;
    public GameObject pauseMenuCanvas;

    [Header("Level Configuration")]
    public LevelConfigManager levelConfigManager;

    [Header("Network Controler")]
    public UdpClientScript UdpClient;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);
        StartGame();
        StartCoroutine(InitializeUdp());
        StartCoroutine(InitializeLevelConfig());
    }

    private IEnumerator InitializeUdp()
    {
        if (UdpClient != null)
        {
            UdpClient.enabled = true;
            UdpClient.gameObject.SetActive(true);
            float elapsed = 0f;
            while (elapsed < 5f && (!UdpClient.isActiveAndEnabled))
            {
                yield return null;
                elapsed += Time.unscaledDeltaTime;
            }

            UdpClient.StartRecording();
        }
        else
        {
            Debug.LogWarning("UdpClient no asignado en GameManager.");
        }
    }
    private IEnumerator InitializeLevelConfig()
    {
        if (levelConfigManager == null)
        {
            Debug.LogError("LevelConfigManager no asignado.");
        }

        try
        {
            //levelConfigManager.GetFirstActiveLevelConfig();
            string tokenUser = PlayerPrefs.GetString("ACCESS_TOKEN");
            levelConfigManager.GetLevelConfigForPatient(1);
            Debug.Log("Configuración del nivel cargada correctamente.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al cargar configuración del nivel: {e.Message}");
        }
        SetupStartCanvas();
        yield break;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                UdpClient?.ResumeRecording();
                Resume();
            }
            else
            {
                UdpClient?.PauseRecording();
                Pause();
            }
        }
    }

    private void SetupStartCanvas()
    {
        if (startCanvas != null)
            startCanvas.SetActive(true);
    }
    private void StartGame()
    {
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        pauseMenuCanvas?.SetActive(false);
        StartGame();
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuCanvas?.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RestartGame()
    {
        StartGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        UdpClient?.StopRecording();
        StartGame();
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGame()
    {
        UdpClient?.StopRecording();
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
