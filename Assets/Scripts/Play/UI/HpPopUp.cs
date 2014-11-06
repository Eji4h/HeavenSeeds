using UnityEngine;
using System.Collections;

public class HpPopUp : MonoBehaviour
{
    #region Static
    static Camera mainCamera, uiCamara;
    static float upperPosYTween = 100f;
    static Transform allHpPopUpParentTransform;
    #endregion

    #region Variable
    UILabel thisLabel;
    TweenAlpha thisTweenAlpha;
    TweenPosition thisTweenPosition;
    TweenScale thisTweenScale;
    #endregion

    #region Static Method
    public static void SetCamera()
    {
        mainCamera = SceneController.MainCamera;
        uiCamara = SceneController.UICamera;
        allHpPopUpParentTransform = GameObject.Find("AllHpPopUp").transform;
    }
    #endregion

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

    public void PopUp(int value, Vector3 targetPos, bool isDmg)
    {
        thisLabel.color = isDmg ? Color.red : Color.green;
        thisLabel.text = value.ToString();

        Vector3 targetScreenPoint = mainCamera.WorldToScreenPoint(targetPos),
            popUpPos = uiCamara.ScreenToWorldPoint(targetScreenPoint);

        transform.position = new Vector3(popUpPos.x, popUpPos.y + 0.5f);
        thisTweenPosition.SetEndToCurrentValue();
        transform.position = new Vector3(popUpPos.x, popUpPos.y + 0.3f);
        thisTweenPosition.SetStartToCurrentValue();
        thisTweenAlpha.ResetToBeginning();
        thisTweenPosition.ResetToBeginning();
        thisTweenScale.ResetToBeginning();
        thisTweenAlpha.PlayForward();
        thisTweenPosition.PlayForward();
        thisTweenScale.PlayForward();
    }

}
