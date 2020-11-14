using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private GameState _gameState;

    [SerializeField] private Menu[] menus;

    [HideInInspector] public FlowState flowState;
    [SerializeField] private Turret turret;

    public MainCamera mainCamera;
    public Transform cameraPoint;
    public Transform spawnPoint;

    [SerializeField] private List<Projectile> projectiles = new List<Projectile>();
    private Projectile _currentProjectile;

    [SerializeField] private List<Asteroid> asteroidPrefabs = new List<Asteroid>();
    private readonly List<Asteroid> _asteroids = new List<Asteroid>();
    private const int AsteroidLimit = 100;

    [SerializeField] private Planet badPlanetPrefab;
    private readonly List<Planet> _planets = new List<Planet>();
    private const int PlanetLimit = 5;

    [SerializeField] private Coin coinPrefab;
    private readonly List<Coin> _coins = new List<Coin>();
    private const int CoinLimit = 20;

    private InputManager _inputManager;

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle game pause input
        _inputManager.Game.Escape.performed += EscapeOnPerformed;

        // Handle shoot input
        _inputManager.Player.Shoot.performed += ShootOnPerformed;

        // Handle aim input
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
        if (flowState != FlowState.Aiming || Time.deltaTime == 0f) return;

        turret.Shoot();

        _currentProjectile = Instantiate(projectiles[UnityEngine.Random.Range(0, projectiles.Count - 1)], turret.spawnPoint.position, turret.spawnPoint.rotation).GetComponent<Projectile>();

        Transform currentProjectileTransform = _currentProjectile.transform;
        mainCamera.followTarget = currentProjectileTransform;
        mainCamera.rotateTarget = currentProjectileTransform;

        ChangeFlowState(FlowState.Flying);
    }

    private void AimOnPerformed(InputAction.CallbackContext context)
    {
        Vector2 rotationVeclocity;
        if (PlayerPrefs.GetInt("YInvert", 0) == 0)
            rotationVeclocity = context.ReadValue<Vector2>();
        else
            rotationVeclocity = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y * -1f);

        switch (flowState)
        {
            case FlowState.Aiming:
                turret.rotationVelocity = rotationVeclocity;
                break;
            case FlowState.Flying:
                _currentProjectile.rotationVelocity = rotationVeclocity;
                break;
            case FlowState.Returning:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AimOnCanceled(InputAction.CallbackContext context)
    {
        switch (flowState)
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
        // Lock the framerate to the 60fps
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        // Start the game
        // TODO: Play a cutscene THEN start the game
        _gameState = GameState.Started;
        Cursor.lockState = CursorLockMode.Locked;

        GlobalController.Instance.DisableDepthOfField();

        SpawnAsteroids();
        SpawnPlanets();
        SpawnCoins();
    }

    // Update is called once per frame
    private void Update()
    {
        if (flowState == FlowState.Returning)
        {
            StartCoroutine(ReturnDelay());
        }
    }

    private IEnumerator ReturnDelay()
    {
        yield return new WaitForSeconds(0.25f);

        ChangeFlowState(FlowState.Aiming);
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
        for (int i = 0; i < AsteroidLimit; i++)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f));
            Quaternion spawnRotation = new Quaternion(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

            _asteroids.Add(Instantiate(asteroidPrefabs[UnityEngine.Random.Range(0, asteroidPrefabs.Count)], spawnPosition, spawnRotation));
        }
    }

    private void SpawnPlanets()
    {
        for (int i = 0; i < PlanetLimit; i++)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f));
            Quaternion spawnRotation = new Quaternion(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

            _planets.Add(Instantiate(badPlanetPrefab, spawnPosition, spawnRotation));
        }
    }

    private void SpawnCoins()
    {

    }

    public void Randomize()
    {
        foreach (Asteroid asteroid in _asteroids)
        {
            asteroid.StartRandomize(new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f)));
        }

        foreach (Planet planet in _planets)
        {
            planet.StartRandomize(new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f)));
        }
    }

    // Change the game's flow state to a new state
    public void ChangeFlowState(FlowState newState)
    {
        flowState = newState;

        if (flowState == FlowState.Flying)
        {
            turret.enabled = false;
        }
        else if (flowState == FlowState.Aiming)
        {
            turret.enabled = true;

            mainCamera.followTarget = cameraPoint;
            mainCamera.rotateTarget = spawnPoint;
        }
        else
        {
            turret.enabled = false;

            mainCamera.followTarget = null;
            mainCamera.rotateTarget = null;
        }
    }
}