using UnityEngine;
using System.Collections;

public class NoochStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Nooch", 81, 91, 43, 63, 48, 1750);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Secondary, GrowRateType.Main, GrowRateType.Bad, GrowRateType.Third, GrowRateType.Bad);
    }
}
