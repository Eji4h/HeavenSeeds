using UnityEngine;
using System.Collections;

public class ElementBarController : UIProgressBar
{
    new static int numberOfSteps = 26;

    ElementType element;
    float oneStepValue;
    int count = 0;

    float Value
    {
        get { return value; }
        set 
        {
            this.value = value;
            if (this.value >= 0.99f)
            {
                SceneController.ChooseMonster.ReceiveAttackQueue(element);
                this.value = 0f;
            }
        }
    }

    // Use this for initialization
    new void Start()
    {
        oneStepValue = 1f / numberOfSteps;
        switch(name.Substring(0, 2))
        {
            case "Fi":
                element = ElementType.Fire;
                break;
            case "Wa":
                element = ElementType.Water;
                break;
            case "Ea":
                element = ElementType.Earth;
                break;
            case "Wo":
                element = ElementType.Wood;
                break;
        }
        base.Start();
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
