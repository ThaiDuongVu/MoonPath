using UnityEngine;

public class CountdownMode : GameController
{
    // Countdown timer
    private float timer = 120f + 1f;
    // Whether to perform countdown or not
    private bool _doCountdown = true;

    // Rank of play through
    private int _starRank;
    private const int MaxRank = 10;
    private const int MinRank = 1;

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

    // Determine the rank of play through
    public override void CalculateRank()
    {

    }
}
