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

    int gateCountTarget;
    Func<int, int, bool> checkGateCountIsTarget;
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
                checkGateCountIsTarget(gateCount, gateCountTarget))
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

    public int GateCountTarget
    {
        get { return gateCountTarget; }
        set { gateCountTarget = value; }
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

    public void SetCheckGateCountIsTarget(bool isMoreThan)
    {
        if (isMoreThan)
            checkGateCountIsTarget = CheckGateCountIsHigherThan;
        else
            checkGateCountIsTarget = CheckGateCountIsLowerThan;
    }

    bool CheckGateCountIsHigherThan(int gateCount, int gateCountTarget)
    {
        return gateCount >= gateCountTarget;
    }

    bool CheckGateCountIsLowerThan(int gateCount, int gateCountTarget)
    {
        return gateCount <= gateCountTarget;
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
            if (value < 1f)
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
            }
            else
                totalValueElapsedToGateUp = 0f;
            yield return null;
        }
    }
}
