using UnityEngine;
using System.Collections;

public class ElementBarController : MonoBehaviour
{
    static int numberOfSteps = 26;

    UIProgressBar progressBar;
    public ElementType element;
    float oneStepValue;
    int count = 0;

    float Value
    {
        get { return progressBar.value; }
        set 
        {
            progressBar.value = value;
            if (progressBar.value >= 0.99f)
            {
                Unit.Monster.ReceiveAttackQueue(element);
                progressBar.value = 0f;
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        progressBar = GetComponent<UIProgressBar>();
        oneStepValue = 1f / numberOfSteps;
    }

    public void Increase()
    {
        count++;
        for (int i = 0; i < count; i++)
            Value += oneStepValue;
    }

    public void ResetCount()
    {
        count = 0;
    }
}
