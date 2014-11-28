using UnityEngine;
using System.Collections;
using PayUnity;

public class PauseButton : MonoBehaviour
{
    bool isPause = false;
    UIButton uiButton;

    // Use this for initialization
    void Start()
    {
        uiButton = GetComponent<UIButton>();
        EventDelegate.Add(uiButton.onClick, PauseButtonOnClick);
    }

    void PauseButtonOnClick()
    {
        isPause = !isPause;
        GameManager.Instance.Pause = isPause;
        UIController.EndTurnButton.Enabled = !isPause;
        UIController.SpinButton.isEnabled = !isPause;
        //UIController.PauseButton.isEnabled = !isPause;
    }
}
