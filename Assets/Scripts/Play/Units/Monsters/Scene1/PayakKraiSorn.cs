using UnityEngine;
using System.Collections;

public class PayakKraiSorn : Monster
{

    // Use this for initialization
    protected override void Start()
    {
        MaxHp = 1500;
        base.Start();

        SetLocalPositionReceiveDamageParticle(new Vector3(0f, 1.75f, 2.25f),
            new Vector3(0f, 1.75f, 2.25f), new Vector3(0f, 0.1f, 0f));
    }
}
