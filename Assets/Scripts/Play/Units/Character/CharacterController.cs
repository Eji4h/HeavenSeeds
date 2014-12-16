using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : Unit
{
    #region Static Variable
    static bool actionIsUpdate = false;
    static int cost;
    static int swordCost,
        bowCost,
        wandCost,
        shieldCost,
        scrollCost,
        lowestCost;

    static int sumHp, maxSumHp,
        barrierHp, barrierTurn;

    static float blockPercentPerTimeDefence;
    static float atkPercentIncrease,
        barrierHpPercentIncrease,
        healPercentIncrease;

    static GameObject barrierGameObject, 
        healGameObject;
    static Vector3 barrierHpPopUpPos;
    static Color32 barrierHpPopUpColor = new Color32(90, 228, 255, 255);

    static bool isBurn,
        isPoison,
        isStun, 
        isFreeze;
    static int burnTurn,
        poisonTurn,
        stunTurn,
        freezeTurn;
    #endregion

    #region Static Properties
    public static bool ActionIsUpdate
    {
        get { return CharacterController.actionIsUpdate; }
    }
    public static int Cost
    {
        get { return CharacterController.cost / randomNumSecurity; }
        set 
        {
            if (value > 99)
                value = 99;
            CharacterController.cost = value * randomNumSecurity;
            UIController.ManaCost = Cost;
        }
    }
    public static int SwordCost
    {
        get { return CharacterController.swordCost; }
    }

    public static int BowCost
    {
        get { return CharacterController.bowCost; }
    }

    public static int WandCost
    {
        get { return CharacterController.wandCost; }
    }

    public static int ShieldCost
    {
        get { return CharacterController.shieldCost; }
    }

    public static int ScrollCost
    {
        get { return CharacterController.scrollCost; }
    }

    public static int LowestCost
    {
        get { return CharacterController.lowestCost; }
    }

    static int SumHp
    {
        get { return CharacterController.sumHp / randomNumSecurity; }
        set 
        {
            if (value > MaxSumHp)
                CharacterController.sumHp = MaxSumHp * randomNumSecurity;
            else
                CharacterController.sumHp = value * randomNumSecurity;

            UIController.PlayerHpBar.Value = SumHp;

            if (CharacterController.SumHp <= 0)
                ;
        }
    }

    static int MaxSumHp
    {
        get { return CharacterController.maxSumHp / randomNumSecurity; }
        set { CharacterController.maxSumHp = value * randomNumSecurity; }
    }

    static int BarrierHp
    {
        get { return CharacterController.barrierHp / randomNumSecurity; }
        set
        {
            CharacterController.barrierHp = value * randomNumSecurity;
            barrierGameObject.SetActive(barrierTurn-- > 0 && BarrierHp > 0);
        }
    }

    static float BlockPercentPerTimeDefence
    {
        get { return CharacterController.blockPercentPerTimeDefence / randomNumSecurity; }
        set { CharacterController.blockPercentPerTimeDefence = value * randomNumSecurity; }
    }

    static float AtkPercentIncrease
    {
        get { return CharacterController.atkPercentIncrease / randomNumSecurity; }
        set { CharacterController.atkPercentIncrease = value * randomNumSecurity; }
    }
    public static float GetAtkPercentIncrease
    {
        get { return CharacterController.atkPercentIncrease / randomNumSecurity; }
    }

    static float BarrierHpPercentIncrease
    {
        get { return CharacterController.barrierHpPercentIncrease / randomNumSecurity; }
        set { CharacterController.barrierHpPercentIncrease = value * randomNumSecurity; }
    }

    static float HealPercentIncrease
    {
        get { return CharacterController.healPercentIncrease / randomNumSecurity; }
        set { CharacterController.healPercentIncrease = value * randomNumSecurity; }
    }

    public static float GetHealPercentIncrease
    {
        get { return CharacterController.healPercentIncrease / randomNumSecurity; }
    }

    public static int SwordValue
    {
        get { return swordCharacterController.characterStatus.SwordValue; }
    }

    public static int BowValue
    {
        get { return bowCharacterController.characterStatus.BowValue; }
    }

    public static int WandValue
    {
        get { return wandCharacterController.characterStatus.WandValue; }
    }

    public static int ShieldValue
    {
        get { return shieldCharacterController.characterStatus.ShiedlValue; }
    }

    public static int ScrollValue
    {
        get { return scrollCharacterController.characterStatus.ScrollValue; }
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

    public static int BurnTurn
    {
        get { return burnTurn; }
        set
        {
            burnTurn = value;
            isBurn = burnTurn > 0;
        }
    }

    public static int PoisonTurn
    {
        get { return poisonTurn; }
        set 
        {
            poisonTurn = value;
            isPoison = poisonTurn > 0;
        }
    }

    public static int StunTurn
    {
        get { return stunTurn; }
        set
        {
            stunTurn = value;
            isStun = stunTurn > 0;
        }
    }

    public static int FreezeTurn
    {
        get { return freezeTurn; }
        set
        {
            freezeTurn = value;
            isFreeze = freezeTurn > 0;
        }
    }
    #endregion

    #region Static Method
    public static new void SetInit()
    {
        swordCharacterController = SceneController.SwordCharacterController;
        bowCharacterController = SceneController.BowCharacterController;
        wandCharacterController = SceneController.WandCharacterController;
        shieldCharacterController = SceneController.ShieldCharacterController;
        scrollCharacterController = SceneController.ScrollCharacterController;

        MaxSumHp = swordCharacterController.MaxHp +
            bowCharacterController.MaxHp +
            wandCharacterController.MaxHp +
            shieldCharacterController.MaxHp +
            scrollCharacterController.MaxHp;

        UIController.PlayerHpBar.MaxValue = MaxSumHp;
        SumHp = MaxSumHp;

        listCharacterController.Clear();
        listCharacterController.Add(swordCharacterController);
        listCharacterController.Add(bowCharacterController);
        listCharacterController.Add(wandCharacterController);
        listCharacterController.Add(shieldCharacterController);
        listCharacterController.Add(scrollCharacterController);

        barrierGameObject = Instantiate(Resources.Load("Prefabs/Particle/Player/Attack/NewBarrier"), 
            Vector3.forward * 3.5f, Quaternion.identity) as GameObject;
        barrierHpPopUpPos = barrierGameObject.transform.position + Vector3.up * 3f;
        barrierGameObject.SetActive(false);

        healGameObject = Instantiate(Resources.Load("Prefabs/Particle/Heal")) as GameObject;
        healGameObject.transform.position = new Vector3(0f, 7f, 0f);
        healGameObject.SetActive(false);

        Cost = PlayerPrefs.GetInt("startCost", 15);
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
        }
        if (dmg > 0)
        {
            SumHp -= dmg;
            UIController.ShowHpPopUp(dmg, swordCharacterController.thisTransform.position, true);
            listCharacterController.ForEach(characterController =>
                {
                    characterController.thisAnimation.Stop();
                    characterController.thisAnimation.Play(characterController.hurtStr);
                    characterController.thisAnimation.CrossFadeQueued(characterController.IsFall ?
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
        UIController.ShowHpPopUp(heal, swordCharacterController.thisTransform.position, false);
    }

    static void ShowHealParticle()
    {
        healGameObject.SetActive(false);
        healGameObject.SetActive(true);
    }

    public static void BarrierDefence(int barrierHp, float blockPercentPerTimeDefence)
    {
        barrierTurn = 10;
        BarrierHp = Mathf.RoundToInt(barrierHp * (1f + BarrierHpPercentIncrease));
        BlockPercentPerTimeDefence = blockPercentPerTimeDefence;
    }

    public static void GetBuff(float atkPercentIncrease, 
        float barrierHpPercentIncrease, 
        float healPercentIncrease)
    {
        AtkPercentIncrease = atkPercentIncrease;
        BarrierHpPercentIncrease = barrierHpPercentIncrease;
        HealPercentIncrease = healPercentIncrease;

        if (BarrierHp > 0)
            BarrierHp *= Mathf.RoundToInt(1f + BarrierHpPercentIncrease);
    }

    public static void ClearDebuff()
    {
        BurnTurn = 0;
        PoisonTurn = 0;
        listCharacterControllerIsFall.ForEach(character =>
            character.IsFall = false);
        magicFieldController.RandomChaActionStateCount = 0;
        magicFieldController.listMagicPoints.ForEach(magicPoint =>
                magicPoint.IsSkull = false);
    }

    public static void ClearBuff()
    {
        AtkPercentIncrease = 0f;
        BarrierHpPercentIncrease = 0f;
        HealPercentIncrease = 0f;
    }

    public static void TurnEffectDecrease()
    {
        ClearBuff();

        int listCharacterControllerIsFallCount = listCharacterControllerIsFall.Count;

        for(int i = 0; i < listCharacterControllerIsFall.Count; i++)
        {
            print(listCharacterControllerIsFall[i].name);
            listCharacterControllerIsFall[i].TurnFall--;
            if (listCharacterControllerIsFall.Count != listCharacterControllerIsFallCount)
            {
                i--;
                listCharacterControllerIsFallCount = listCharacterControllerIsFall.Count;
            }
        }
    }
    #endregion

    #region Variable
    CharacterStatus characterStatus;
    GameObject weaponGameObject;
    CharacterActionState chaActionState;
    string actionStr,
        idleStr,
        hurtStr, 
        fallStr;

    IActionBehaviour iActionBehaviour;

    //Action Time
    float timeBeforeMonsterListShowParticleReceiveDamage,
        timeAfterMonsterListShowParticleReceiveDamage;

    bool isFall = false;
    int turnFall = 0;
    #endregion

    #region Properties
    public bool IsFall
    {
        get { return isFall; }
        set
        {
            isFall = value;
            if (isFall)
            {
                listCharacterControllerIsFall.Add(this);
                thisAnimation.CrossFade(fallStr);
            }
            else
            {
                listCharacterControllerIsFall.Remove(this);
                thisAnimation.CrossFade(idleStr);
            }
        }
    }

    public int TurnFall
    {
        get { return turnFall; }
        set
        {
            turnFall = value;
            if (turnFall <= 0)
                IsFall = false;
            print(name + ", " + turnFall);
        }
    }
    #endregion
    public void SetStatus()
    {
        switch (chaActionState)
        {
            case CharacterActionState.SwordAction:
                {
                    AttackWeapon attackWeapon = weaponGameObject.GetComponent<AttackWeapon>();
                    iActionBehaviour = new AttackBehaviour(characterStatus.SwordValue,
                        attackWeapon.minimumDmgMultiply, attackWeapon.maximumDmgMultiply);
                    swordCost = 8 + characterStatus.SwordCostChange;
                    lowestCost = swordCost;
                }
                break;
            case CharacterActionState.BowAction:
                {
                    AttackWeapon attackWeapon = weaponGameObject.GetComponent<AttackWeapon>();
                    iActionBehaviour = new AttackBehaviour(characterStatus.BowValue,
                        attackWeapon.minimumDmgMultiply, attackWeapon.maximumDmgMultiply);
                    bowCost = 9 + characterStatus.BowCostChange;
                }
                break;
            case CharacterActionState.WandAction:
                {
                    AttackWeapon attackWeapon = weaponGameObject.GetComponent<AttackWeapon>();
                    iActionBehaviour = new AttackBehaviour(characterStatus.WandValue,
                        attackWeapon.minimumDmgMultiply, attackWeapon.maximumDmgMultiply);
                    wandCost = 7 + characterStatus.WandCostChange;
                }
                break;
            case CharacterActionState.ShieldAction:
                {
                    DefenceWeapon defenceWeapon = weaponGameObject.GetComponent<DefenceWeapon>();
                    iActionBehaviour = new DefenceBehaviour(defenceWeapon.barrierHp,
                        characterStatus.ShiedlValue * 0.0025f);
                    shieldCost = 20 + characterStatus.ShieldCostChange;
                }
                break;
            case CharacterActionState.ScrollAction:
                {
                    HealAndBuffWeapon healAndBuffWeapon = weaponGameObject.GetComponent<HealAndBuffWeapon>();
                    iActionBehaviour = new HealAndBuffBehaviour(characterStatus.ScrollValue,
                        healAndBuffWeapon.atkPercentIncrease, healAndBuffWeapon.barrierHpPercentIncrease,
                        healAndBuffWeapon.healPercentIncrease);
                    scrollCost = 5 + characterStatus.ScrollCostChange;
                }
                break;
        }
        MaxHp = characterStatus.Hp;
    }

    protected override void Awake()
    {
        characterStatus = GetComponent<CharacterStatus>();
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        thisAnimation.Play(idleStr);
        base.Start();
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

    public void Action()
    {
        StartCoroutine(UpdateAction());
    }

    IEnumerator UpdateAction()
    {
        if (!IsFall)
        {
            actionIsUpdate = true;
            thisAnimation.Play(actionStr);

            yield return new WaitForSeconds(timeBeforeMonsterListShowParticleReceiveDamage);
            if (iActionBehaviour is AttackBehaviour)
                monster.ShowParticleReceiveDamage(chaActionState);
            else if (iActionBehaviour is HealAndBuffBehaviour)
                ShowHealParticle();
            yield return new WaitForSeconds(timeAfterMonsterListShowParticleReceiveDamage);

            iActionBehaviour.Action();

            while (thisAnimation.isPlaying)
                yield return null;
            thisAnimation.Play(idleStr);

            while (Monster.QueueElementIsRunning)
                yield return null;

            while (Monster.NowBurning)
                yield return null;

            if(isBurn)
            {
                ReceiveDamageByPercentOfSumMaxHp(4f);
                yield return new WaitForSeconds(1f);
                BurnTurn--;
            }

            if(isPoison)
            {
                ReceiveDamageByPercentOfSumMaxHp(4f);
                yield return new WaitForSeconds(1f);
                PoisonTurn--;
            }

            actionIsUpdate = false;
        }
        turnController.CharacterActionEnd();
    }

    public void SetToFall(int turnFall)
    {
        IsFall = true;
        TurnFall = turnFall;
    }
}
