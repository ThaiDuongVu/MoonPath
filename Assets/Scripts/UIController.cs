using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible
    #region Singleton

    private static UIController _instance;

    public static UIController Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<UIController>();

            return _instance;
        }
    }

    #endregion

    [SerializeField] private TMP_Text fpsText;
    private float _timer;

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text boardText;

    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text summaryText;

    [SerializeField] private TMP_Text feedbackText;

    [SerializeField] private GameObject stars;

    [SerializeField] private Transform title;

    // Start is called before the first frame update
    private void Start()
    {
        HideFPS();
        if (stars != null) stars.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        DisplayFPS();
    }

    #region FPS Methods

    // Show game framerate
    private void DisplayFPS()
    {
        UpdateText(fpsText, ((int)(1f / Time.unscaledDeltaTime)).ToString(), 1f);
    }

    // Show fps text
    public void ShowFPS()
    {
        fpsText.gameObject.SetActive(true);
    }

    // Hide fps text
    public void HideFPS()
    {
        fpsText.gameObject.SetActive(false);
    }

    #endregion

    #region Update Texts

    // Update a text on screen with a refresh rate to stop screen from updating every frame
    private void UpdateText(TMP_Text text, string message, float refreshRate)
    {
        if (!(Time.unscaledTime > _timer)) return;

        text.text = message;
        _timer = Time.unscaledTime + refreshRate;
    }

    // Update coin text
    public void UpdateCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }

    // Update board text
    public void UpdateBoardText(int board)
    {
        boardText.text = board.ToString();
    }

    // Update countdown text
    public void UpdateCountdownText(float timer)
    {
        countdownText.text = ((int)timer).ToString();
    }

    // Update summary text
    public void UpdateSummaryText(int peopleBoarded, int takeOffs, int coinsEarned, int fatalities)
    {
        feedbackText.gameObject.SetActive(false);

        summaryText.text = "People boarded - " + peopleBoarded + "\n";
        summaryText.text += "Take offs - " + takeOffs + "\n";
        summaryText.text += "Coins earned - " + coinsEarned + "\n";
        summaryText.text += "Fatalities - " + fatalities + "\n";
    }

    // Pop up a feedback text
    public void Feedback(string feedback)
    {
        feedbackText.text = feedback;
        feedbackText.gameObject.SetActive(true);
    }

    #endregion

    // Set stars active by rank
    public void UpdateRank(int rank)
    {
        stars.SetActive(true);

        int i = 0;
        foreach (Transform child in stars.transform)
        {
            i++;
            if (i > rank)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // Disable or enable main menu title
    public void SetTitleActive(bool value)
    {
        title.gameObject.SetActive(value);
    }
}