using UnityEngine;
using System.Collections;

public class EndTurnButton : UIButtonMonoBehaviour
{
    TweenRotation tweenRotation;

    protected override void Start()
    {
        base.Start();
        tweenRotation = GetComponent<TweenRotation>();
        EventDelegate.Add(tweenRotation.onFinished, OnTweenRotationFinish);
    }

    protected override void OnClickBehaviour()
    {
        SceneController.TurnController.TurnChange();
    }

    void OnTweenRotationFinish()
    {
        if (SceneController.TurnController.PlayerTurn)
            Enabled = true;
    }

    public void TurnChange(bool playerTurn)
    {
        Enabled = false;
        if(playerTurn)
        {
            tweenRotation.from = new Vector3(0f, 0f, 180f);
            tweenRotation.to = new Vector3(0f, 0f, 360f);
        }
        else
        {
            tweenRotation.from = new Vector3(0f, 0f, 0f);
            tweenRotation.to = new Vector3(0f, 0f, 180f);
        }
        tweenRotation.ResetToBeginning();
        tweenRotation.PlayForward();
    }
}
