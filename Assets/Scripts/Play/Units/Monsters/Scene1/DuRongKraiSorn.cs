using UnityEngine;
using System.Collections;

public class DuRongKraiSorn : Monster
{
    protected override void Awake()
    {
        base.Awake();
        maxHp = 900;
        DamageBase = 55;
    }
}
