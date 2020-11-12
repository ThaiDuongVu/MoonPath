using UnityEngine;
using System.Collections;
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

    private Transform destination;

    private Animator _animator;
    private static readonly int IsBoosting = Animator.StringToHash("isBoosting");
    private static readonly int IsBraking = Animator.StringToHash("isBraking");

    private GameController _gameController;
    [SerializeField] private ParticleSystem _explosion;

    private InputManager _inputManager;
    private Camera _camera;
    private MainCamera _mainCamera;

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle boost & brake input
        _inputManager.Player.Boost.performed += BoostOnPerformed;
        _inputManager.Player.Brake.performed += BrakeOnPerformed;

        _inputManager.Player.Boost.canceled += BoostBrakeOnCanceled;
        _inputManager.Player.Brake.canceled += BoostBrakeOnCanceled;

        _inputManager.Enable();
    }

    #region Input Methods

    private void BoostOnPerformed(InputAction.CallbackContext context)
    {
        BoostBrakeOnCanceled(context);

        _targetVelocity = BoostVelocity;
        _animator.SetBool(IsBoosting, true);
    }

    private void BrakeOnPerformed(InputAction.CallbackContext context)
    {
        BoostBrakeOnCanceled(context);

        _targetVelocity = BrakeVelocity;
        _animator.SetBool(IsBraking, true);
    }

    private void BoostBrakeOnCanceled(InputAction.CallbackContext context)
    {
        _targetVelocity = NormalVelocity;
        _animator.SetBool(IsBoosting, false);
        _animator.SetBool(IsBraking, false);
    }

    #endregion

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    // Awake is called when object is initialized
    private void Awake()
    {
        _camera = Camera.main;
        _mainCamera = _camera.GetComponent<MainCamera>();

        _characterController = GetComponent<CharacterController>();
        _gameController = FindObjectOfType<GameController>();

        _animator = GetComponent<Animator>();

        _targetVelocity = NormalVelocity;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Fly();
        Accelerate(_targetVelocity);

        if (destination)
        {
            transform.up = Vector3.Lerp(transform.up, (destination.transform.position - transform.position).normalized, 0.1f);
        }
        else
        {
            Rotate(rotationVelocity * RotationFactor, transform);
        }
    }

    #region Movement Methods

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

    #endregion

    // Rotate based on player's input
    private void Rotate(Vector2 speed, Transform target)
    {
        if (Time.timeScale == 0f) return;

        // target.Rotate(-speed.y, speed.x, 0f, Space.World);
        Vector3 position = transform.position;
        target.RotateAround(position, _camera.transform.up, speed.x);
        target.RotateAround(position, _camera.transform.right, -speed.y);
    }

    #region Trigger Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moon"))
        {
            _mainCamera.followTarget = null;
            destination = other.transform;
        }
        else if (other.CompareTag("Asteroid"))
        {
            CameraShake.Instance.ShakeNormal();
        }
    }

    #endregion

    #region Collision Methods

    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.transform.CompareTag("Moon"))
        {
            _gameController.ChangeFlowState(FlowState.Returning);
            _gameController.RandomizeAsteroids();

            Instantiate(_explosion, transform.position, _explosion.transform.rotation);

            Destroy(gameObject);
        }
    }

    #endregion
}
