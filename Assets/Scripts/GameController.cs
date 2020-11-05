using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private GameState _gameState;

    [SerializeField] private Menu[] menus;

    private FlowState _flowState;
    [SerializeField] private Turret turret;

    public MainCamera mainCamera;
    public Transform cameraPoint;
    public Transform spawnPoint;

    public List<Projectile> projectiles = new List<Projectile>();
    private Projectile _currentProjectile;

    public List<Asteroid> asteroidPrefabs = new List<Asteroid>();
    private List<Asteroid> asteroids = new List<Asteroid>();

    private InputManager _inputManager;

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle game pause input
        _inputManager.Game.Escape.performed += EscapeOnPerformed;

        _inputManager.Player.Shoot.performed += ShootOnPerformed;

        _inputManager.Player.Aim.performed += AimOnPerformed;
        _inputManager.Player.Aim.canceled += AimOnCanceled;

        _inputManager.Enable();
    }

    #region Input Methods

    private void EscapeOnPerformed(InputAction.CallbackContext context)
    {
        switch (_gameState)
        {
            case GameState.Started:
                Pause();
                break;
            case GameState.Paused:
                Resume();
                break;
            case GameState.NotStarted:
                break;
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ShootOnPerformed(InputAction.CallbackContext context)
    {
        if (_flowState != FlowState.Aiming) return;

        turret.Shoot();

        _currentProjectile = Instantiate(projectiles[UnityEngine.Random.Range(0, projectiles.Count - 1)], turret.spawnPoint.position, turret.spawnPoint.rotation).GetComponent<Projectile>();

        Transform currentProjectileTransform = _currentProjectile.transform;
        mainCamera.followTarget = currentProjectileTransform;
        mainCamera.rotateTarget = currentProjectileTransform;

        ChangeFlowState(FlowState.Flying);
    }

    private void AimOnPerformed(InputAction.CallbackContext context)
    {
        switch (_flowState)
        {
            case FlowState.Aiming:
                turret.rotationVelocity = context.ReadValue<Vector2>();
                break;
            case FlowState.Flying:
                _currentProjectile.rotationVelocity = context.ReadValue<Vector2>();
                break;
            case FlowState.Returning:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AimOnCanceled(InputAction.CallbackContext context)
    {
        switch (_flowState)
        {
            case FlowState.Aiming:
                turret.rotationVelocity = Vector2.zero;
                break;
            case FlowState.Flying:
                _currentProjectile.rotationVelocity = Vector2.zero;
                break;
            case FlowState.Returning:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Lock the framerate to the screen's refresh rate (usually 60Hz)
        // TODO: Add option to toggle this on or off
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        // Start the game
        // TODO: Play a cutscene THEN start the game
        _gameState = GameState.Started;
        Cursor.lockState = CursorLockMode.Locked;

        GlobalController.Instance.DisableDepthOfField();

        SpawnAsteroids();
    }

    // Pause game
    private void Pause()
    {
        _gameState = GameState.Paused;
        Cursor.lockState = CursorLockMode.None;

        // Enable depth of field effects
        GlobalController.Instance.EnableDepthOfField();

        // Enable the pause menu
        menus[0].Enable();

        // Freeze game
        Time.timeScale = 0f;
    }

    // Resume game from pause
    public void Resume()
    {
        _gameState = GameState.Started;
        Cursor.lockState = CursorLockMode.Locked;

        // Disable depth of field effects
        GlobalController.Instance.DisableDepthOfField();

        // Disable menus
        foreach (Menu menu in menus)
        {
            menu.Disable();
            menu.SetInteractable(true);
        }

        // Unfreeze game
        Time.timeScale = 1f;
    }

    // Game over
    private void GameOver()
    {
        _gameState = GameState.GameOver;

        // Enable depth of field effects
        GlobalController.Instance.EnableDepthOfField();

        // TODO: Enable game over menu
        // TODO: Freeze game
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < 200; i++)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f));
            Quaternion spawnRotation = new Quaternion(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

            asteroids.Add(Instantiate(asteroidPrefabs[UnityEngine.Random.Range(0, asteroidPrefabs.Count)], spawnPosition, spawnRotation));
        }
    }

    // Change the game's flow state to a new state
    public void ChangeFlowState(FlowState newState)
    {
        _flowState = newState;

        if (_flowState != FlowState.Aiming)
        {
            turret.enabled = false;
        }
        else
        {
            turret.enabled = true;
        }

        if (_flowState == FlowState.Aiming)
        {
            mainCamera.followTarget = cameraPoint;
            mainCamera.rotateTarget = spawnPoint;
        }
    }
}