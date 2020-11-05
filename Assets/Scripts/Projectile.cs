using UnityEngine;

public class Projectile : MonoBehaviour
{
    private CharacterController _characterController;
    private const float Velocity = 30f;

    [HideInInspector] public Vector2 rotationVelocity;
    private const float RotationFactor = 2f;

    private GameController _gameController;

    // Awake is called when object is initialized
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Fly();
        Rotate(rotationVelocity * RotationFactor, transform);
    }

    // Move forward
    private void Fly()
    {
        _characterController.Move(transform.up * (Velocity * Time.deltaTime));
    }

    // Rotate based on player's input
    private void Rotate(Vector2 speed, Transform target)
    {
        if (Time.timeScale == 0f) return;

        // target.Rotate(-speed.y, speed.x, 0f, Space.World);
        target.RotateAround(transform.position, Camera.main.transform.up, speed.x);
        target.RotateAround(transform.position, Camera.main.transform.right, -speed.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        _gameController.ChangeFlowState(FlowState.Aiming);
        _gameController.RandomizeAsteroids();
        
        Destroy(gameObject);
    }
}
