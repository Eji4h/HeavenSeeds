﻿using UnityEngine;
using System.Collections;

public class DuRongKraiSorn : Monster
{

    // Use this for initialization
    protected override void Start()
    {
        MaxHp = 1350;
        DamageBase = 55;
        base.Start();
    }
}
