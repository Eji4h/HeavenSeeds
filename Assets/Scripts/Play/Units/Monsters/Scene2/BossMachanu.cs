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
    int waterCorrect = 0;

    // Use this for initialization
    protected override void Start()
    {
        MaxHp = 10000;
        DamageBase = 176;
        listBossMachanuState.Add(BossMachanuState.JumpDown);
        listBossMachanuState.Add(BossMachanuState.JumpPounded);
        base.Start();
    }

    protected override void MonsterBehaviour()
    {
        if (isDown)
            thisAnimation.CrossFade("JumpUp");
        else
        {
            if (waterCorrect < 6)
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

    public void ResetWaterCollect()
    {
        waterCorrect = 0;
    }

    public void IncreaseWaterCollect()
    {
        waterCorrect++;
    }

    public void SendDamgeSpecific(float damageBaseMultiply)
    {
        SendDamageToCharacter(damageBaseMultiply * (1 + waterCorrect * 0.05f));
    }

}
