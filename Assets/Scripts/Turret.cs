using UnityEngine;

public class Turret : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Shoot1 = Animator.StringToHash("shoot");

    [SerializeField] private Transform head;

    [HideInInspector] public Vector2 rotationVelocity;
    private const float RotationFactor = 1.5f;

    public Transform spawnPoint;

    // Awake is called when object is initialized
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotate(rotationVelocity * RotationFactor, head);
    }

    // Shoot projectile from turret
    public void Shoot()
    {
        // Play shoot animation
        _animator.SetTrigger(Shoot1);
        // Shake the camera
        CameraShake.Instance.ShakeNormal();
    }

    // Rotate based on player's input
    private void Rotate(Vector2 speed, Transform target)
    {
        if (Time.timeScale == 0f) return;

        target.Rotate(0f, speed.x, 0f, Space.World);
        // target.RotateAround(transform.position, Camera.main.transform.up, speed.x);
    }
}
