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
        maxHp = 6000;
        DamageBase = 176;
        listBossMachanuState.Add(BossMachanuState.JumpDown);
        listBossMachanuState.Add(BossMachanuState.JumpPounded);
    }

    protected override void MonsterBehaviour()
    {
        if (isDown)
            ThisAnimation.CrossFade("JumpUp");
        else
        {
            if (attackUpStack < 6)
                ThisAnimation.CrossFade(listBossMachanuState[Random.Range(0, listBossMachanuState.Count)].ToString());
            else
                ThisAnimation.CrossFade("Ultimate");
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
