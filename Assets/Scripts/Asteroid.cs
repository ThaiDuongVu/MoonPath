using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const float InterpolationRatio = 0.01f;
    private const float FloatingRange = 5f;

    private const float RotateSpeed = 0.5f;

    // Update is called once per frame
    private void Update()
    {
        Float();
        Rotate();
    }

    public void Randomize()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f));
    }

    private void Float()
    {
        if (Time.timeScale == 0f) return;

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + Random.Range(-FloatingRange, FloatingRange), transform.position.y + Random.Range(-FloatingRange, FloatingRange), transform.position.z + Random.Range(-FloatingRange, FloatingRange)), InterpolationRatio);
    }

    private void Rotate()
    {
        transform.Rotate(0f, RotateSpeed, 0f, Space.Self);
    }
}
