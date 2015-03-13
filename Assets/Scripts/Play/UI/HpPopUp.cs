using UnityEngine;
using System.Collections;

public class HpPopUp : MonoBehaviour
{
    UILabel thisLabel;
    TweenAlpha thisTweenAlpha;
    TweenPosition thisTweenPosition;
    TweenScale thisTweenScale;

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(-10f, -10f);
        transform.parent = UIController.AllHpPopUpParentTransform;
        thisLabel = GetComponent<UILabel>();
        thisTweenAlpha = GetComponent<TweenAlpha>();
        thisTweenPosition = GetComponent<TweenPosition>();
        thisTweenScale = GetComponent<TweenScale>();

        thisTweenPosition.SetStartToCurrentValue();
        thisTweenPosition.SetEndToCurrentValue();
    }

    public void PopUp(int value, Vector3 targetPos, Color32 color)
    {
        thisLabel.color = color;
        thisLabel.text = value.ToString();

        transform.position = OftenMethod.NGUITargetWorldPoint(targetPos, new Vector2(0f, 0.4f),
            SceneController.MainCamera, SceneController.UICamera);
        thisTweenPosition.SetEndToCurrentValue();
        transform.position = OftenMethod.NGUITargetWorldPoint(targetPos, new Vector2(0f, 0.2f),
            SceneController.MainCamera, SceneController.UICamera);
        thisTweenPosition.SetStartToCurrentValue();
        thisTweenAlpha.ResetToBeginning();
        thisTweenPosition.ResetToBeginning();
        thisTweenScale.ResetToBeginning();
        thisTweenAlpha.PlayForward();
        thisTweenPosition.PlayForward();
        thisTweenScale.PlayForward();
    }
}
