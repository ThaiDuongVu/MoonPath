using UnityEngine;
using UnityEngine.InputSystem;

public class CustomizeController : MonoBehaviour
{
    [SerializeField] private Projectile[] projectiles;
    private int currentSelected;

    private InputManager _inputManager;

    private void OnEnable()
    {
        _inputManager = new InputManager();

        _inputManager.Game.Direction.performed += DirectionOnPerformed;

        _inputManager.Enable();
    }

    #region Input Methods

    private void DirectionOnPerformed(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().x < -0.5f)
        {
            ToggleLeft();
        }
        else if (context.ReadValue<Vector2>().x > 0.5f)
        {
            ToggleRight();
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
        currentSelected = PlayerPrefs.GetInt("Projectile", 0);
        Select(projectiles[currentSelected]);
    }
    public void ToggleLeft()
    {
        if (currentSelected == 0)
        {
            currentSelected = projectiles.Length - 1;
        }
        else
        {
            currentSelected--;
        }
        Select(projectiles[currentSelected]);
    }

    public void ToggleRight()
    {
        if (currentSelected == projectiles.Length - 1)
        {
            currentSelected = 0;
        }
        else
        {
            currentSelected++;
        }
        Select(projectiles[currentSelected]);
    }

    // Select a projectile
    private void Select(Projectile projectile)
    {
        foreach (Projectile proj in projectiles)
        {
            proj.gameObject.SetActive(false);
        }

        projectile.gameObject.SetActive(true);

        if (currentSelected <= PlayerPrefs.GetInt("Projectile", 0))
        {
            PlayerPrefs.GetInt("Projectile", currentSelected);
        }
    }
}
