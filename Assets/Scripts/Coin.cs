using UnityEngine;

public class Coin : Asteroid
{
    private const float RotateSpeed = 5f;

    // Awake is called when object is initialized
    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotate(RotateSpeed);
    }
}
