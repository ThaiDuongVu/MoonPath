using UnityEngine;

public class Coin : Asteroid
{
    private const float RotateSpeed = 5f;

    [HideInInspector] public Transform attractTarget;
    private const float InterpolationRatio = 0.5f;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotate(RotateSpeed);
        Attract(attractTarget);

        if (isRandomizing)
        {
            Randomize();
        }
    }

    private void Attract(Transform target)
    {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position, InterpolationRatio);
    }
}
