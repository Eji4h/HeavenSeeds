using UnityEngine;
using System.Collections;

public class KraiSornKarVee : Monster
{
    protected override void Awake()
    {
        base.Awake();
        maxHp = 1200;
        DamageBase = 63;
    }
}
