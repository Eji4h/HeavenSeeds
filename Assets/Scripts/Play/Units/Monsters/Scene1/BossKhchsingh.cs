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
        Idle,
        Move,
        Madden,
        CallOrdinate,
        ThrashTrunk3Time,
        ChargeToDash,
        Trample,
        ThrowATree,
        IvoryBeam
    }
    #endregion

    #region Variable
    BossKhchsinghState bossKhchsinghState;
    float minimumDmgMultiply = 0.9f,
        maximumDmgMultiply = 1.1f;

    GameObject maddenSparkGameObjectParticle,
        maddenReleaseGameObjectParticle,
        chargeGameObjectParticle,
        fallGameObjectParticle,
        thrashTrunkHitGameObjectParticle,
        treeBrokeGameObjectParticle,
        groundSlamGameObjectParticle,
        ultimateGameObjectParticle;

    GameObject treeForThrowGameObject;
    #endregion

    #region Properties

    #endregion

    #region Method

    // Use this for initialization
    protected override void Start()
    {
        MaxHp = 1000;
        base.Start();

        SetLocalPositionReceiveDamageParticle(new Vector3(0f, 4f, 4.75f),
            new Vector3(0f, 4f, 4.75f), new Vector3(0f, 0.1f, 0f));
    }



    public override void StartState()
    {

    }
    #endregion
}

//public class BossKhchsingh : Boss
//{
//    #region Variable
//    bool isMadden = false, 
//        nextStateMadden = false;

//    BossKhchsinghState bossKhchsinghState = BossKhchsinghState.Idle;

//    List<Monster> listOrdinatePrefab = new List<Monster>(2);

//    float normalSpeed = 5f,
//        fasterSpeed = 7.5f,
//        dashSpeed = 75f;

//    float percentMove = 50f;

//    List<BossKhchsinghState> listNearRangeState = new List<BossKhchsinghState>(),
//        listMiddleRangeState = new List<BossKhchsinghState>(),
//        listFarRangeState = new List<BossKhchsinghState>();

//    float minimumDmgMultiply = 0.9f,
//        maximumDmgMultiply = 1.1f;

//    bool isImmortal = false;

//    GameObject maddenSparkGameObjectParticle,
//        maddenReleaseGameObjectParticle,
//        chargeGameObjectParticle,
//        fallGameObjectParticle,
//        thrashTrunkHitGameObjectParticle,
//        treeBrokeGameObjectParticle,
//        groundSlamGameObjectParticle,
//        ultimateGameObjectParticle;

//    GameObject treeForThrowGameObject;

//    #endregion

//    #region Properties
//    public override int Hp
//    {
//        get
//        {
//            return base.Hp;
//        }
//        set
//        {
//            base.Hp = value;
//            if (!isMadden)
//                nextStateMadden = Hp <= MaxHp * 0.1f;
//        }
//    }
//    #endregion

//    #region EnumType
//    enum BossKhchsinghState
//    {
//        Idle,
//        Move,
//        Madden,
//        CallOrdinate,
//        ThrashTrunk3Time,
//        ChargeToDash,
//        Trample,
//        ThrowATree,
//        IvoryBeam
//    }
//    #endregion

//    // Use this for initialization
//    protected override void Start()
//    {
//        MaxHp = 10000;
//        base.Start();

//        SetLocalPositionReceiveDamageParticle(new Vector3(0f, 4f, 4.75f),
//            new Vector3(0f, 4f, 4.75f), new Vector3(0f, 0.1f, 0f));

//        speed = normalSpeed;

//        listNearRangeState.Add(BossKhchsinghState.ThrashTrunk3Time);
//        listNearRangeState.Add(BossKhchsinghState.Trample);

//        listMiddleRangeState.Add(BossKhchsinghState.ChargeToDash);
//        listMiddleRangeState.Add(BossKhchsinghState.Trample);
//        listMiddleRangeState.Add(BossKhchsinghState.ThrowATree);

//        listFarRangeState.Add(BossKhchsinghState.ChargeToDash);
//        listFarRangeState.Add(BossKhchsinghState.ThrowATree);

//        thisAnimation["Walk"].speed = 1.25f;

//        string particlePrefabPath = "Prefabs/Particle/Enemy/Boss01/";

