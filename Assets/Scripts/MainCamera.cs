using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    public Transform followTarget;
    public Transform rotateTarget;

    private const float InterpolationRatio = 0.075f;

    [SerializeField] private GameController gameController;

    private Animator _animator;
    private static readonly int IsBoosting = Animator.StringToHash("isBoosting");
    private static readonly int IsBraking = Animator.StringToHash("isBraking");

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
        if (gameController is null || Time.deltaTime == 0f) return;

        _animator.SetBool(IsBoosting, true);
    }

    private void BrakeOnPerformed(InputAction.CallbackContext context)
    {
        if (gameController is null || Time.deltaTime == 0f) return;

        _animator.SetBool(IsBraking, true);
    }

    private void BoostBrakeOnCanceled(InputAction.CallbackContext context)
    {
        if (gameController is null || Time.deltaTime == 0f) return;

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
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Follow(followTarget);
        Rotate(rotateTarget);
    }

    private void Follow(Transform target)
    {
        if (target is null || Time.deltaTime == 0f) return;

        Vector3 targetPosition = target.position;
        Vector3 lerpPosition = new Vector3(targetPosition.x, targetPosition.y + 1f, targetPosition.z);

        transform.position = Vector3.Lerp(transform.position, lerpPosition, InterpolationRatio);
    }

    private void Rotate(Transform target)
    {
        if (target is null || Time.deltaTime == 0f) return;

        transform.forward = Vector3.Lerp(transform.forward, target.up, InterpolationRatio);
    }
}
