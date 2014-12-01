using UnityEngine;
using System.Collections;

public class EndTurnButton : MonoBehaviour
{
    UIButton uiButton;

    public bool Enabled
    {
        get { return uiButton.isEnabled; }
        set 
        {
            if(uiButton != null)
                uiButton.isEnabled = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        uiButton = GetComponent<UIButton>();
        EventDelegate.Add(uiButton.onClick, EndTurnButtonOnClick);
    }

    void EndTurnButtonOnClick()
    {
        SceneController.TurnController.TurnChange();
    }

    public void ResetDefaultColor()
    {
        if (uiButton != null)
            uiButton.ResetDefaultColor();
    }
}
