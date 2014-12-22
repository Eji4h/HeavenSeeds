using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PayUnity;
using System.Diagnostics;

public class BossKhchsingh : Monster
{
    #region EnumType
    enum BossKhchsinghState
    {
        ThrashTrunk3Time,
        ChargeToDash,
        Trample,
        ThrowATree,
        IvoryBeam
    }
    #endregion

    #region Variable
    List<BossKhchsinghState> listBossKhchsinghStateCanUse = new List<BossKhchsinghState>(5);
    #endregion

    #region Properties
    public override int Hp
    {
        get { return base.Hp; }
        set
        {
            base.Hp = value;
            if (Hp <= (int)(MaxHp * 0.1))
                listBossKhchsinghStateCanUse.Add(BossKhchsinghState.IvoryBeam);
        }
    }
    #endregion

    #region Method
    // Use this for initialization
    protected override void Start()
    {
        MaxHp = 5000;
        DamageBase = 143;
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ChargeToDash);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ThrashTrunk3Time);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.ThrowATree);
        listBossKhchsinghStateCanUse.Add(BossKhchsinghState.Trample);
        base.Start();
    }

    protected override void MonsterBehaviour()
    {
        thisAnimation.CrossFade(listBossKhchsinghStateCanUse[Random.Range(0, listBossKhchsinghStateCanUse.Count)].ToString());
    }
    #endregion
}