//        maddenSparkGameObjectParticle = Instantiate(Resources.Load(particlePrefabPath + "Boss1BerserkBuff2")) as GameObject;
//        maddenReleaseGameObjectParticle = Instantiate(Resources.Load(particlePrefabPath + "Boss1BeserkRelease")) as GameObject;
//        chargeGameObjectParticle = Instantiate(Resources.Load(particlePrefabPath + "Boss1Charge")) as GameObject;
//        fallGameObjectParticle = Instantiate(Resources.Load(particlePrefabPath + "Boss1Fall")) as GameObject;
//        thrashTrunkHitGameObjectParticle = Instantiate(Resources.Load(particlePrefabPath + "Boss1Hit")) as GameObject;
//        treeBrokeGameObjectParticle = Instantiate(Resources.Load(particlePrefabPath + "Boss1Treehit")) as GameObject;
//        groundSlamGameObjectParticle = Instantiate(Resources.Load(particlePrefabPath + "Boss1GroundSlam")) as GameObject;
//        ultimateGameObjectParticle = Instantiate(Resources.Load(particlePrefabPath + "Boss1Ultimate")) as GameObject;

//        treeForThrowGameObject = Instantiate(Resources.Load("Prefabs/Monsters/Scene1/TreeForThrow")) as GameObject;

//        maddenSparkGameObjectParticle.SetActive(false);
//        maddenReleaseGameObjectParticle.SetActive(false);
//        chargeGameObjectParticle.SetActive(false);
//        fallGameObjectParticle.SetActive(false);
//        thrashTrunkHitGameObjectParticle.SetActive(false);
//        treeBrokeGameObjectParticle.SetActive(false);
//        groundSlamGameObjectParticle.SetActive(false);
//        ultimateGameObjectParticle.SetActive(false);

//        treeForThrowGameObject.SetActive(false);

//        //StartCoroutine(WaitTimeStart(1.5f));
//    }

//    public override void ReceiveDamage(int dmg)
//    {
//        if (!isImmortal)
//            base.ReceiveDamage(dmg);
//    }

//    public override void StartState()
//    {
//        print("StartState");
//        if (nextStateMadden)
//            bossKhchsinghState = BossKhchsinghState.Madden;
//        else
//        {
//            if (OftenMethod.RandomPercent(percentMove))
//                StartCoroutine(Move());
//            else
//                RandomBossKhchsinghStateFromRange();
//        }
//    }

//    protected override IEnumerator BossFSM()
//    {
//        for (; ; )
//            yield return StartCoroutine(bossKhchsinghState.ToString());
//    }

//    IEnumerator WaitTimeStart(float timeWaiting)
//    {
//        yield return new WaitForSeconds(timeWaiting);
//        bossKhchsinghState = BossKhchsinghState.CallOrdinate;
//        //bossKhchsinghState = BossKhchsinghState.Madden;
//        StartCoroutine(BossFSM());
//    }

//    IEnumerator Move()
//    {
//        percentMove -= 10f;
//        //switch (thisRangeType)
//        //{
//        //    case RangeType.Near:
//        //        thisAnimation.CrossFade("Jump");
//        //        isImmortal = true;
//        //        thisRigidbody.AddForce(0, 500000f, 500000f);
//        //        yield return new WaitForSeconds(0.5f);

//        //        while (thisTransform.position.y > 0.125f)
//        //            yield return null;

//        //        isImmortal = false;
//        //        ReuseGameObject(fallGameObjectParticle, new Vector3(0f, 0.1f, 0f), false);

//        //        thisRangeType = RangeType.Far;
//        //        break;
//        //    case RangeType.Middle:
//        //        {
//        //            thisAnimation.CrossFade("Walk");
//        //            while (thisTransform.position.z > nearPoint)
//        //            {
//        //                thisTransform.Translate(Vector3.forward * speed * Time.deltaTime);
//        //                yield return null;
//        //            }

//        //            thisRangeType = RangeType.Near;
//        //            break;
//        //        }
//        //    case RangeType.Far:
//        //        {
//        //            thisAnimation.CrossFade("Walk");
//        //            while (thisTransform.position.z > middlePoint)
//        //            {
//        //                thisTransform.Translate(Vector3.forward * speed * Time.deltaTime);
//        //                yield return null;
//        //            }

//        //            thisRangeType = RangeType.Middle;
//        //            break;
//        //        }
//        //}
//        EndTurn();
//        yield return null;
//    }

//    #region Attack Method
//    IEnumerator Madden()
//    {
//        thisAnimation.CrossFade(bossKhchsinghState.ToString());

//        yield return new WaitForSeconds(2.78f);
//        ReuseGameObject(maddenReleaseGameObjectParticle, new Vector3(0f, 0.1f, 0f), true);

//        while (thisAnimation.isPlaying)
//            yield return null;

