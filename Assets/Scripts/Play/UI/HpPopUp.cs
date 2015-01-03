using UnityEngine;
using System.Collections;

public class HpPopUp : MonoBehaviour
{
    static Camera mainCamera, uiCamara;
    static Transform allHpPopUpParentTransform;

    public static void SetCamera()
    {
        mainCamera = SceneController.MainCamera;
        uiCamara = SceneController.UICamera;
        allHpPopUpParentTransform = GameObject.Find("AllHpPopUp").transform;
    }

    UILabel thisLabel;
    TweenAlpha thisTweenAlpha;
    TweenPosition thisTweenPosition;
    TweenScale thisTweenScale;

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(-10f, -10f);
        transform.parent = allHpPopUpParentTransform;
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

        Vector3 targetScreenPoint = mainCamera.WorldToScreenPoint(targetPos),
            popUpPos = uiCamara.ScreenToWorldPoint(targetScreenPoint);

        transform.position = new Vector3(popUpPos.x, popUpPos.y + 0.4f);
        thisTweenPosition.SetEndToCurrentValue();
        transform.position = new Vector3(popUpPos.x, popUpPos.y + 0.2f);
        thisTweenPosition.SetStartToCurrentValue();
        thisTweenAlpha.ResetToBeginning();
        thisTweenPosition.ResetToBeginning();
        thisTweenScale.ResetToBeginning();
        thisTweenAlpha.PlayForward();
        thisTweenPosition.PlayForward();
        thisTweenScale.PlayForward();
    }
}
