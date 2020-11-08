using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private PostProcessProfile postProcessProfile;
    private MotionBlur _motionBlur;

    public Settings fullScreen;
    public Settings resolution;

    public Settings quality;
    public Settings targetFPS;

    public Settings motionBlur;

    // Awake is called when an object is initialized
    private void Awake()
    {
        postProcessProfile.TryGetSettings(out _motionBlur);
    }

    // Start is called before the first frame update
    private void Start()
    {
        Apply();
    }

    public void SetMotionBlur(int value)
    {
        _motionBlur.enabled.value = value == 0;
    }

    public void Apply()
    {
        Screen.SetResolution(resolution.currentState, resolution.currentState / 16 * 9,
            (FullScreenMode)fullScreen.currentState);

        QualitySettings.SetQualityLevel(quality.currentState);
        Application.targetFrameRate = targetFPS.currentState;

        SetMotionBlur(motionBlur.currentState);
    }
}