//        ReuseGameObject(maddenSparkGameObjectParticle, Vector3.zero, true);
//        transform.FindChild("Cube_001_Cube_001").renderer.material.color = new Color(0.8f, 0.5f, 0.5f);

//        isMadden = true;
//        nextStateMadden = false;

//        speed = fasterSpeed;

//        thisAnimation["Idle"].speed = 1.75f;
//        thisAnimation["Walk"].speed = 2f;
//        thisAnimation["CallOrdinate"].speed = 1.75f;
//        thisAnimation["ThrashTrunk3Time"].speed = 1.75f;
//        thisAnimation["ChargeToDash"].speed = 1.75f;
//        thisAnimation["Trample"].speed = 1.75f;
//        thisAnimation["ThrowATree"].speed = 1.75f;

//        listFarRangeState.Add(BossKhchsinghState.IvoryBeam);

//        EndTurn();
//    }

//    IEnumerator CallOrdinate()
//    {
//        thisAnimation.CrossFade(bossKhchsinghState.ToString());

//        yield return new WaitForSeconds(isMadden ? 2.85f / 1.75f : 2.85f);

//        int ordinateNumber = isMadden ? 4 : 2;
//        for (int i = 0; i < ordinateNumber; i++)
//        {
//            Monster monster = (Instantiate(listOrdinatePrefab[i % 2],
//                new Vector3(((i + 0.5f) - ordinateNumber * 0.5f) * 7.5f, 1f, 10f),
//                Quaternion.AngleAxis(180, Vector3.up))) as Monster;
//        }
//        EndTurn();
//    }

//    IEnumerator ThrashTrunk3Time()
//    {
//        thisAnimation.CrossFade(bossKhchsinghState.ToString());

//        yield return new WaitForSeconds(isMadden ? 1.167f / 1.75f : 1.167f);
//        ReuseGameObject(thrashTrunkHitGameObjectParticle, new Vector3(0f, 1.25f, 7f), false);
//        CharacterController.ReceiveDamage(DamageProbabilityDistribution(atkValue * 0.5f, minimumDmgMultiply, maximumDmgMultiply));

//        yield return new WaitForSeconds(isMadden ? 1.25f / 1.75f : 1.25f);
//        ReuseGameObject(thrashTrunkHitGameObjectParticle, new Vector3(0f, 1.25f, 7f), false);
//        CharacterController.ReceiveDamage(DamageProbabilityDistribution(atkValue * 0.5f, minimumDmgMultiply, maximumDmgMultiply));

//        yield return new WaitForSeconds(isMadden ? 1.417f / 1.75f : 1.417f);
//        ReuseGameObject(thrashTrunkHitGameObjectParticle, new Vector3(0f, 1.25f, 7f), false);
//        CharacterController.ReceiveDamage(DamageProbabilityDistribution(atkValue * 1.5f, minimumDmgMultiply, maximumDmgMultiply));
//        magicFieldController.RotateMagicCircle(2, -2);

//        EndTurn();
//    }

//    IEnumerator ChargeToDash()
//    {
//        thisAnimation.CrossFade(bossKhchsinghState.ToString());

//        yield return new WaitForSeconds(isMadden ? 2.8f / 1.75f : 2.8f);

//        ReuseGameObject(chargeGameObjectParticle, new Vector3(0f, 3f, 7f), true);

//        Vector3 targetMove = new Vector3(0f, 1f, -35f);
//        thisRigidbody.useGravity = false;
//        thisRigidbody.velocity = new Vector3(0f, 0f, -dashSpeed);

//        while (thisTransform.position.z > -2.5f)
//            yield return null;

//        CharacterController.ReceiveDamage(DamageProbabilityDistribution(atkValue * 2.5f, minimumDmgMultiply, maximumDmgMultiply));

//        while (thisTransform.position.z > targetMove.z)
//            yield return null;

//        thisRigidbody.velocity = Vector3.zero;
//        isImmortal = true;
//        yield return new WaitForSeconds(1f);
//        thisTransform.position = targetMove;

//        thisAnimation.CrossFade("Jump");
//        thisRigidbody.AddForce(0, 750000f, 1000000f);
//        thisRigidbody.useGravity = true;
//        yield return new WaitForSeconds(1f);

//        while (thisTransform.position.y > 0.1f)
//            yield return null;

//        ReuseGameObject(fallGameObjectParticle, new Vector3(0f, 0.1f, 0f), false);

//        isImmortal = false;

//        //thisRangeType = RangeType.Far;
//        EndTurn();
//    }

