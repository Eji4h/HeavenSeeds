using UnityEngine;
using System.Collections;

public class DaraStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Dara", 33, 97, 72, 47, 59, 1550);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Bad, GrowRateType.Main, GrowRateType.Secondary, GrowRateType.Bad, GrowRateType.Third);
    }
}
