using UnityEngine;
using System.Collections;

public class Hemrach : Monster
{
    protected override void Awake()
    {
        base.Awake();
        maxHp = 1500;
        DamageBase = 90;
    }
}
