using UnityEngine;

public class CountdownMode : GameController
{
    // Countdown timer
    private float timer = 90f + 1f;
    // Whether to perform countdown or not
    private bool _doCountdown = true;

    // Rank of play through
    private int _starRank;
    private const int MaxRank = 5;
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
        if (timer <= 0f + 1f && _doCountdown)
        {
            GameOver();
            _doCountdown = false;

            CalculateRank();
        }

        // If enough people boarded, perform rocket take off
        if (peopleBoarded >= BoardThreshold)
        {
            RocketTakeOff();
            UIController.Instance.UpdateBoardText(peopleBoarded);
        }
    }

    // Determine the rank of play through
    public override void CalculateRank()
    {
        int rank = 0;

        if (totalPeopleBoarded >= 2 && totalPeopleBoarded < 4)
        {
            rank += 1;
        }
        else if (totalPeopleBoarded >= 4 && totalPeopleBoarded < 6)
        {
            rank += 2;
        }
        else if (totalPeopleBoarded >= 6 && totalPeopleBoarded < 8)
        {
            rank += 3;
        }
        else if (totalPeopleBoarded >= 8 && totalPeopleBoarded < 10)
        {
            rank += 4;
        }
        else if (totalPeopleBoarded >= 10)
        {
            rank += 5;
        }

        if (earnedCoin >= 5 && earnedCoin < 10)
        {
            rank += 1;
        }
        else if (earnedCoin >= 10 && earnedCoin < 15)
        {
            rank += 2;
        }
        else if (earnedCoin >= 15 && earnedCoin < 20)
        {
            rank += 3;
        }
        else if (earnedCoin >= 20 && earnedCoin < 25)
        {
            rank += 4;
        }
        else if (earnedCoin >= 25)
        {
            rank += 5;
        }

        rank = Mathf.RoundToInt(rank / 2);

        UIController.Instance.UpdateRank(rank);
    }
}
