using UnityEngine;

public class CountdownMode : GameController
{
    private float timer;

    // Update is called once per frame
    private void Update()
    {
        if (flowState == FlowState.Returning)
        {
            StartCoroutine(ReturnDelay());
        }
    }
}
