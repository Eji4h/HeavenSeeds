﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PayUIProgressBar : UIProgressBar
{
    #region Variable
    float maxValue,
        multiplyValue;

    List<RangeOfColor> listRangeOfColor;
    #endregion

    #region Properties
    public float Value
    {
        get { return value; }
        set 
        {
            this.value = value * multiplyValue;

            if(listRangeOfColor != null)
            {
                listRangeOfColor.ForEach(rangeOfColor =>
                    {
                        if (this.value >= rangeOfColor.lowestRange &&
                            this.value <= rangeOfColor.highestRange)
                            mFG.color = rangeOfColor.colorThisRange;
                    });
            }
        }
    }

    public float MaxValue
    {
        get { return maxValue; }
        set 
        {
            maxValue = value;
            multiplyValue = 1f / maxValue;
        }
    }
    #endregion

    void Awake()
    {
        List<RangeOfColor> listRangeOfColor = new List<RangeOfColor>(GetComponentsInChildren<RangeOfColor>());
        if (listRangeOfColor.Count > 0)
            this.listRangeOfColor = listRangeOfColor;
    }
}
