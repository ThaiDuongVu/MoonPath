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

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private TMP_Text feedbackText;

    [SerializeField] private Transform title;

    // Start is called before the first frame update
    private void Start()
    {
        HideFPS();
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

    // Update score text
    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    // Update a text on screen with a refresh rate to stop screen from updating every frame
    private void UpdateText(TMP_Text text, string message, float refreshRate)
    {
        if (!(Time.unscaledTime > _timer)) return;

        text.text = message;
        _timer = Time.unscaledTime + refreshRate;
    }

    // Pop up a feedback text
    public void Feedback(string feedback)
    {
        feedbackText.text = feedback;
        feedbackText.gameObject.SetActive(true);
    }

    // Disable or enable main menu title
    public void SetTitleActive(bool value)
    {
        title.gameObject.SetActive(value);
    }
}