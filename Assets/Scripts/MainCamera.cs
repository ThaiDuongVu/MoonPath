using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform followTarget;
    public Transform rotateTarget;

    private const float InterpolationRatio = 0.0625f;


    // Update is called once per frame
    private void Update()
    {
        Follow(followTarget);
        Rotate(rotateTarget);
    }

    private void Follow(Transform target)
    {
        if (target == null || Time.deltaTime == 0f) return;

        Vector3 targetPosition = target.position;
        Vector3 lerpPosition = new Vector3(targetPosition.x, targetPosition.y + 1f, targetPosition.z);

        transform.position = Vector3.Lerp(transform.position, lerpPosition, InterpolationRatio);
    }

    private void Rotate(Transform target)
    {
        if (target == null || Time.deltaTime == 0f) return;

        transform.forward = Vector3.Lerp(transform.forward, target.up, InterpolationRatio);
    }
}
