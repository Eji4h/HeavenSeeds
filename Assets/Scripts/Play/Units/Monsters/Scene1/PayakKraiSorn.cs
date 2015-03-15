using UnityEngine;
using System.Collections;

public class PayakKraiSorn : Monster
{
    protected override void Awake()
    {
        base.Awake();
        MaxHp = 1500;
        DamageBase = 74;
    }
}
