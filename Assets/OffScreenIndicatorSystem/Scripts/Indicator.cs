using UnityEngine;
using UnityEngine.UI;

// Assign this script to the indicator prefabs.
public class Indicator : MonoBehaviour
{
    [SerializeField] private IndicatorType indicatorType;
    private Image _indicatorImage;
    private Text _distanceText;

    // Gets if the game object is active in hierarchy.
    public bool Active => transform.gameObject.activeInHierarchy;

    // Gets the indicator type
    public IndicatorType Type => indicatorType;

    private void Awake()
    {
        _indicatorImage = transform.GetComponent<Image>();
        _distanceText = transform.GetComponentInChildren<Text>();
    }

    // Sets the image color for the indicator.
    public void SetImageColor(Color color)
    {
        _indicatorImage.color = color;
    }

    // Sets the distance text for the indicator.
    public void SetDistanceText(float value)
    {
        _distanceText.text = value >= 0 ? Mathf.Floor(value) + " m" : "";
    }

    // Sets the distance text rotation of the indicator.
    public void SetTextRotation(Quaternion rotation)
    {
        _distanceText.rectTransform.rotation = rotation;
    }

    // Sets the indicator as active or inactive.
    public void Activate(bool value)
    {
        transform.gameObject.SetActive(value);
    }
}