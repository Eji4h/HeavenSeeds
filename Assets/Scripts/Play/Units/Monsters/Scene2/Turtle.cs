using UnityEngine;
using System.Collections;

public class Turtle : Monster
{
    protected override void Awake()
    {
        base.Awake();
        maxHp = 2100;
        DamageBase = 80;
    }
}
