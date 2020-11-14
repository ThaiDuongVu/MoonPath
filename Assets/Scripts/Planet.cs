using UnityEngine;

public class Planet : MonoBehaviour
{
    private const float RotateSpeed = 0.5f;

    private bool isRandomizing;
    private Vector3 lerpPosition;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotate();

        if (isRandomizing)
        {
            Randomize();
        }
    }

    private void Rotate()
    {
        transform.Rotate(0f, RotateSpeed, 0f, Space.Self);
    }

    public void StartRandomize(Vector3 lerp)
    {
        isRandomizing = true;
        lerpPosition = lerp;
    }

    public void Randomize()
    {
        // transform.position = new Vector3(Random.Range(-75f, 75f), Random.Range(-25f, 100f), Random.Range(50f, 200f));
        transform.position = Vector3.Lerp(transform.position, lerpPosition, 0.05f);

        if (Mathf.Abs(transform.position.x - lerpPosition.x) < 0.1f)
        {
            isRandomizing = false;
        }
    }
}
