using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const float InterpolationRatio = 0.01f;
    private const float FloatingRange = 5f;

    // // Awake is called when object is initialized
    // private void Awake()
    // {

    // }

    // // Start is called before the first frame update
    // private void Start()
    // {

    // }

    // Update is called once per frame
    private void Update()
    {
        if (Time.timeScale == 0f) return;

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + Random.Range(-FloatingRange, FloatingRange), transform.position.y + Random.Range(-FloatingRange, FloatingRange), transform.position.z + Random.Range(-FloatingRange, FloatingRange)), InterpolationRatio);
    }

    public void Randomize()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f));
    }
}
