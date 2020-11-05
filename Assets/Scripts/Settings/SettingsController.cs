using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public Settings fullScreen;
    public Settings resolution;

    public Settings quality;
    public Settings targetFPS;

    public Settings motionBlur;

    // Start is called before the first frame update
    private void Start()
    {
        Apply();
    }

    public void Apply()
    {
        Screen.SetResolution(resolution.currentState, resolution.currentState / 16 * 9,
            (FullScreenMode)fullScreen.currentState);

        QualitySettings.SetQualityLevel(quality.currentState);
        Application.targetFrameRate = targetFPS.currentState;

        GlobalController.Instance.SetMotionBlur(motionBlur.currentState);
    }
}