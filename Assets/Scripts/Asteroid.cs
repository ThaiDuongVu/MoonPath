using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const float InterpolationRatio = 0.05f;

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

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z + Random.Range(-1f, 1f)), InterpolationRatio);
    }

    public void Randomize()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(-75f, 75f), UnityEngine.Random.Range(-25f, 100f), UnityEngine.Random.Range(50f, 200f));
    }
}
