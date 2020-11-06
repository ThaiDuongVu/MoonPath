using UnityEngine;

public class Planet : MonoBehaviour
{
    private const float RotateSpeed = 0.5f;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(0f, RotateSpeed, 0f, Space.Self);
        // transform.RotateAround(transform.position, Camera.main.transform.up, RotateSpeed);
    }
}
