using UnityEngine;
using System;
using System.Collections;

public class GateBarController : UIProgressBar
{
    int gateCount,
        maxGate;

    float changePerSecond;
    float valuePerGate;
    float totalValueElapsedToGateUp;

    Predicate<int> checkGateCountIsTarget;
    Action gateCountTargetAction;

    public int GateCount
    {
        get { return gateCount; }
        set
        {
            int oldGateCount = gateCount,
                deltaGate;

            gateCount = Mathf.Clamp(value, 0, maxGate);
            deltaGate = gateCount - oldGateCount;
            this.value += deltaGate * valuePerGate;
            if (checkGateCountIsTarget != null &&
                checkGateCountIsTarget(gateCount))
            {
                gateCountTargetAction();
                StopCoroutine(UpdateGateValue());
            }
        }
    }

    public int MaxGate
    {
        get { return maxGate; }
        set
        {
            maxGate = value;
            valuePerGate = 1f / maxGate;

            var bgSprite = backgroundWidget.GetComponent<UISprite>();
            bgSprite.spriteName = "ShortBar" + maxGate + "Split";
        }
    }

    public void SetInit(int maxGate, float changePerSecond,
        Predicate<int> checkGateCountIsTarget, Action gateCountTargetAction)
    {
        MaxGate = maxGate;
        this.changePerSecond = changePerSecond;
        this.checkGateCountIsTarget = checkGateCountIsTarget;
        this.gateCountTargetAction = gateCountTargetAction;
    }

    // Use this for initialization
    new void Start()
    {
        base.Start();
        StartCoroutine(UpdateGateValue());
    }

    IEnumerator UpdateGateValue()
    {
        for (; ; )
        {
            float valueChange = Time.deltaTime * changePerSecond;

            value += valueChange;

            totalValueElapsedToGateUp += valueChange;
            if (totalValueElapsedToGateUp >= valuePerGate)
            {
                if (changePerSecond >= 0f)
                    GateCount++;
                else
                    GateCount--;
                totalValueElapsedToGateUp -= valuePerGate;
            }
            yield return null;
        }
    }
}
