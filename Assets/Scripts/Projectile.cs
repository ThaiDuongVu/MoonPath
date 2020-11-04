using UnityEngine;

public class Projectile : MonoBehaviour
{
    private CharacterController _characterController;
    private const float Velocity = 20f;

    [HideInInspector] public Vector2 rotationVelocity;
    private const float RotationFactor = 1.5f;

    // Awake is called when object is initialized
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Rotate(rotationVelocity * RotationFactor, transform);
    }

    // Move forward
    private void Move()
    {
        _characterController.Move(transform.up * Velocity * Time.deltaTime);
    }

    // Rotate based on player's input
    private void Rotate(Vector2 speed, Transform target)
    {
        target.Rotate(-speed.y, speed.x, 0f, Space.World);
        transform.localRotation = new Quaternion(transform.localRotation.x, 0f, transform.localRotation.z, transform.localRotation.w);
    }
}
