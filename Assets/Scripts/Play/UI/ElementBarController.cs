using UnityEngine;
using System.Collections;

public class ElementBarController : MonoBehaviour
{
    #region Variable
    UIProgressBar progressBar;
    public ElementType element;
    float oneStepValue;

    delegate void ProgressBarFullEvent();
    ProgressBarFullEvent progressBarFullEvent;
    #endregion

    #region Properties
    float Value
    {
        get { return progressBar.value; }
        set 
        {
            progressBar.value = value;
            if (progressBar.value >= 0.99f)
            {
                Unit.Monster.ReceiveQueue(element);
                progressBar.value = 0f;
            }
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        progressBar = GetComponent<UIProgressBar>();
        oneStepValue = 1f / progressBar.numberOfSteps;
    }

    public void IncreaseOneStepValue()
    {
        Value += oneStepValue * 2f;
    }
}
