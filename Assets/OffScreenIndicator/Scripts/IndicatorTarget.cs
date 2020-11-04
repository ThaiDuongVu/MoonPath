using UnityEngine;

// Attach this script to all the target game objects in the scene.
[DefaultExecutionOrder(0)]
public class IndicatorTarget : MonoBehaviour
{
    [Tooltip("Change this color to change the indicators color for this target")] [SerializeField]
    private Color targetColor = Color.red;

    [Tooltip("Select if box indicator is required for this target")] [SerializeField]
    private bool needBoxIndicator = true;

    [Tooltip("Select if arrow indicator is required for this target")] [SerializeField]
    private bool needArrowIndicator = true;

    [Tooltip("Select if distance text is required for this target")] [SerializeField]
    private bool needDistanceText = true;

    // Please do not assign its value yourself without understanding its use.
    // A reference to the target's indicator, 
    // its value is assigned at runtime by the offscreen indicator script.
    [HideInInspector] public Indicator indicator;

    // Gets the color for the target indicator.
    public Color TargetColor => targetColor;

    // Gets if box indicator is required for the target.
    public bool NeedBoxIndicator => needBoxIndicator;

    // Gets if arrow indicator is required for the target.
    public bool NeedArrowIndicator => needArrowIndicator;

    // Gets if the distance text is required for the target.
    public bool NeedDistanceText => needDistanceText;

    // On enable add this target object to the targets list.
    private void OnEnable()
    {
        if (OffScreenIndicator.TargetStateChanged != null) OffScreenIndicator.TargetStateChanged.Invoke(this, true);
    }

    // On disable remove this target object from the targets list.
    private void OnDisable()
    {
        if (OffScreenIndicator.TargetStateChanged != null) OffScreenIndicator.TargetStateChanged.Invoke(this, false);
    }

    // Gets the distance between the camera and the target.
    public float GetDistanceFromCamera(Vector3 cameraPosition)
    {
        float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
        return distanceFromCamera;
    }
}