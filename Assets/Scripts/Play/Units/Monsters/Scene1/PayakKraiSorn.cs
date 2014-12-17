using UnityEngine;
using System.Collections;

public class PayakKraiSorn : Monster
{

    // Use this for initialization
    protected override void Start()
    {
        MaxHp = 1500;
        DamageBase = 74;
        base.Start();
    }
}
