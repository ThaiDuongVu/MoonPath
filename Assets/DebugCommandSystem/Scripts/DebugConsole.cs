using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private bool _isConsoleVisible;

    // List of debug commands
    public List<object> debugCommands;

    private static DebugCommand _showFPS;
    private static DebugCommand _hideFPS;

    [SerializeField] private GameController gameController;
    private static DebugCommand _gameOver;

    private InputManager _inputManager;

    private void OnEnable()
    {
        _inputManager = new InputManager();

        _inputManager.Console.Show.performed += OnShowPerformed;
        _inputManager.Console.Enter.performed += OnEnterPerformed;

        _inputManager.Enable();
    }

    #region Input Methods

    private void OnShowPerformed(InputAction.CallbackContext context)
    {
        if (_isConsoleVisible)
            HideConsole();
        else
            ShowConsole();
    }

    private void OnEnterPerformed(InputAction.CallbackContext context)
    {
        // Properties when command requires a parameter
        string[] properties = inputField.text.Split(' ');

        // Iterate through list of commands
        foreach (DebugCommandBase debugCommand in debugCommands.Cast<DebugCommandBase>().Where(debugCommand => inputField.text.Contains(debugCommand.Format)))
            switch (debugCommand)
            {
                case DebugCommand command:
                    command.Invoke();
                    return;
                case DebugCommand<int> command1:
                    command1.Invoke(int.Parse(properties[1]));
                    return;
            }

        // If none match then said invalid command
        inputField.text = "Invalid command";
        // Reactivate input field
        inputField.ActivateInputField();
    }

    #endregion

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Awake()
    {
        _showFPS = new DebugCommand("show_fps", "Show game time in frames per second", "show_fps",
            () => { UIController.Instance.ShowFPS(); });

        _hideFPS = new DebugCommand("hide_fps", "Hide game fps", "hide_fps",
            () => { UIController.Instance.HideFPS(); });

        _gameOver = new DebugCommand("game_over", "Activate game over", "game_over",
            () => { gameController.GameOver(); });

        // Add all commands to list
        debugCommands = new List<object>
        {
            _showFPS,
            _hideFPS,
            _gameOver
        };
    }

    // Start is called before the first frame update
    private void Start()
    {
        HideConsole();
    }

    // Show and hide input console
    private void ShowConsole()
    {
        _isConsoleVisible = true;
        inputField.gameObject.SetActive(_isConsoleVisible);

        // Make input field available to type
        inputField.ActivateInputField();
        // Freeze game
        Time.timeScale = 0f;
    }

    private void HideConsole()
    {
        _isConsoleVisible = false;
        inputField.gameObject.SetActive(_isConsoleVisible);

        // Unfreeze game
        Time.timeScale = 1f;
    }
}