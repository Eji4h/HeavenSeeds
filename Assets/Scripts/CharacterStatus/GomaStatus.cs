using UnityEngine;
using System.Collections;

public class GomaStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Goma", 47, 54, 83, 65, 87, 1450);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Bad, GrowRateType.Bad, GrowRateType.Secondary, GrowRateType.Third, GrowRateType.Main);
    }
}
