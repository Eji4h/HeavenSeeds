using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class BossKhchsingh : Monster
{
    enum BossKhchsinghState
    {
        ThrashTrunk3Time,
        ChargeToDash,
        Trample,
        ThrowATree,
        IvoryBeam
    }

    List<BossKhchsinghState> listBossKhchsinghStateCanUse = new List<BossKhchsinghState>(5);
    bool addUltimateSkillToListStateCanUse = false;

    public override int Hp
    {
        get { return base.Hp; }
        set
        {
            base.Hp = value;

            if (!addUltimateSkillToListStateCanUse &&
                Hp <= (int)(MaxHp * 0.2))
            {
                listBossKhchsinghStateCanUse.Add(BossKhchsinghState.IvoryBeam);
                addUltimateSkillToListStateCanUse = true;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ChargeToDash);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ThrashTrunk3Time);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ThrowATree);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.Trample);
    }

    protected override void MonsterBehaviour()
    {
        ThisAnimation.CrossFade(listBossKhchsinghStateCanUse[Random.Range(0, listBossKhchsinghStateCanUse.Count)].ToString());
    }
}
