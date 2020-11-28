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

    [SerializeField] private List<Projectile> projectilePrefabs = new List<Projectile>();
    private Projectile _currentProjectile;

    [SerializeField] private List<Asteroid> asteroidPrefabs = new List<Asteroid>();
    private readonly List<Asteroid> _asteroids = new List<Asteroid>();
    private const int AsteroidLimit = 100;

    [SerializeField] private List<Planet> badPlanetPrefabs;
    private readonly List<Planet> _planets = new List<Planet>();
    private const int PlanetLimit = 3;

    [SerializeField] private List<Coin> coinPrefabs;
    private readonly List<Coin> _coins = new List<Coin>();
    private const int CoinLimit = 100;

    // Number of people successfully boarded
    protected int peopleBoarded;
    protected int totalPeopleBoarded;
    // Number of people on board to take off
    protected const int BoardThreshold = 3;
    // Number of rocket take offs performed
    private int _takeOffs;
    [SerializeField] private Animator rocket;

    // Number of coins earned
    protected int earnedCoin;
    // Number of fatalities
    private int _fatalities;

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

    // Pause & resume game on escape
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

    // Shoot projectile
    private void ShootOnPerformed(InputAction.CallbackContext context)
    {
        // If not aiming or paused then return
        if (flowState != FlowState.Aiming || Time.deltaTime == 0f) return;

        // Shoot from turret
        turret.Shoot();

        // Create a new projectile and set it to current projectile
        _currentProjectile = Instantiate(projectilePrefabs[PlayerPrefs.GetInt("SelectedProjectile", 0)], turret.spawnPoint.position, turret.spawnPoint.rotation).GetComponent<Projectile>();

        // Set camera targets
        Transform currentProjectileTransform = _currentProjectile.transform;
        mainCamera.followTarget = currentProjectileTransform;
        mainCamera.rotateTarget = currentProjectileTransform;

        // Change to flying state
        ChangeFlowState(FlowState.Flying);

        // Reset turret rotation
        turret.ResetRotation();
    }

    // Aim with mouse, keyboard or gamepad
    private void AimOnPerformed(InputAction.CallbackContext context)
    {
        // If y invert is enabled then flip the rotation vector
        Vector2 rotationVeclocity;
        if (PlayerPrefs.GetInt("YInvert", 0) == 0)
            rotationVeclocity = context.ReadValue<Vector2>();
        else
            rotationVeclocity = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y * -1f);

        // If aiming with turret or aiming with projectile
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

    // Stop aiming
    private void AimOnCanceled(InputAction.CallbackContext context)
    {
        // Set all rotation velocity to vector 0
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
        _gameState = GameState.Started;
        Cursor.lockState = CursorLockMode.Locked;

        GlobalController.Instance.DisableDepthOfField();

        // Spawn the planets and asteroids
        SpawnAsteroids();
        SpawnPlanets();
        SpawnCoins();

        Randomize();
    }

    #region Pause, Resume & Game Over

    // Pause game
    private void Pause()
    {
        _gameState = GameState.Paused;
        Cursor.lockState = CursorLockMode.None;

        // Enable depth of field effects
        GlobalController.Instance.EnableDepthOfField();

        // Enable pause menu
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
    public void GameOver()
    {
        _gameState = GameState.GameOver;
        Cursor.lockState = CursorLockMode.None;

        // Enable depth of field effects
        GlobalController.Instance.EnableDepthOfField();

        // Enable game over menu
        menus[1].Enable();
        UIController.Instance.UpdateSummaryText(totalPeopleBoarded, _takeOffs, earnedCoin, _fatalities);

        // Freeze game
        Time.timeScale = 0f;
    }

    #endregion

    #region Spawn Objects

    private void SpawnAsteroids()
    {
        for (int i = 0; i < AsteroidLimit; i++)
        {
            // Asteroid to spawn
            Asteroid spawnAsteroid = asteroidPrefabs[UnityEngine.Random.Range(0, asteroidPrefabs.Count)];

            // Generate a random spawn rotation
            Quaternion spawnRotation = new Quaternion(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

            // Spawn asteroids and add them to a list
            _asteroids.Add(Instantiate(spawnAsteroid, Vector3.zero, spawnRotation));
        }
    }

    private void SpawnPlanets()
    {
        for (int i = 0; i < PlanetLimit; i++)
        {
            // Planet to spawn
            Planet spawnPlanet = badPlanetPrefabs[UnityEngine.Random.Range(0, badPlanetPrefabs.Count)];

            // Generate a random spawn rotation
            Quaternion spawnRotation = new Quaternion(0f, UnityEngine.Random.Range(0f, 1f), 0f, 0f);

            // Spawn planets and add them to a list
            _planets.Add(Instantiate(spawnPlanet, Vector3.zero, spawnRotation));
        }
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < CoinLimit; i++)
        {
            // Coin to spawn
            Coin spawnCoin = coinPrefabs[UnityEngine.Random.Range(0, coinPrefabs.Count)];

            // Generate a random spawn rotation
            Quaternion spawnRotation = spawnCoin.transform.rotation;

            // Spawn coins and add them to a list
            _coins.Add(Instantiate(spawnCoin, Vector3.zero, spawnRotation));
        }
    }

    public void Randomize()
    {
        // Randomize all asteroids
        foreach (Asteroid asteroid in _asteroids)
        {
            asteroid.StartRandomize(new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f)));
        }

        // Randomize all planets
        foreach (Planet planet in _planets)
        {
            planet.StartRandomize(new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(0f, 75f), UnityEngine.Random.Range(50f, 200f)));
        }

        // Randomize all coins
        foreach (Coin coin in _coins)
        {
            coin.StartRandomize(new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(0f, 75f), UnityEngine.Random.Range(50f, 200f)));
        }
    }

    #endregion

    // Change the game's flow state to a new state
    public void ChangeFlowState(FlowState newState)
    {
        flowState = newState;

        // If new flow state is flying then disable turret
        if (flowState == FlowState.Flying)
        {
            turret.enabled = false;
        }
        // If new flow state is aiming then enable turret and set camera follow and rotate target
        else if (flowState == FlowState.Aiming)
        {
            turret.enabled = true;

            mainCamera.followTarget = cameraPoint;
            mainCamera.rotateTarget = spawnPoint;
        }
        // If new flow state is returning then enable disable turrent and set camera follow and rotate target to null
        else
        {
            turret.enabled = false;

            mainCamera.followTarget = null;
            mainCamera.rotateTarget = null;
        }
    }

    // Delay when return
    protected IEnumerator ReturnDelay()
    {
        yield return new WaitForSeconds(0.25f);
        ChangeFlowState(FlowState.Aiming);
    }

    // Calculate post game ranking by people boarded, take offs and coins earned
    public virtual void CalculateRank()
    {
    }

    #region Game Events

    // Earn an amount of coin
    public void EarnCoin(int coin)
    {
        earnedCoin += coin;
        UIController.Instance.UpdateCoinText(earnedCoin);

        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin", 0) + 1);
    }

    // Board a number of people
    public void Board(int people)
    {
        peopleBoarded += people;
        UIController.Instance.UpdateBoardText(peopleBoarded);

        totalPeopleBoarded += people;
    }

    // Rocket take off
    protected void RocketTakeOff()
    {
        rocket.SetTrigger("takeOff");
        _takeOffs++;

        peopleBoarded = 0;
    }

    // Crash
    public void Crash(int fatalities)
    {
        _fatalities += fatalities;
    }

    #endregion
}