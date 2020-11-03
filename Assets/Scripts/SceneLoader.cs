using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    private Animator _cameraAnimator;
    private Canvas _canvas;

    // Cached string for triggering animations
    private static readonly int Outro = Animator.StringToHash("outro");

    // Awake is called when an object is initialized
    private void Awake()
    {
        if (!(Camera.main is null)) _cameraAnimator = Camera.main.GetComponent<Animator>();
        _canvas = FindObjectOfType<Canvas>();
    }

    // Load a scene
    public void Load(string scene)
    {
        StartCoroutine(LoadDelay(scene));
    }

    // Load a scene with delay for transition animation to play
    private IEnumerator LoadDelay(string scene)
    {
        // Reset time scale
        Time.timeScale = 1f;
        // Disable depth of field effects
        GlobalController.Instance.DisableDepthOfField();
        // Disable canvas
        _canvas.enabled = false;

        // Play transition animation
        _cameraAnimator.SetTrigger(Outro);

        // Delay the load for animation to play
        yield return new WaitForSeconds(30f * Time.unscaledDeltaTime);

        // Once delayed, load scene
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    // Quit game
    public void Quit()
    {
        Application.Quit();
    }
}