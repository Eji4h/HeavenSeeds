using UnityEngine;
using System.Collections;

public class DuRongKraiSorn : Monster
{

    // Use this for initialization
    protected override void Start()
    {
        MaxHp = 1350;
        base.Start();

        material = transform.Find("DuRongKraiSornSD1:Mesh").renderer.material;
        SetLocalPositionReceiveDamageParticle(new Vector3(0f, 1.75f, 2.25f),
            new Vector3(0f, 1.75f, 2.25f), new Vector3(0f, 0.1f, 0f));
    }
}
