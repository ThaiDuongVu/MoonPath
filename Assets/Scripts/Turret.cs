using UnityEngine;
using UnityEngine.InputSystem;

public class Turret : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private Transform head;

    [HideInInspector] public Vector2 rotationVelocity;
    private const float RotationFactor = 1.5f;

    public Transform spawnPoint;

    // Awake is called when object is initialized
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (rotationVelocity != Vector2.zero)
        {
            Rotate(rotationVelocity * RotationFactor, head);
        }
    }

    // Shoot projectile from turret
    public void Shoot()
    {
        // Play shoot animation
        _animator.SetTrigger("shoot");
        // Shake the camera
        CameraShake.Instance.ShakeLight();
    }

    // Rotate based on player's input
    private void Rotate(Vector2 speed, Transform target)
    {
        target.Rotate(0f, speed.x, 0f, Space.World);
    }
}
