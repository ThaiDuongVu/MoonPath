using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Use the singleton pattern to make the class globally accessible

    #region Singleton

    private static CameraShake _instance;

    public static CameraShake Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<CameraShake>();

            return _instance;
        }
    }

    #endregion

    // How long to shake the camera
    private float _shakeDuration;

    // How hard to shake the camera
    private float _shakeIntensity;

    // How steep should the shake decrease
    private float _decreaseFactor;

    private Vector3 _originalPosition;

    // Start is called before the first frame update
    private void Start()
    {
        _originalPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        Randomize();
    }

    // When camera shakes, randomize its position by shake intensity
    private void Randomize()
    {
        // While shake duration is greater than 0
        if (_shakeDuration > 0)
        {
            // Randomize position
            transform.localPosition = _originalPosition + Random.insideUnitSphere * _shakeIntensity;

            // Decrease shake duration
            _shakeDuration -= Time.deltaTime * _decreaseFactor;
        }
        // When shake duration reaches 0
        else
        {
            // Reset everything
            _shakeDuration = 0f;
            transform.localPosition = _originalPosition;
        }
    }

    #region Shake Methods

    // Shake the camera at different intensities and duration

    public void ShakeMicro()
    {
        _shakeDuration = 0.1f;
        _shakeIntensity = 0.15f;
        _decreaseFactor = 2f;
    }

    public void ShakeLight()
    {
        _shakeDuration = 0.2f;
        _shakeIntensity = 0.25f;
        _decreaseFactor = 2f;
    }

    public void ShakeNormal()
    {
        _shakeDuration = 0.3f;
        _shakeIntensity = 0.35f;
        _decreaseFactor = 2f;
    }

    public void ShakeHard()
    {
        _shakeDuration = 0.4f;
        _shakeIntensity = 0.45f;
        _decreaseFactor = 2f;
    }

    #endregion
}