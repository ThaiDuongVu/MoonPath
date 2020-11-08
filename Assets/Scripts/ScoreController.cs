using UnityEngine;

public class ScoreController : MonoBehaviour
{
    // Use a singleton pattern to make the class globally accessible
    #region Singleton

    private static ScoreController _instance;

    public static ScoreController Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<ScoreController>();

            return _instance;
        }
    }

    #endregion

    [HideInInspector] private int _score;
    private int _highScore;

    // Awake is called when object is initialized
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void AddScore(int amount)
    {
        _score += amount;
        UIController.Instance.UpdateScoreText(_score);
    }
}
