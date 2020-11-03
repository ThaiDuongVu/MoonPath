using UnityEngine;

public class LightSource : MonoBehaviour
{
    private float _rotatingSpeed = 2f;

    // Awake is called when object is initialized
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Rotate();
    }

    // Rotate the light source to give illusion of the sun orbiting
    private void Rotate()
    {
        transform.Rotate( new Vector3(0f, _rotatingSpeed * Time.deltaTime, 0f), Space.World);
    }
}
