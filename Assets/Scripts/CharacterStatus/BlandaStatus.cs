using UnityEngine;
using System.Collections;

public class BlandaStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Blanda", 57, 82, 52, 93, 40, 2300);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Third, GrowRateType.Secondary, GrowRateType.Bad, GrowRateType.Main, GrowRateType.Bad);
    }
}
