using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

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

    BossKhchsinghState nextBossKhchsinghState;
    List<BossKhchsinghState> listBossKhchsinghStateCanUse = new List<BossKhchsinghState>(5);
    bool addUltimateSkillToListStateCanUse = false;

    Dictionary<BossKhchsinghState, int>
        BossKhchsinghStateUseGateDic = new Dictionary<BossKhchsinghState, int>();

    public override int Hp
    {
        get { return base.Hp; }
        set
        {
            base.Hp = value;
            CheckToAddUltimateSkillToListStateCanUse(Hp, MaxHp);
        }
    }

    void CheckToAddUltimateSkillToListStateCanUse(int hp, int maxHp)
    {
        if (!addUltimateSkillToListStateCanUse &&
            hp <= (int)(maxHp * 0.2))
        {
            listBossKhchsinghStateCanUse.Add(BossKhchsinghState.IvoryBeam);
            addUltimateSkillToListStateCanUse = true;
        }
    }

    protected override void Start()
    {
        base.Start();

        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ChargeToDash);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ThrashTrunk3Time);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ThrowATree);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.Trample);

        BossKhchsinghStateUseGateDic.Add(BossKhchsinghState.ThrashTrunk3Time, 2);
        BossKhchsinghStateUseGateDic.Add(BossKhchsinghState.ChargeToDash, 2);
        BossKhchsinghStateUseGateDic.Add(BossKhchsinghState.Trample, 3);
        BossKhchsinghStateUseGateDic.Add(BossKhchsinghState.ThrowATree, 3);
        BossKhchsinghStateUseGateDic.Add(BossKhchsinghState.IvoryBeam, 5);

        GateBarController.SetCheckGateCountIsTarget(true);
        GateBarController.gameObject.SetActive(false);
        GateBarController.GateCountTargetAction = BossKhchsinghAttack;
    }

    protected override void MonsterBehaviour()
    {
        nextBossKhchsinghState = listBossKhchsinghStateCanUse[
            Random.Range(0, listBossKhchsinghStateCanUse.Count)];

        GateBarController.GateCountTarget = BossKhchsinghStateUseGateDic[nextBossKhchsinghState];
    }

    void BossKhchsinghAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        ThisAnimation.CrossFade(nextBossKhchsinghState.ToString());
        GateBarController.GateCount -= BossKhchsinghStateUseGateDic[nextBossKhchsinghState];
        yield return new WaitForSeconds(ThisAnimation[nextBossKhchsinghState.ToString()].length);
    }
}
