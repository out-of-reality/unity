using System.Collections;
using UnityEngine;
using TMPro;

public class BallBalanceGameController : MonoBehaviour
{
    [Header("Gameplay Settings")]
    public float moveSpeed = 1000f;
    public float impulseStrength = 500f;
    public Vector3 impulseDirection = new Vector3(0, 1, 1);

    [Header("References")]
    public Rigidbody rb;
    public LecturaPose lecturaPose;
    public TextMeshProUGUI score_text;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI poseCoinText;

    [Header("UI Canvases")]
    public GameObject victoryCanvas;
    public GameObject defeatCanvas;
    public GameObject coinPopupCanvas;
    public GameObject penaltyMessagePanel;
    public GameObject successMessagePanel;

    private LevelData currentLevelData;
    private PoseDefinition currentCoinChallengePose;

    private float countdownTime;
    private bool isPopupActive = false;

    private int score;
    private int nextCoinIndex = 0;

    private float penaltyMessageDuration = 3f;
    private float successMessageDuration = 3f;

    void Start()
    {
        StartCoroutine(InitializeGame());
    }

    IEnumerator InitializeGame()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        yield return new WaitUntil(() => LevelConfigManager.LoadedLevelData != null);
        currentLevelData = LevelConfigManager.LoadedLevelData;

        yield return new WaitUntil(() => lecturaPose != null);

        score = 0;
        countdownTime = currentLevelData.timing.coin_challenge_time_limit;
        UpdateScoreUI();
    }

    void Update()
    {
        if (currentLevelData == null || lecturaPose == null) return;

        CheckPosition();
        UpdatePopupTimer();
        HandleMovement();
    }
    private void HandleMovement()
    {
        if (isPopupActive) return;

        Vector3 force = Vector3.zero;

        if (lecturaPose.IsPoseCurrentlyDetected(currentLevelData.movement_poses.forward))
        {
            force += Vector3.forward;
        }
        else if (lecturaPose.IsPoseCurrentlyDetected(currentLevelData.movement_poses.backward))
        {
            force += Vector3.back;
        }
        if (lecturaPose.IsPoseCurrentlyDetected(currentLevelData.movement_poses.left))
        {
            force += Vector3.left;
        }
        else if (lecturaPose.IsPoseCurrentlyDetected(currentLevelData.movement_poses.right))
        {
            force += Vector3.right;
        }
        rb.AddForce(force.normalized * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("coin"))
        {
            HandleCoinCollision(other);
        }
        else if (other.CompareTag("goal"))
        {
            victoryCanvas.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("arrow"))
        {
            if (rb != null)
            {
                rb.AddForce(impulseDirection.normalized * impulseStrength, ForceMode.Impulse);
            }
        }
    }
    private void HandleCoinCollision(Collider coin)
    {
    currentCoinChallengePose = GetPoseForCoin(nextCoinIndex);

    if (currentCoinChallengePose == null)
    {
        Debug.LogError($"Pose no definida para la moneda {nextCoinIndex}.");
        IncrementScore();
        Destroy(coin.gameObject);
        return;
    }

    countdownTime = currentLevelData.timing.coin_challenge_time_limit;
    poseCoinText.text = "Realiza la pose: " + currentCoinChallengePose.name;
    Time.timeScale = 0f;
    coinPopupCanvas.SetActive(true);
    isPopupActive = true;

    Destroy(coin.gameObject);
    }

    private PoseDefinition GetPoseForCoin(int index)
    {
        int i = 0;
        foreach (var kvp in currentLevelData.coin_poses_dict)
        {
            if (i == index)
                return kvp.Value;
            i++;
        }
        return null;
    }

    public void CloseCoinPopup()
    {
        coinPopupCanvas.SetActive(false);
        isPopupActive = false;
        IncrementScore();
        ShowSuccessMessage();
        nextCoinIndex++;
        currentCoinChallengePose = null;
    }

    private void UpdatePopupTimer()
    {
        if (!isPopupActive) return;

        countdownTime -= Time.unscaledDeltaTime;
        timerText.text = "Tiempo restante: " + Mathf.Ceil(countdownTime).ToString();

        if (lecturaPose.IsPoseCurrentlyDetected(currentCoinChallengePose))
        {
            CloseCoinPopup();
            return;
        }

        if (countdownTime <= 0)
        {
            ShowPenaltyMessage();
            coinPopupCanvas.SetActive(false);
            isPopupActive = false;
            currentCoinChallengePose = null;
        }
    }

    private void IncrementScore()
    {
        score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        score_text.text = score.ToString();
    }

    private void ShowPenaltyMessage()
    {
        penaltyMessagePanel.SetActive(true);
        StartCoroutine(HidePanelAfterDelay(penaltyMessagePanel, penaltyMessageDuration));
    }

    private void ShowSuccessMessage()
    {
        successMessagePanel.SetActive(true);
        StartCoroutine(HidePanelAfterDelay(successMessagePanel, successMessageDuration));
    }

    private IEnumerator HidePanelAfterDelay(GameObject panel, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void CheckPosition()
    {
        if (transform.position.y <= -115)
        {
            defeatCanvas.SetActive(true);
        }
    }
}
