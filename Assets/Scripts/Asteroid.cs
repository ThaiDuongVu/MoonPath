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
        transform.position = new Vector3(Random.Range(-75f, 75f), Random.Range(-25f, 100f), Random.Range(50f, 200f));
    }

    private void Float()
    {
        if (Time.timeScale == 0f) return;

        Vector3 position = transform.position;
        position = Vector3.Lerp(position, new Vector3(position.x + Random.Range(-FloatingRange, FloatingRange), position.y + Random.Range(-FloatingRange, FloatingRange), position.z + Random.Range(-FloatingRange, FloatingRange)), InterpolationRatio);

        transform.position = position;
    }

    private void Rotate()
    {
        transform.Rotate(0f, RotateSpeed, 0f, Space.Self);
    }
}
