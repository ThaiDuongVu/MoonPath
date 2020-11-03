using UnityEngine;
using TMPro;

public class Settings : MonoBehaviour
{
    public int[] toggles;
    private int _toggleIndex;

    [HideInInspector] public int currentState;

    public SettingsController settingsController;

    public string propertyName;
    public TMP_Text propertyText;
    public string[] properties;

    // Awake is called when object is initialized
    private void Awake()
    {
        _toggleIndex = PlayerPrefs.GetInt(propertyName, 0);

        currentState = toggles[_toggleIndex];
        propertyText.text = properties[_toggleIndex];
    }

    public void Toggle()
    {
        if (_toggleIndex < toggles.Length - 1)
            _toggleIndex++;
        else
            _toggleIndex = 0;

        currentState = toggles[_toggleIndex];
        propertyText.text = properties[_toggleIndex];

        PlayerPrefs.SetInt(propertyName, _toggleIndex);

        settingsController.Apply();
    }
}