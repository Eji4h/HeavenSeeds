using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : Unit
{
    static int sumHp, maxSumHp,
        barrierHp, barrierRound;

    static float blockPercentPerTimeDefence;
    static float atkPercentIncrease,
        barrierHpPercentIncrease,
        healPercentIncrease;
    static float buffTime;

    static GameObject barrierGameObject, 
        healGameObject;
    static Vector3 barrierHpPopUpPos;
    static Color32 receiveDamageHpPopUpColor = new Color32(208, 60, 224, 255),
        healHpPopUpColor = new Color32(0, 220, 0, 255),
        barrierHpPopUpColor = new Color32(254, 250, 94, 255);

    static bool isBurn,
        isPoison,
        isStun, 
        isFreeze;
    static float burnTime,
        poisonTime,
        stunTime;

    static int freezeTurn;

    static int SumHp
    {
        get { return CharacterController.sumHp / NumberSecurity.RandomNumSecurity; }
        set 
        {
            if (value > MaxSumHp)
                CharacterController.sumHp = MaxSumHp * NumberSecurity.RandomNumSecurity;
            else
                CharacterController.sumHp = value * NumberSecurity.RandomNumSecurity;

            UIController.PlayerHpBar.Value = SumHp;

            if (CharacterController.SumHp <= 0)
                SceneController.StopGame();
        }
    }

    static int MaxSumHp
    {
        get { return CharacterController.maxSumHp / NumberSecurity.RandomNumSecurity; }
        set { CharacterController.maxSumHp = value * NumberSecurity.RandomNumSecurity; }
    }

    static int BarrierHp
    {
        get { return CharacterController.barrierHp / NumberSecurity.RandomNumSecurity; }
        set
        {
            CharacterController.barrierHp = value * NumberSecurity.RandomNumSecurity;
            CheckBarrierActive();
        }
    }

    static int BarrierRound
    {
        get { return CharacterController.barrierRound; }
        set 
        {
            CharacterController.barrierRound = value;
            CheckBarrierActive();
        }
    }

    static void CheckBarrierActive()
    {
        barrierGameObject.SetActive(BarrierRound > 0 && BarrierHp > 0);
    }

    static float BlockPercentPerTimeDefence
    {
        get { return CharacterController.blockPercentPerTimeDefence / NumberSecurity.RandomNumSecurity; }
        set { CharacterController.blockPercentPerTimeDefence = value * NumberSecurity.RandomNumSecurity; }
    }

    static float AtkPercentIncrease
    {
        get { return CharacterController.atkPercentIncrease / NumberSecurity.RandomNumSecurity; }
        set { CharacterController.atkPercentIncrease = value * NumberSecurity.RandomNumSecurity; }
    }
    public static float GetAtkPercentIncrease
    {
        get { return CharacterController.atkPercentIncrease / NumberSecurity.RandomNumSecurity; }
    }

    static float BarrierHpPercentIncrease
    {
        get { return CharacterController.barrierHpPercentIncrease / NumberSecurity.RandomNumSecurity; }
        set { CharacterController.barrierHpPercentIncrease = value * NumberSecurity.RandomNumSecurity; }
    }

    static float HealPercentIncrease
    {
        get { return CharacterController.healPercentIncrease / NumberSecurity.RandomNumSecurity; }
        set { CharacterController.healPercentIncrease = value * NumberSecurity.RandomNumSecurity; }
    }

    public float BuffTime
    {
        get { return CharacterController.buffTime; }
        set
        {
            CharacterController.buffTime = value;
            StartCoroutine(WaitToClearBuff());
        }
    }

    public static float GetHealPercentIncrease
    {
        get { return CharacterController.healPercentIncrease / NumberSecurity.RandomNumSecurity; }
    }

    public static int SwordValue
    {
        get { return SwordCharacterController.characterStatus.SwordValue; }
    }

    public static int BowValue
    {
        get { return BowCharacterController.characterStatus.BowValue; }
    }

    public static int WandValue
    {
        get { return WandCharacterController.characterStatus.WandValue; }
    }

    public static int ShieldValue
    {
        get { return ShieldCharacterController.characterStatus.ShiedlValue; }
    }

    public static int ScrollValue
    {
        get { return ScrollCharacterController.characterStatus.ScrollValue; }
    }

    public static bool IsBurn
    {
        get { return isBurn; }
    }

    public static bool IsPoison
    {
        get { return isPoison; }
    }

    public static bool IsStun
    {
        get { return isStun; }
    }

    public static bool IsFreeze
    {
        get { return isFreeze; }
    }

    public static float BurnTime
    {
        get { return burnTime / NumberSecurity.RandomNumSecurity; }
        set
        {
            burnTime = value * NumberSecurity.RandomNumSecurity;
            isBurn = BurnTime > 0f;
        }
    }

    public static float PoisonTime
    {
        get { return poisonTime / NumberSecurity.RandomNumSecurity; }
        set 
        {
            poisonTime = value * NumberSecurity.RandomNumSecurity;
            isPoison = PoisonTime > 0f;
        }
    }

    public static float StunTime
    {
        get { return stunTime / NumberSecurity.RandomNumSecurity; }
        set
        {
            stunTime = value * NumberSecurity.RandomNumSecurity;
            isStun = StunTime > 0f;
        }
    }

    public static int FreezeTurn
    {
        get { return freezeTurn / NumberSecurity.RandomNumSecurity; }
        set
        {
            freezeTurn = value * NumberSecurity.RandomNumSecurity;
            isFreeze = FreezeTurn > 0;
        }
    }

    public static void SetInit()
    {
        MaxSumHp = SwordCharacterController.MaxHp +
            BowCharacterController.MaxHp +
            WandCharacterController.MaxHp +
            ShieldCharacterController.MaxHp +
            ScrollCharacterController.MaxHp;

        UIController.PlayerHpBar.MaxValue = MaxSumHp;
        SumHp = MaxSumHp;

        ListCharacterController.Clear();
        ListCharacterController.Add(SwordCharacterController);
        ListCharacterController.Add(BowCharacterController);
        ListCharacterController.Add(WandCharacterController);
        ListCharacterController.Add(ShieldCharacterController);
        ListCharacterController.Add(ScrollCharacterController);

        barrierGameObject = Instantiate(Resources.Load("Prefabs/Particle/Player/Attack/NewBarrier")) as GameObject;
        barrierGameObject.transform.position = new Vector3(0f, 0f, -1.5f);
        barrierHpPopUpPos = barrierGameObject.transform.position + Vector3.up * 3f;
        barrierGameObject.SetActive(false);

        healGameObject = Instantiate(Resources.Load("Prefabs/Particle/Heal")) as GameObject;
        healGameObject.transform.position = Vector3.zero;
        healGameObject.SetActive(false);
    }

    public static void ReceiveDamage(int dmg)
    {
        if (barrierGameObject.activeInHierarchy)
        {
            int blockDmg = Mathf.RoundToInt(dmg * BlockPercentPerTimeDefence);
            if (BarrierHp > blockDmg)
                BarrierHp -= blockDmg;
            else
            {
                blockDmg = BarrierHp;
                BarrierHp = 0;
            }
            UIController.ShowHpPopUp(blockDmg, barrierHpPopUpPos, barrierHpPopUpColor);
            dmg -= blockDmg;
            BarrierRound--;
        }
        if (dmg > 0)
        {
            SumHp -= dmg;
            UIController.ShowHpPopUp(dmg, SwordCharacterController.ThisTransform.position, receiveDamageHpPopUpColor);
            ListCharacterController.ForEach(characterController =>
                {
                    characterController.ThisAnimation.Stop();
                    characterController.ThisAnimation.Play(characterController.hurtStr);
                    characterController.ThisAnimation.CrossFadeQueued(characterController.IsFall ?
                        characterController.fallStr : characterController.idleStr);
                });
        }
    }

    public static void ReceiveDamageByPercentOfSumMaxHp(float percentOfSumMaxHp)
    {
        ReceiveDamage((int)(percentOfSumMaxHp * SumHp));
    }

    public static void ReceiveHeal(int heal)
    {
        SumHp += Mathf.RoundToInt(heal * (1f + HealPercentIncrease));
        UIController.ShowHpPopUp(heal, SwordCharacterController.ThisTransform.position, healHpPopUpColor);
    }

    static void ShowHealParticle()
    {
        healGameObject.SetActive(false);
        healGameObject.SetActive(true);
    }

    public static void BarrierDefence(int barrierHp, float blockPercentPerTimeDefence)
    {
        BarrierRound = 3;
        BarrierHp = Mathf.RoundToInt(barrierHp * (1f + BarrierHpPercentIncrease));
        BlockPercentPerTimeDefence = blockPercentPerTimeDefence;
    }

    public void GetBuff(float buffTime,
        float atkPercentIncrease, 
        float barrierHpPercentIncrease, 
        float healPercentIncrease)
    {
        BuffTime = buffTime;
        AtkPercentIncrease = atkPercentIncrease;
        BarrierHpPercentIncrease = barrierHpPercentIncrease;
        HealPercentIncrease = healPercentIncrease;

        if (BarrierHp > 0)
            BarrierHp *= Mathf.RoundToInt(1f + BarrierHpPercentIncrease);
    }

    IEnumerator WaitToClearBuff()
    {
        yield return new WaitForSeconds(BuffTime);
        ClearBuff();
    }

    public static void ClearDebuff()
    {
        BurnTime = 0;
        PoisonTime = 0;
        StunTime = 0;
        FreezeTurn = 0;

        ListCharacterController.ForEach(characterController =>
            {
                if (characterController.IsFall)
                    characterController.FallTime = 0f;
            });

        MagicFieldController.RandomChaActionStateCount = 0;
        MagicFieldController.listMagicPoints.ForEach(magicPoint =>
                magicPoint.IsSkull = false);
    }

    public static void ClearBuff()
    {
        AtkPercentIncrease = 0f;
        BarrierHpPercentIncrease = 0f;
        HealPercentIncrease = 0f;
    }

    CharacterStatus characterStatus;
    GameObject weaponGameObject;
    CharacterActionState chaActionState;
    string actionStr,
        idleStr,
        hurtStr, 
        fallStr;

    bool moreThanFullOneGate;
    IActionBehaviour iActionBehaviour;

    //Action Time
    float timeBeforeMonsterListShowParticleReceiveDamage,
        timeAfterMonsterListShowParticleReceiveDamage;

    bool isFall = false;
    float fallTime = 0;

    public bool CanAction
    {
        get { return moreThanFullOneGate; }
    }

    public bool IsFall
    {
        get { return isFall; }
        set
        {
            isFall = value;
            ThisAnimation.CrossFade(isFall ? fallStr : idleStr);
        }
    }

    public float FallTime
    {
        get { return fallTime; }
        set
        {
            fallTime = value;
            bool oldIsFall = isFall;
            IsFall = FallTime > 0f;
            if (!oldIsFall && isFall)
                StartCoroutine(UpdateFallBehaviour());
        }
    }

    IEnumerator UpdateFallBehaviour()
    {
        while(isFall)
        {
            FallTime -= Time.deltaTime;
            yield return null;
        }
        Debug.ClearDeveloperConsole();
    }

    public void SetStatus()
    {
        switch (chaActionState)
        {
            case CharacterActionState.SwordAction:
                {
                    AttackWeapon attackWeapon = weaponGameObject.GetComponent<AttackWeapon>();
                    iActionBehaviour = new AttackBehaviour(characterStatus.SwordValue,
                        attackWeapon.minimumDmgMultiply, attackWeapon.maximumDmgMultiply);
                }
                break;
            case CharacterActionState.BowAction:
                {
                    AttackWeapon attackWeapon = weaponGameObject.GetComponent<AttackWeapon>();
                    iActionBehaviour = new AttackBehaviour(characterStatus.BowValue,
                        attackWeapon.minimumDmgMultiply, attackWeapon.maximumDmgMultiply);
                }
                break;
            case CharacterActionState.WandAction:
                {
                    AttackWeapon attackWeapon = weaponGameObject.GetComponent<AttackWeapon>();
                    iActionBehaviour = new AttackBehaviour(characterStatus.WandValue,
                        attackWeapon.minimumDmgMultiply, attackWeapon.maximumDmgMultiply);
                }
                break;
            case CharacterActionState.ShieldAction:
                {
                    DefenceWeapon defenceWeapon = weaponGameObject.GetComponent<DefenceWeapon>();
                    iActionBehaviour = new DefenceBehaviour(defenceWeapon.barrierHp,
                        characterStatus.ShiedlValue * 0.0025f);
                }
                break;
            case CharacterActionState.ScrollAction:
                {
                    HealAndBuffWeapon healAndBuffWeapon = weaponGameObject.GetComponent<HealAndBuffWeapon>();
                    iActionBehaviour = new HealAndBuffBehaviour(characterStatus.ScrollValue,
                        healAndBuffWeapon.atkPercentIncrease, healAndBuffWeapon.barrierHpPercentIncrease,
                        healAndBuffWeapon.healPercentIncrease);
                }
                break;
        }
        maxHp = characterStatus.Hp;
        MaxGate = characterStatus.MaxGate;
        GateBarRegenFullOneGate = characterStatus.RegenGateRate;
    }

    protected override void Awake()
    {
        characterStatus = GetComponent<CharacterStatus>();
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        ThisAnimation.Play(idleStr);
    }

    public void SetWeapon(CharacterActionState chaActionState, GameObject weaponGameObject)
    {
        this.chaActionState = chaActionState;
        this.weaponGameObject = weaponGameObject;
        
        actionStr = chaActionState.ToString();

        string actionName = actionStr.Substring(0, actionStr.Length - 6);

        idleStr = "Idle" + actionName;
        hurtStr = "Hurt" + actionName;
        fallStr = "Fall" + actionName;

        string leftHandIK = "rig/root/hand_ik_L",
            rightHandIK = "rig/root/hand_ik_R";

        Transform holdHandTransform, 
            weaponTransform = weaponGameObject.transform;

        switch(chaActionState)
        {
            case CharacterActionState.SwordAction:
                holdHandTransform = transform.Find(rightHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.1f;
                weaponTransform.localRotation = Quaternion.Euler(5f, 180f, 0f);

                timeBeforeMonsterListShowParticleReceiveDamage = 1.15f;
                timeAfterMonsterListShowParticleReceiveDamage = 0.1f;
                break;
            case CharacterActionState.BowAction:
                holdHandTransform = transform.Find(leftHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.1f;
                weaponTransform.localRotation = Quaternion.Euler(5f, 0f, 0f);

                timeBeforeMonsterListShowParticleReceiveDamage = 1.15f;
                timeAfterMonsterListShowParticleReceiveDamage = 0.1f;
                break;
            case CharacterActionState.WandAction:
                holdHandTransform = transform.Find(rightHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.1f;
                weaponTransform.localRotation = Quaternion.Euler(5f, 0f, 0f);

                timeBeforeMonsterListShowParticleReceiveDamage = 0.25f;
                timeAfterMonsterListShowParticleReceiveDamage = 1f;
                break;
            case CharacterActionState.ShieldAction:
                holdHandTransform = transform.Find(rightHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.2f;
                weaponTransform.localRotation = Quaternion.Euler(75f, 180f, 90f);

                timeBeforeMonsterListShowParticleReceiveDamage = 0f;
                timeAfterMonsterListShowParticleReceiveDamage = 1f;
                break;
            case CharacterActionState.ScrollAction:
                holdHandTransform = transform.Find(rightHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.1f;
                weaponTransform.localRotation = Quaternion.Euler(-60f, 0f, 105f);

                timeBeforeMonsterListShowParticleReceiveDamage = 0f;
                timeAfterMonsterListShowParticleReceiveDamage = 1f;
                break;
        }
    }

    public override void SetGateBarController(GateBarController gateBarController)
    {
        gateBarController.transform.position = OftenMethod.NGUITargetWorldPoint(
            ThisTransform.position, new Vector2(0f, -0.05f),
            SceneController.MainCamera, SceneController.UICamera);
        gateBarController.GateCountTarget = 1;
        gateBarController.SetCheckGateCountIsTarget(true);
        gateBarController.GateCountTargetAction = SetMoreThanFullOneGateToTrue;
        base.SetGateBarController(gateBarController);
    }

    void SetMoreThanFullOneGateToTrue()
    {
        moreThanFullOneGate = true;
    }

    public void Action()
    {
        StartCoroutine(UpdateAction());
    }

    IEnumerator UpdateAction()
    {
        moreThanFullOneGate = CheckGateCountMoreThanOne(--GateBarController.GateCount);
        if (!IsFall)
        {
            ThisAnimation.Play(actionStr);

            yield return new WaitForSeconds(timeBeforeMonsterListShowParticleReceiveDamage);
            if (iActionBehaviour is AttackBehaviour)
                SceneController.ChooseMonster.ShowParticleReceiveDamage(chaActionState);
            else if (iActionBehaviour is HealAndBuffBehaviour)
                ShowHealParticle();
            yield return new WaitForSeconds(timeAfterMonsterListShowParticleReceiveDamage);

            iActionBehaviour.Action();

            while (ThisAnimation.isPlaying)
                yield return null;
            ThisAnimation.Play(idleStr);
        }

        if (isBurn)
        {
            ReceiveDamageByPercentOfSumMaxHp(4f);
            yield return new WaitForSeconds(1f);
            BurnTime--;
        }

        if (isPoison)
        {
            ReceiveDamageByPercentOfSumMaxHp(4f);
            yield return new WaitForSeconds(1f);
            PoisonTime--;
        }
    }

    public void SetToFall(float fallTime)
    {
        FallTime = fallTime;
    }
}
