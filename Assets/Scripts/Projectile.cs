using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    private CharacterController _characterController;

    private const float NormalVelocity = 30f;
    private const float BoostVelocity = 40f;
    private const float BrakeVelocity = 20f;
    private float _currentVelocity;
    private float _targetVelocity;

    private const float Acceleration = 40f;
    private const float Deceleration = 20f;

    [HideInInspector] public Vector2 rotationVelocity;
    private const float RotationFactor = 2f;

    private Animator _animator;

    private GameController _gameController;

    private InputManager _inputManager;

    private void OnEnable()
    {
        _inputManager = new InputManager();

        _inputManager.Player.Boost.performed += BoostOnPerformed;
        _inputManager.Player.Brake.performed += BrakeOnPerformed;

        _inputManager.Player.Boost.canceled += BoostBrakeOnCanceled;
        _inputManager.Player.Brake.canceled += BoostBrakeOnCanceled;

        _inputManager.Enable();
    }

    #region Input Methods

    private void BoostOnPerformed(InputAction.CallbackContext context)
    {
        _targetVelocity = BoostVelocity;
        _animator.SetBool("isBoosting", true);
    }

    private void BrakeOnPerformed(InputAction.CallbackContext context)
    {
        _targetVelocity = BrakeVelocity;
        _animator.SetBool("isBraking", true);
    }

    private void BoostBrakeOnCanceled(InputAction.CallbackContext context)
    {
        _targetVelocity = NormalVelocity;

        _animator.SetBool("isBoosting", false);
        _animator.SetBool("isBraking", false);
    }

    #endregion

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    // Awake is called when object is initialized
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _gameController = FindObjectOfType<GameController>();

        _animator = GetComponent<Animator>();

        _targetVelocity = NormalVelocity;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Fly();
        Rotate(rotationVelocity * RotationFactor, transform);

        Accelerate(_targetVelocity);
    }

    // Move forward
    private void Fly()
    {
        _characterController.Move(transform.up * (_currentVelocity * Time.deltaTime));
    }

    // Accelerate or decelerate to target velocity
    private void Accelerate(float velocity)
    {
        if (_currentVelocity < velocity)
        {
            _currentVelocity += Acceleration * Time.deltaTime;
        }
        else if (_currentVelocity > velocity)
        {
            _currentVelocity -= Deceleration * Time.deltaTime;
        }
    }

    // Rotate based on player's input
    private void Rotate(Vector2 speed, Transform target)
    {
        if (Time.timeScale == 0f) return;

        // target.Rotate(-speed.y, speed.x, 0f, Space.World);
        target.RotateAround(transform.position, Camera.main.transform.up, speed.x);
        target.RotateAround(transform.position, Camera.main.transform.right, -speed.y);
    }

    #region Trigger Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moon"))
        {
            _gameController.ChangeFlowState(FlowState.Aiming);
            _gameController.RandomizeAsteroids();

            Destroy(gameObject);
        }
        else if (other.CompareTag("Asteroid"))
        {
            CameraShake.Instance.ShakeNormal();
        }
    }

    #endregion
}
