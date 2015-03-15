using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossMachanu : Monster
{
    enum BossMachanuState
    {
        JumpDown,
        JumpPounded, 
        Ultimate
    }

    List<BossMachanuState> listBossMachanuState = new List<BossMachanuState>(3);

    bool isDown = false;

    protected override void Awake()
    {
        base.Awake();
        MaxHp = 6000;
        DamageBase = 176;
        listBossMachanuState.Add(BossMachanuState.JumpDown);
        listBossMachanuState.Add(BossMachanuState.JumpPounded);
    }

    protected override void MonsterBehaviour()
    {
        if (isDown)
            thisAnimation.CrossFade("JumpUp");
        else
        {
            if (attackUpStack < 6)
                thisAnimation.CrossFade(listBossMachanuState[Random.Range(0, listBossMachanuState.Count)].ToString());
            else
                thisAnimation.CrossFade("Ultimate");
        }
    }

    public void SetIsDownTrue()
    {
        isDown = true;
        isImmortal = true;
    }

    public void SetIsDownFalse()
    {
        isDown = false;
        isImmortal = false;
    }

}
