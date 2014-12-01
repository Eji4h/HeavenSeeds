using UnityEngine;
using System.Collections;

public abstract class UIButtonMonoBehaviour : MonoBehaviour
{
    UIButton uiButton;

    public bool Enabled
    {
        get { return uiButton.isEnabled; }
        set
        {
            if (uiButton != null)
                uiButton.isEnabled = value;
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        uiButton = GetComponent<UIButton>();
        EventDelegate.Add(uiButton.onClick, OnClickBehaviour);
    }

    protected abstract void OnClickBehaviour();

    public void ResetDefaultColor()
    {
        if (uiButton != null)
            uiButton.ResetDefaultColor();
    }
}
