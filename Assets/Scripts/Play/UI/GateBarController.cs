using UnityEngine;
using System;
using System.Collections;

public class GateBarController : UIProgressBar
{
    int gateCount,
        maxGate;

    float changePerSecond, 
        valuePerGate,
        totalValueElapsedToGateUp;

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
            this.value = Mathf.Clamp01(this.value);
            if (checkGateCountIsTarget != null &&
                checkGateCountIsTarget(gateCount))
            {
                gateCountTargetAction();
                StopCoroutine(UpdateGateValue());
            }
        }
    }

    int MaxGate
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

    public float ChangePerSecond
    {
        get { return changePerSecond; }
        set { changePerSecond = value / MaxGate; }
    }

    public Predicate<int> CheckGateCountIsTarget
    {
        get { return checkGateCountIsTarget; }
        set { checkGateCountIsTarget = value; }
    }

    public Action GateCountTargetAction
    {
        get { return gateCountTargetAction; }
        set { gateCountTargetAction = value; }
    }

    public void SetInit(int maxGate, float changePerSecond)
    {
        transform.parent = UIController.BarsTransform;
        transform.localScale = Vector3.one;
        MaxGate = maxGate;
        ChangePerSecond = changePerSecond;
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
