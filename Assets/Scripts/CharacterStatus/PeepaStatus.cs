using UnityEngine;
using System.Collections;

public class PeepaStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Peepa", 41, 35, 88, 70, 75, 1600);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Bad, GrowRateType.Bad, GrowRateType.Main, GrowRateType.Third, GrowRateType.Secondary);
    }
}