//    IEnumerator Trample()
//    {
//        thisAnimation.CrossFade(bossKhchsinghState.ToString());

//        yield return new WaitForSeconds(isMadden ? 1.5f / 1.75f : 1.5f);

//        ReuseGameObject(groundSlamGameObjectParticle, new Vector3(0f, 0f, 4f), false);

//        yield return new WaitForSeconds(isMadden ? 0.75f / 1.75f : 0.75f);

//        CharacterController.ReceiveDamage(DamageProbabilityDistribution(atkValue * 1f, minimumDmgMultiply, maximumDmgMultiply));
//        magicFieldController.ResetMagicPointIsSelected();

//        EndTurn();
//    }

//    IEnumerator ThrowATree()
//    {
//        thisAnimation.CrossFade(bossKhchsinghState.ToString());

//        yield return new WaitForSeconds(isMadden ? 2.75f / 1.75f : 2.75f);

//        ReuseGameObject(treeForThrowGameObject, new Vector3(0f, 3f, 5f), false);
//        treeForThrowGameObject.rigidbody.velocity = Vector3.back * 50f;
//        treeForThrowGameObject.rigidbody.angularVelocity = Vector3.right * 75f;

//        while (treeForThrowGameObject.transform.position.z > 0f)
//            yield return null;
//        treeForThrowGameObject.SetActive(false);
//        ReuseGameObject(treeBrokeGameObjectParticle, Vector3.zero, false, treeForThrowGameObject.transform);

//        CharacterController.ReceiveDamage(DamageProbabilityDistribution(atkValue * 2f, minimumDmgMultiply, maximumDmgMultiply));

//        RandomCharacterFall(1);

//        while (thisAnimation.isPlaying)
//            yield return null;

//        EndTurn();
//    }

//    IEnumerator IvoryBeam()
//    {
//        thisAnimation.CrossFade(bossKhchsinghState.ToString());

//        ReuseGameObject(ultimateGameObjectParticle, new Vector3(0f, 2.55f, 7.5f), true);

//        //yield return new WaitForSeconds(4.333f);

//        yield return new WaitForSeconds(2.75f);

//        for (int i = 0; i < 10; i++)
//        {
//            yield return new WaitForSeconds(0.075f);
//            CharacterController.ReceiveDamage(DamageProbabilityDistribution(atkValue * 0.5f, minimumDmgMultiply, maximumDmgMultiply));
//        }

//        EndTurn();
//    }
//    #endregion

//    void RandomBossKhchsinghStateFromRange()
//    {
//        percentMove += 10f;
//        List<BossKhchsinghState> listRangeState;

//        //switch(thisRangeType)
//        //{
//        //    case RangeType.Near:
//        //        listRangeState = listNearRangeState;
//        //        break;
//        //    case RangeType.Middle:
//        //        listRangeState = listMiddleRangeState;
//        //        break;
//        //    default:
//        //        listRangeState = listFarRangeState;
//        //        break;
//        //}

//        //bossKhchsinghState = listRangeState[Random.Range(0, listRangeState.Count)];
//        StartCoroutine(bossKhchsinghState.ToString());
//        //StartCoroutine(RemoveAndReAddStateToListRange(listRangeState, bossKhchsinghState));
//    }

//    IEnumerator RemoveAndReAddStateToListRange(List<BossKhchsinghState> listRangeState, BossKhchsinghState bossKhchsinghState)
//    {
//        if (bossKhchsinghState != BossKhchsinghState.ChargeToDash)
//        {
//            string bossKhchsinghStateName = bossKhchsinghState.ToString();
//            listRangeState.Remove(bossKhchsinghState);
//            yield return new WaitForSeconds(isMadden ?
//                (thisAnimation[bossKhchsinghStateName].length + 2.5f) / 1.75f : thisAnimation[bossKhchsinghStateName].length + 2.5f);
//            listRangeState.Add(bossKhchsinghState);
//        }
//        else
//        {
//            listMiddleRangeState.Remove(BossKhchsinghState.ChargeToDash);
//            listFarRangeState.Remove(BossKhchsinghState.ChargeToDash);
//            yield return new WaitForSeconds(10f);
//            listMiddleRangeState.Add(BossKhchsinghState.ChargeToDash);
//            listFarRangeState.Add(BossKhchsinghState.ChargeToDash);
//        }
//    }

//    void OnGUI()
//    {
//        GUI.TextArea(new Rect(25f, 25f, 100f, 40f), Hp.ToString());
//    }
//}
