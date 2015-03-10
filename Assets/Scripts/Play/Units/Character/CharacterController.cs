using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : Unit
{
    static int sumHp, maxSumHp,
        barrierHp, barrierTurn;

    static float blockPercentPerTimeDefence;
    static float atkPercentIncrease,
        barrierHpPercentIncrease,
        healPercentIncrease;

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
    static int burnTurn,
        poisonTurn,
        stunTurn,
        freezeTurn;

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
                ;
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

    static int BarrierTurn
    {
        get { return CharacterController.barrierTurn; }
        set 
        {
            CharacterController.barrierTurn = value;
            CheckBarrierActive();
        }
    }

    static void CheckBarrierActive()
    {
        barrierGameObject.SetActive(BarrierTurn > 0 && BarrierHp > 0);
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

    public static float GetHealPercentIncrease
    {
        get { return CharacterController.healPercentIncrease / NumberSecurity.RandomNumSecurity; }
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
        get { return burnTurn / NumberSecurity.RandomNumSecurity; }
        set
        {
            burnTurn = value * NumberSecurity.RandomNumSecurity;
            isBurn = BurnTurn > 0;
        }
    }

    public static int PoisonTurn
    {
        get { return poisonTurn / NumberSecurity.RandomNumSecurity; }
        set 
        {
            poisonTurn = value * NumberSecurity.RandomNumSecurity;
            isPoison = PoisonTurn > 0;
        }
    }

    public static int StunTurn
    {
        get { return stunTurn / NumberSecurity.RandomNumSecurity; }
        set
        {
            stunTurn = value * NumberSecurity.RandomNumSecurity;
            isStun = StunTurn > 0;
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
        }
        if (dmg > 0)
        {
            SumHp -= dmg;
            UIController.ShowHpPopUp(dmg, swordCharacterController.thisTransform.position, receiveDamageHpPopUpColor);
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
        UIController.ShowHpPopUp(heal, swordCharacterController.thisTransform.position, healHpPopUpColor);
    }

    static void ShowHealParticle()
    {
        healGameObject.SetActive(false);
        healGameObject.SetActive(true);
    }

    public static void BarrierDefence(int barrierHp, float blockPercentPerTimeDefence)
    {
        BarrierTurn = 3;
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
        int listCharacterControllerIsFallCount = listCharacterControllerIsFall.Count;

        for (int i = 0; i < listCharacterControllerIsFall.Count; i++)
        {
            listCharacterControllerIsFall[i].TurnFall = 0;
            if (listCharacterControllerIsFall.Count != listCharacterControllerIsFallCount)
            {
                i--;
                listCharacterControllerIsFallCount = listCharacterControllerIsFall.Count;
            }
        }
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
            listCharacterControllerIsFall[i].TurnFall--;
            if (listCharacterControllerIsFall.Count != listCharacterControllerIsFallCount)
            {
                i--;
                listCharacterControllerIsFallCount = listCharacterControllerIsFall.Count;
            }
        }

        BarrierTurn--;
    }

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
        }
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
            thisAnimation.Play(actionStr);

            yield return new WaitForSeconds(timeBeforeMonsterListShowParticleReceiveDamage);
            if (iActionBehaviour is AttackBehaviour)
                SceneController.CurrentMonster.ShowParticleReceiveDamage(chaActionState);
            else if (iActionBehaviour is HealAndBuffBehaviour)
                ShowHealParticle();
            yield return new WaitForSeconds(timeAfterMonsterListShowParticleReceiveDamage);

            iActionBehaviour.Action();

            while (thisAnimation.isPlaying)
                yield return null;
            thisAnimation.Play(idleStr);
        }

        while (SceneController.CurrentMonster.QueueElementIsRunning)
            yield return null;

        while (SceneController.CurrentMonster.NowBurning)
            yield return null;

        if (isBurn)
        {
            ReceiveDamageByPercentOfSumMaxHp(4f);
            yield return new WaitForSeconds(1f);
            BurnTurn--;
        }

        if (isPoison)
        {
            ReceiveDamageByPercentOfSumMaxHp(4f);
            yield return new WaitForSeconds(1f);
            PoisonTurn--;
        }
    }

    public void SetToFall(int turnFall)
    {
        IsFall = true;
        TurnFall = turnFall;
    }
}
