using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Sprite _crosshairNormal;
    [SerializeField] private Sprite _crosshairRaycast;

    private Image _image;

    private Camera _camera;

    public Color32 normalColor;
    public Color32 raycastColor;
    
    private RaycastHit hit;

    // Awake is called when object is initialized
    private void Awake()
    {
        _image = GetComponent<Image>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        PerformRaycast();
    }

    private void PerformRaycast()
    {

        // If raycast from camera is "seeing" the moon then change crosshair sprite and color
        if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if (hit.transform.CompareTag("Moon"))
            {
                _image.color = raycastColor;
                _image.sprite = _crosshairRaycast;
            }
        }
        // Else reset sprite and color
        else
        {
            _image.color = normalColor;
            _image.sprite = _crosshairNormal;
        }
    }
}
