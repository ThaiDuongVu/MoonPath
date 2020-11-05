using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Whether menu is disabled when scene loads
    [SerializeField] private bool disableOnStartup;

    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileSelector tileSelector;

    [SerializeField] private Image background;

    // Start is called before the first frame update
    private void Start()
    {
        if (disableOnStartup) Disable();

        SetInteractable(true);
    }

    // Enable menu
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    // Disable menu
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    // Set whether a menu is interactable or not
    public void SetInteractable(bool interactable)
    {
        tileManager.enabled = interactable;
        background.gameObject.SetActive(!interactable);

        if (!interactable)
        {
            tileSelector.Animator.speed = 0f;
            tileManager.CurrentSelectedTile.OnDeselected();
        }
        else
        {
            tileSelector.Animator.speed = 1f;
            tileManager.CurrentSelectedTile.OnSelected();
        }

        foreach (Button button in tileManager.buttons) button.interactable = interactable;
    }
}