using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CustomizeController : MonoBehaviour
{
    [SerializeField] private Projectile[] projectiles;
    [SerializeField] private Animator projectilesAnimator;
    private int currentSelected;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text requiredCoinText;

    [SerializeField] private TMP_Text unlockText;

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

        UIController.Instance.UpdateCoinText(PlayerPrefs.GetInt("Coin", 0));
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
    private void Select(Projectile selected)
    {
        foreach (Projectile projectile in projectiles)
        {
            projectile.gameObject.SetActive(false);
        }

        selected.gameObject.SetActive(true);

        if (selected.unlocked) 
        {
            selected.selected = true;
            unlockText.text = "Unlocked";
        }
        else
        {
            unlockText.text = "Unlock";
        }
        
        nameText.text = selected.name;
        requiredCoinText.text = "Required Coin - " + selected.requiredCoin.ToString();
    }

    // Unlock projectile to become playable
    public void Unlock()
    {
        if (!(PlayerPrefs.GetInt("Coin", 0) > projectiles[currentSelected].requiredCoin)) return;

        projectiles[currentSelected].unlocked = true;
        projectiles[currentSelected].selected = true;

        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin", 0) - projectiles[currentSelected].requiredCoin);
        if (currentSelected > PlayerPrefs.GetInt("Projectile", 0)) PlayerPrefs.SetInt("Projectile", currentSelected);

        UIController.Instance.UpdateCoinText(PlayerPrefs.GetInt("Coin", 0));
        unlockText.text = "Unlocked";

        projectilesAnimator.SetTrigger("unlock");
    }
}
