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

    // Show game framerate
    private void DisplayFPS()
    {
        UpdateText(fpsText, ((int) (1f / Time.unscaledDeltaTime)).ToString(), 1);
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

    // Update a text on screen with a refresh rate to stop screen from updating every frame
    private void UpdateText(TMP_Text text, string message, float refreshRate)
    {
        if (!(Time.unscaledTime > _timer)) return;

        text.text = message;
        _timer = Time.unscaledTime + refreshRate;
    }
}