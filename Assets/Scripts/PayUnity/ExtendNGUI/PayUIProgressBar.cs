using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PayUIProgressBar : UIProgressBar
{
    #region Variable
    float maxValue,
        multiplyValue;
    #endregion

    #region Properties
    public float Value
    {
        get { return value; }
        set 
        {
            this.value = value * multiplyValue;
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
}
