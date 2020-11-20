using UnityEngine;

public class CountdownMode : GameController
{
    private float timer = 60f + 1f;
    private bool _doCountdown = true;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (flowState == FlowState.Returning)
        {
            StartCoroutine(ReturnDelay());
        }

        // Perform countdown
        if (_doCountdown) timer -= Time.fixedDeltaTime;
        UIController.Instance.UpdateCountdownText(timer);

        // If countdown runs out then game over
        if (timer <= 0f && _doCountdown)
        {
            GameOver();
            _doCountdown = false;
        }
        
        // If enough people boarded, perform rocket take off
        if (peopleBoarded >= BoardThreshold)
        {
            RocketTakeOff();
            peopleBoarded = 0;
            UIController.Instance.UpdateBoardText(peopleBoarded);
        }
    }
}
