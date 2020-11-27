using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TileManager : MonoBehaviour
{
    public Tile[] tiles;
    [HideInInspector] public Button[] buttons;

    public TileSelector selector;
    public Tile CurrentSelectedTile { get; set; }

    private InputManager _inputManager;

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle directional input and click tiles
        _inputManager.Game.Direction.performed += DirectionOnPerformed;
        _inputManager.Game.Click.performed += ClickOnPerformed;

        _inputManager.Enable();

        Select(tiles[0]);
    }

    #region Input Methods

    private void DirectionOnPerformed(InputAction.CallbackContext context)
    {
        Vector2 directionValue = context.ReadValue<Vector2>();

        // if (directionValue.x > 0.5f) // Right
        // {
        //     Select(_currentSelectedTile.rightTile);
        // }
        // else if (directionValue.x < -0.5f) // Left
        // {
        //     Select(_currentSelectedTile.leftTile);
        // }

        if (directionValue.y > 0.5f && CurrentSelectedTile.upTile != null) // Up
            Select(CurrentSelectedTile.upTile);
        else if (directionValue.y < -0.5f && CurrentSelectedTile.downTile != null) // Down
            Select(CurrentSelectedTile.downTile);
    }

    private void ClickOnPerformed(InputAction.CallbackContext context)
    {
        Click(CurrentSelectedTile);
    }

    #endregion

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    // Select a tile
    public void Select(Tile tileToSelect)
    {
        // Deselect current tile first
        if (CurrentSelectedTile != null) CurrentSelectedTile.OnDeselected();

        // Select tile
        tileToSelect.OnSelected();
        CurrentSelectedTile = tileToSelect;

        // Set selector position
        selector.Select(tileToSelect);
    }

    // Click a tile
    private void Click(Tile tileToClick)
    {
        tileToClick.OnClicked();
    }

    // Awake is called when an object is initialized
    private void Awake()
    {
        buttons = new Button[tiles.Length];
        for (int i = 0; i < tiles.Length; i++) buttons[i] = tiles[i].GetComponent<Button>();
    }
}