using UnityEngine;
using System.Collections;

public class TacomStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Tacom", 95, 69, 71, 50, 34, 1800);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Main, GrowRateType.Third, GrowRateType.Secondary, GrowRateType.Bad, GrowRateType.Bad);
    }
}
