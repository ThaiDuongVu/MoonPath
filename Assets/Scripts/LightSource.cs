using UnityEngine;

public class LightSource : MonoBehaviour
{
    private const float RotatingSpeed = 2f;

    // Update is called once per frame
    private void Update()
    {
        Rotate();
    }

    // Rotate the light source to give illusion of the sun orbiting
    private void Rotate()
    {
        transform.Rotate( new Vector3(0f, RotatingSpeed * Time.deltaTime, 0f), Space.World);
    }
}
