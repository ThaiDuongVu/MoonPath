using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GlobalController : MonoBehaviour
{
    #region Singleton

    private static GlobalController _instance;

    public static GlobalController Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<GlobalController>();

            return _instance;
        }
    }

    #endregion

    [SerializeField] private PostProcessProfile postProcessProfile;
    private DepthOfField _depthOfField;

    // Awake is called when an object is initialized
    private void Awake()
    {
        postProcessProfile.TryGetSettings(out _depthOfField);
    }

    // Start is called before the first frame update
    private void Start()
    {
        DisableDepthOfField();
    }

    // For logical purposes only
    // Check whether two numbers are close enough to each other
    public static bool CloseTo(float x, float y, float epsilon)
    {
        return Mathf.Abs(x - y) < epsilon;
    }

    // Enable the depth of field effects
    public void EnableDepthOfField()
    {
        _depthOfField.enabled.value = true;
    }

    // Disable the depth of field effects
    public void DisableDepthOfField()
    {
        _depthOfField.enabled.value = false;
    }
}