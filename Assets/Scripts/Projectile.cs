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
    [SerializeField] private ParticleSystem _coinExplosion;
    private bool _isCollided;

    [SerializeField] private GameObject _rig;
    [SerializeField] private GameObject _trail;

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
        // If game paused the stop rotating
        if (Time.timeScale == 0f) return;

        Vector3 position = transform.position;

        target.RotateAround(position, _camera.transform.up, speed.x);
        target.RotateAround(position, _camera.transform.right, -speed.y);
    }

    // Disconnect projectile's motion from camera
    private void Disconnect()
    {
        _mainCamera.followTarget = null;
        _mainCamera.StopBoostBrakeAnimation();
    }

    // Projectile arrive at destination
    private IEnumerator OnArrived()
    {
        // Shake camera
        CameraShake.Instance.ShakeNormal();

        // Explode and disable character rig
        Instantiate(_explosion, transform.position, _explosion.transform.rotation);
        _rig.SetActive(false);
        _trail.SetActive(false);

        yield return new WaitForSeconds(0.25f);

        // Return to original position
        _gameController.ChangeFlowState(FlowState.Returning);
        // Randomize asteroids and planets
        _gameController.Randomize();

        // Destroy this projectile
        Destroy(gameObject);
    }

    // When projectile fly too far away from objective
    private IEnumerator Lost()
    {
        Disconnect();

        yield return new WaitForSeconds(0.45f);

        UIController.Instance.Feedback("Lost in the void");
        StartCoroutine(OnArrived());
    }

    #region Trigger Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moon") || other.CompareTag("BadPlanet"))
        {
            Disconnect();
            destination = other.transform;
        }
        else if (other.CompareTag("Coin"))
        {
            if (other.GetType().Equals(typeof(SphereCollider)))
            {
                other.GetComponent<Coin>().attractTarget = transform;
            }
            else
            {
                CameraShake.Instance.ShakeLight();
                _gameController.EarnCoin(1);

                Instantiate(_coinExplosion, transform.position, _coinExplosion.transform.rotation);
                Destroy(other.gameObject);

                UIController.Instance.Feedback("Nice!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Border"))
        {
            StartCoroutine(Lost());
        }
    }

    #endregion

    #region Collision Methods

    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        // If projectile already collided then stop
        if (_isCollided) return;

        if (other.transform.CompareTag("Moon"))
        {
            CameraShake.Instance.ShakeHard();
            UIController.Instance.Feedback("Successfully boarded");

            _gameController.Board(1);
        }
        else if (other.transform.CompareTag("BadPlanet") || other.transform.CompareTag("Asteroid") || other.transform.CompareTag("Earth"))
        {
            CameraShake.Instance.ShakeHard();
            UIController.Instance.Feedback("Fatal crash");

            _gameController.Crash(1);

        }
        _isCollided = true;
        StartCoroutine(OnArrived());
    }

    #endregion
}
