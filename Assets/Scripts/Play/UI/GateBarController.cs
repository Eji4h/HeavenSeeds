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

    bool isUpdateGateValue;

    public int GateCount
    {
        get { return gateCount; }
        set
        {
            int oldGateCount = gateCount,
                deltaGate;

            gateCount = Mathf.Clamp(value, 0, maxGate);
            if (gateCount != (int)(this.value / valuePerGate))
            {
                deltaGate = gateCount - oldGateCount;
                this.value += deltaGate * valuePerGate;
                this.value = Mathf.Clamp01(this.value);
                gateCount = (int)(this.value / valuePerGate);
            }

            if (checkGateCountIsTarget != null &&
                checkGateCountIsTarget(gateCount, gateCountTarget))
                gateCountTargetAction();
        }
    }

    int MaxGate
    {
        get { return maxGate; }
        set
        {
            maxGate = value;
            valuePerGate = 1f / maxGate;

            var frameSprite = transform.Find("Frame").GetComponent<UISprite>();
            frameSprite.spriteName = "ShortBar" + maxGate + "Split";
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

    public bool IsUpdateGateValue
    {
        get { return isUpdateGateValue; }
        set { isUpdateGateValue = value; }
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
        isUpdateGateValue = true;
        StartCoroutine(UpdateGateValue());
    }

    IEnumerator UpdateGateValue()
    {
        for (; ; )
        {
            if (isUpdateGateValue)
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
            }
            yield return null;
        }
    }
}
