using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const float InterpolationRatio = 0.01f;
    private const float FloatingRange = 7.5f;

    private const float RotateSpeed = 2.5f;

    private bool isRandomizing;
    private Vector3 lerpPosition;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotate(RotateSpeed);

        if (isRandomizing)
        {
            Randomize();
        }
        else
        {
            Float();
        }
    }

    public void StartRandomize(Vector3 lerp)
    {
        isRandomizing = true;
        lerpPosition = lerp;
    }

    // Move to a random position in range
    public void Randomize()
    {
        // transform.position = new Vector3(Random.Range(-75f, 75f), Random.Range(-25f, 100f), Random.Range(50f, 200f));
        transform.position = Vector3.Lerp(transform.position, lerpPosition, 0.05f);

        if (Mathf.Abs(transform.position.x - lerpPosition.x) < 0.1f)
        {
            isRandomizing = false;
        }
    }

    // Float around randomly
    private void Float()
    {
        // If game paused then stop floating
        if (Time.timeScale == 0f) return;

        Vector3 position = transform.position;
        position = Vector3.Lerp(position, new Vector3(position.x + Random.Range(-FloatingRange, FloatingRange), position.y + Random.Range(-FloatingRange, FloatingRange), position.z + Random.Range(-FloatingRange, FloatingRange)), InterpolationRatio);

        transform.position = position;
    }

    // Rotate around local axis
    protected void Rotate(float speed)
    {
        transform.Rotate(0f, speed, 0f, Space.World);
    }
}
