using UnityEngine;
using System.Collections;

public class Toad : Monster
{
    protected override void Awake()
    {
        base.Awake();
        MaxHp = 1800;
        DamageBase = 82;
    }
}
