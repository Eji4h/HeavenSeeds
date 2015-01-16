using UnityEngine;
using System.Collections;

public class KraiSornKarVee : Monster
{

	// Use this for initialization
    protected override void Start()
    {
        MaxHp = 1200;
        DamageBase = 63;
        base.Start();
    }
}
