using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : Unit
{
    #region Static Variable
    static int sumHp, maxSumHp,
        barrierHp;
    static float blockPercentPerTimeDefence;
    static float atkPercentIncrease,
        barrierHpPercentIncrease,
        healPercentIncrease;
    #endregion

    #region Static Properties
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
        set { CharacterController.barrierHp = value * randomNumSecurity; }
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
    }

    public static void ReceiveDamage(int dmg)
    {
        if(BarrierHp > 0)
        {
            int blockDmg = Mathf.RoundToInt(dmg * BlockPercentPerTimeDefence);
            if (BarrierHp > blockDmg)
                BarrierHp -= blockDmg;
            else
            {
                blockDmg = BarrierHp;
                BarrierHp = 0;
            }
            dmg -= blockDmg;
        }
        if (dmg > 0)
        {
            SumHp -= dmg;
            UIController.ShowHpPopUp(dmg, swordCharacterController.thisTransform.position, true);
            listCharacterController.ForEach(characterController =>
                {
                    if (!characterController.AtkIsPlay)
                    {
                        characterController.thisAnimation.Stop();
                        characterController.thisAnimation.Play(characterController.hurtStr);
                        characterController.thisAnimation.CrossFadeQueued(characterController.IsFall ?
                            characterController.fallStr : characterController.idleStr);
                    }
                });
        }
    }

    public static void ReceiveHeal(int heal)
    {
        SumHp += Mathf.RoundToInt(heal * (1f + HealPercentIncrease));
        UIController.ShowHpPopUp(heal, swordCharacterController.thisTransform.position, false);
    }

    public static void BarrierDefence(int barrierHp, float blockPercentPerTimeDefence)
    {
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

    public static void TurnEffectDecrease()
    {
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
    bool atkIsPlay = false;
    CharacterActionState chaActionState;
    string actionStr,
        idleStr,
        hurtStr, 
        fallStr;

    IActionBehaviour iActionBehaviour;

	int useCost;

    //Action Time
    float timeBeforeMonsterListShowParticleReceiveDamage,
        timeAfterMonsterListShowParticleReceiveDamage;

    bool isFall = false;
    int turnFall = 0;
    #endregion

    #region Properties
    public bool AtkIsPlay
    {
        get { return atkIsPlay; }
    }

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
        switch(chaActionState)
        {
            case CharacterActionState.SwordAction:

                break;
            case CharacterActionState.BowAction:

                break;
            case CharacterActionState.WandAction:

                break;
            case CharacterActionState.ShieldAction:

                break;
            case CharacterActionState.ScrollAction:

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
                //actionFinishMethod = SwordAttack;

                timeBeforeMonsterListShowParticleReceiveDamage = 0.1f;
                timeAfterMonsterListShowParticleReceiveDamage = 0.65f;
                break;
            case CharacterActionState.BowAction:
                holdHandTransform = transform.Find(leftHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.1f;
                weaponTransform.localRotation = Quaternion.Euler(5f, 0f, 0f);
                //actionFinishMethod = BowAttack;

                timeBeforeMonsterListShowParticleReceiveDamage = 0.75f;
                timeAfterMonsterListShowParticleReceiveDamage = 0f;
                break;
            case CharacterActionState.WandAction:
                holdHandTransform = transform.Find(rightHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.1f;
                weaponTransform.localRotation = Quaternion.Euler(5f, 0f, 0f);
                //actionFinishMethod = WandAttack;

                timeBeforeMonsterListShowParticleReceiveDamage = 0f;
                timeAfterMonsterListShowParticleReceiveDamage = 0.75f;
                break;
            case CharacterActionState.ShieldAction:
                holdHandTransform = transform.Find(rightHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.2f;
                weaponTransform.localRotation = Quaternion.Euler(75f, 180f, 90f);
                //actionFinishMethod = ShieldDefence;

                timeBeforeMonsterListShowParticleReceiveDamage = 0.75f;
                timeAfterMonsterListShowParticleReceiveDamage = 0f;
                break;
            case CharacterActionState.ScrollAction:
                holdHandTransform = transform.Find(rightHandIK);
                weaponTransform.parent = holdHandTransform;
                weaponTransform.localPosition = Vector3.left * 0.1f;
                weaponTransform.localRotation = Quaternion.Euler(-60f, 0f, 105f);
                //actionFinishMethod = ScrollBuff;

                timeBeforeMonsterListShowParticleReceiveDamage = 0.75f;
                timeAfterMonsterListShowParticleReceiveDamage = 0f;
                break;
        }
    }

    public void Action()
    {
        StartCoroutine(UpdateAnimationAtk());
    }

    IEnumerator UpdateAnimationAtk()
    {
        if (!IsFall)
        {
            atkIsPlay = true;
            thisAnimation.Play(actionStr);

            yield return new WaitForSeconds(timeBeforeMonsterListShowParticleReceiveDamage);
            MonsterListShowParticleReceiveDamage();
            yield return new WaitForSeconds(timeAfterMonsterListShowParticleReceiveDamage);

            //yield return new WaitForSeconds(0.6f);

            //actionFinishMethod();
            print("Atk");
            atkIsPlay = false;

            while (thisAnimation.isPlaying)
                yield return null;

            thisAnimation.Play(idleStr);
            yield return new WaitForSeconds(1f);
        }
        turnController.TurnChange();
    }

    void SwordAttack()
    {
        //listMonsters.ForEach(monster =>
            //{
                //int dmg = AttackDamageCalculate(swordAtkValue, nearPoint, monster.transform.position.z, 0.9f, 1.1f);
                //monster.ReceiveDamage(dmg);
            //});
    }

    void BowAttack()
    {
        //listMonsters.ForEach(monster =>
            //{
                //int dmg = AttackDamageCalculate(bowAtkValue, middlePoint, monster.transform.position.z, 0.75f, 1.5f);
                //monster.ReceiveDamage(dmg);
            //});
    }

    void WandAttack()
    {
        //listMonsters.ForEach(monster =>
            //{
                //int dmg = AttackDamageCalculate(wandAtkValue, farPoint, monster.transform.position.z, 0.8f, 1.2f);
                //monster.ReceiveDamage(dmg);
            //});
    }

    void ShieldDefence()
    {
        StartCoroutine(TimeDefence(2f));
    }

    void ScrollBuff()
    {

    }

    IEnumerator TimeDefence(float time)
    {
        yield return new WaitForSeconds(time);
    }

    void MonsterListShowParticleReceiveDamage()
    {
        monster.ShowParticleReceiveDamage(chaActionState);
    }

    public void SetToFall(int turnFall)
    {
        IsFall = true;
        TurnFall = turnFall;
    }
}
