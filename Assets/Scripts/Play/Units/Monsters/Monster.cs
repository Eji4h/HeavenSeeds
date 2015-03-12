﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using PayUnity;

public class Monster : Unit
{
    static GameObject slashParticle,
        arrowHitParticle,
        spellParticle, 
        fireParticle,
        waterParticle,
        earthParticle,
        woodParticle, 
        burnParticle, 
        vortexParticle,
        stunParticle,
        rootParticle;

    static int elementDamageBase;

    static Color32 receiveDamageHpPopColor = new Color32(235, 72, 7, 255),
        healHpPopUpColor = new Color32(0, 220, 0, 255);

    public static new void SetInit()
    {
        GameObject slashParticlePrefab = Resources.Load("Prefabs/Particle/Player/Attack/Slash 2") as GameObject, 
            arrowHitParticlePrefab = Resources.Load("Prefabs/Particle/ArrowHit") as GameObject, 
            spellParticlePrefab = Resources.Load("Prefabs/Particle/Player/Attack/Spell2") as GameObject,
            fireParticlePrefab = Resources.Load("Prefabs/Particle/Player/Ultimate/LatestFire") as GameObject,
            waterParticlePrefab = Resources.Load("Prefabs/Particle/Player/Ultimate/Water") as GameObject,
            earthParticlePrefab = Resources.Load("Prefabs/Particle/Player/Ultimate/Land") as GameObject,
            woodParticlePrefab = Resources.Load("Prefabs/Particle/Player/Ultimate/LeafStrom") as GameObject, 
            burnParticlePrefab = Resources.Load("Prefabs/Particle/StatEffect/Burn") as GameObject,
            vortexParticlePrefab = Resources.Load("Prefabs/Particle/StatEffect/WaterVotex") as GameObject, 
            stunParticlePrefab = Resources.Load("Prefabs/Particle/StatEffect/NewStun") as GameObject,
            rootParticlePrefab = Resources.Load("Prefabs/Particle/StatEffect/Vineeee") as GameObject;

        slashParticle = Instantiate(slashParticlePrefab, Vector3.zero, Quaternion.AngleAxis(180f, Vector3.up)) as GameObject;
        arrowHitParticle = Instantiate(arrowHitParticlePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        spellParticle = Instantiate(spellParticlePrefab, Vector3.zero, spellParticlePrefab.transform.rotation) as GameObject;
        fireParticle = Instantiate(fireParticlePrefab) as GameObject;
        waterParticle = Instantiate(waterParticlePrefab) as GameObject;
        earthParticle = Instantiate(earthParticlePrefab) as GameObject;
        woodParticle = Instantiate(woodParticlePrefab) as GameObject;
        burnParticle = Instantiate(burnParticlePrefab) as GameObject;
        vortexParticle = Instantiate(vortexParticlePrefab) as GameObject;
        stunParticle = Instantiate(stunParticlePrefab) as GameObject;
        rootParticle = Instantiate(rootParticlePrefab) as GameObject;

        slashParticle.SetActive(false);
        arrowHitParticle.SetActive(false);
        spellParticle.SetActive(false);
        fireParticle.SetActive(false);
        waterParticle.SetActive(false);
        earthParticle.SetActive(false);
        woodParticle.SetActive(false);
        burnParticle.SetActive(false);
        vortexParticle.SetActive(false);
        stunParticle.SetActive(false);
        rootParticle.SetActive(false);

        elementDamageBase = (CharacterController.SwordValue +
            CharacterController.BowValue +
            CharacterController.WandValue +
            CharacterController.ShieldValue +
            CharacterController.ScrollValue) / 5;
    }

    protected Rigidbody thisRigidbody;

    int hp;

    [SerializeField]
    [Range(0, 100000)]
    protected int damageBase;

    float damageMinimumMultiply,
        damageMaximumMultiply;

    protected ElementType weaknessElement;

    protected bool isImmortal = false;

    Vector3 slashParticleLocalPosition,
        arrowHitParticleLocalPosition,
        spellParticleLocalPosition = new Vector3(0f, 0.05f, 0f);

    Queue<IEnumerator> queueElementReceive = new Queue<IEnumerator>(4);
    bool queueElementIsRunning = false,
        nowBurning = false;

    protected bool isBurn = false,
        isLowAttackDamage = false,
        isStun = false,
        isMoreReceiveDamage = false;

    int burnTurn = 0,
        lowAttackDamageTurn = 0,
        stunTurn = 0,
        moreReceiveDamageTurn = 0;

    float percentDebuffToCharacter;

    protected bool isAttackUp = false;
    protected int attackUpStack = 0;

    protected float LocalPositionX
    {
        get { return ThisTransform.localPosition.x; }
        set { ThisTransform.localPosition = new Vector3(value, ThisTransform.position.y, ThisTransform.position.z); }
    }

    protected float LocalPositionY
    {
        get { return ThisTransform.localPosition.y; }
        set { ThisTransform.localPosition = new Vector3(ThisTransform.position.x, value, ThisTransform.position.z); }
    }

    protected float LocalPositionZ
    {
        get { return ThisTransform.localPosition.z; }
        set { ThisTransform.localPosition = new Vector3(ThisTransform.position.x, ThisTransform.position.y, value); }
    }

    public virtual int Hp
    {
        get { return hp / NumberSecurity.RandomNumSecurity; }
        set
        {
            if (value > MaxHp)
                hp = MaxHp * NumberSecurity.RandomNumSecurity;
            else
                hp = value * NumberSecurity.RandomNumSecurity;

            //UIController.MonsterHpBar.Value = Hp;

            if (Hp <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(WaitingDieAnimationToDestroy());
            }
        }
    }

    protected int DamageBase
    {
        get { return damageBase; }
    }

    protected float DamageMinimumMultiply
    {
        get { return damageMinimumMultiply / NumberSecurity.RandomNumSecurity; }
        set { damageMinimumMultiply = value * NumberSecurity.RandomNumSecurity; }
    }

    protected float DamageMaximumMultiply
    {
        get { return damageMaximumMultiply / NumberSecurity.RandomNumSecurity; }
        set { damageMaximumMultiply = value * NumberSecurity.RandomNumSecurity; }
    }

    public bool IsImmortal
    {
        get { return isImmortal; }
    }

    public void SetIsImmortalTrue()
    {
        isImmortal = true;
    }

    public void SetIsImmortalFalse()
    {
        isImmortal = false;
    }

    public bool QueueElementIsRunning
    {
        get { return queueElementIsRunning; }
    }

    public bool NowBurning
    {
        get { return nowBurning; }
    }

    int BurnTurn
    {
        get { return burnTurn / NumberSecurity.RandomNumSecurity; }
        set
        {
            burnTurn = value * NumberSecurity.RandomNumSecurity;
            isBurn = BurnTurn > 0;
            burnParticle.SetActive(isBurn);
        }
    }

    int LowAttackDamageTurn
    {
        get { return lowAttackDamageTurn / NumberSecurity.RandomNumSecurity; }
        set
        {
            lowAttackDamageTurn = value * NumberSecurity.RandomNumSecurity;
            isLowAttackDamage = LowAttackDamageTurn > 0;
            vortexParticle.SetActive(isLowAttackDamage);
        }
    }

    int StunTurn
    {
        get { return stunTurn / NumberSecurity.RandomNumSecurity; }
        set
        {
            stunTurn = value * NumberSecurity.RandomNumSecurity;
            isStun = StunTurn > 0;
            stunParticle.SetActive(isStun);
        }
    }

    int MoreReceiveDamageTurn
    {
        get { return moreReceiveDamageTurn / NumberSecurity.RandomNumSecurity; }
        set
        {
            moreReceiveDamageTurn = value * NumberSecurity.RandomNumSecurity;
            isMoreReceiveDamage = MoreReceiveDamageTurn > 0;
            rootParticle.SetActive(isMoreReceiveDamage);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        thisRigidbody = rigidbody;
    }

    protected override void Start()
    {
        if (damageMinimumMultiply == 0f)
            DamageMinimumMultiply = 0.9f;
        if (damageMaximumMultiply == 0f)
            DamageMaximumMultiply = 1.1f;

        //UIController.MonsterHpBar.MaxValue = MaxHp;
        Hp = MaxHp;

        Collider thisCollider = collider;
        slashParticleLocalPosition =
            new Vector3(0f, thisCollider.bounds.center.y, thisCollider.bounds.extents.z + 0.5f);
        arrowHitParticleLocalPosition =
            new Vector3(0f, thisCollider.bounds.center.y - 0.25f, thisCollider.bounds.extents.z + 0.5f);

        burnParticle.transform.parent = ThisTransform;
        burnParticle.transform.localPosition = Vector3.zero;
        listGameObjectTransformInParent.Add(burnParticle.transform);

        vortexParticle.transform.parent = ThisTransform;
        vortexParticle.transform.localPosition = Vector3.zero;
        listGameObjectTransformInParent.Add(vortexParticle.transform);

        stunParticle.transform.parent = ThisTransform;
        stunParticle.transform.localPosition = new Vector3(0f, 
            thisCollider.bounds.max.y / ThisTransform.localScale.y, 
            (thisCollider.bounds.extents.z + 0.1f) / ThisTransform.localScale.z);
        listGameObjectTransformInParent.Add(stunParticle.transform);

        rootParticle.transform.parent = ThisTransform;
        rootParticle.transform.localPosition = Vector3.zero;
        listGameObjectTransformInParent.Add(rootParticle.transform);

        base.Start();
    }

    public void StartState()
    {
        if (Hp > 0)
        {
            if (!isStun)
                MonsterBehaviour();
            else
                StartCoroutine(RunWaitTimeToEndTurn(3f));
        }
    }

    protected virtual void EndTurn()
    {
        StartCoroutine(EndTurnBehaviour(true));
    }

    protected virtual void EndTurnNoCrossFadeToIdle()
    {
        StartCoroutine(EndTurnBehaviour(false));
    }

    protected virtual IEnumerator EndTurnBehaviour(bool isCrossFadeToIdle)
    {
        if (Hp > 0)
        {
            if (isCrossFadeToIdle)
                ThisAnimation.CrossFade("Idle");

            if (isBurn)
                yield return StartCoroutine(BurnReceiveBehaviour());
            LowAttackDamageTurn--;
            StunTurn--;
        }
    }

    public virtual void ReceiveDamage(int dmg)
    {
        if (!isImmortal)
        {
            dmg = Mathf.RoundToInt(dmg * (isMoreReceiveDamage ? 1.2f : 1f));
            UIController.ShowHpPopUp(dmg, ThisTransform.position, receiveDamageHpPopColor);
            Hp -= dmg;
            MoreReceiveDamageTurn--;
        }
    }

    public virtual void ReceiveHeal(int heal)
    {
        UIController.ShowHpPopUp(heal, ThisTransform.position, healHpPopUpColor);
        Hp += heal;
    }

    public void ReceiveHealOfMissingHpPercent(float percentHealOfMissingHp)
    {
        int healHp = (int)((MaxHp - Hp) * (percentHealOfMissingHp / 100f));
        ReceiveHeal(healHp);
    }

    public virtual void ShowParticleReceiveDamage(CharacterActionState chaActionState)
    {
        switch (chaActionState)
        {
            case CharacterActionState.SwordAction:
                ReuseGameObject(slashParticle, slashParticleLocalPosition, true);
                break;
            case CharacterActionState.BowAction:
                ReuseGameObject(arrowHitParticle, arrowHitParticleLocalPosition, true);
                break;
            case CharacterActionState.WandAction:
                ReuseGameObject(spellParticle, spellParticleLocalPosition, true);
                break;
        }
    }

    public void SendDamageToCharacter(float damageBaseMultiply)
    {
        float damageBaseCal = DamageBase * (1f + (isAttackUp ? 0.4f : 0f) + attackUpStack * 0.1f);
        CharacterController.ReceiveDamage(
            OftenMethod.ProbabilityDistribution(damageBaseCal * damageBaseMultiply *
            (isLowAttackDamage ? 0.8f : 1f), DamageMinimumMultiply, DamageMaximumMultiply, 3));
    }

    #region AttackUp

    public void SetIsAttackUpIsTrue()
    {
        isAttackUp = true;
    }

    public void SetIsAttackUpIsFalse()
    {
        isAttackUp = false;
    }

    public void ResetAttackUpStack()
    {
        attackUpStack = 0;
    }

    public void IncreaseAttackUpStack()
    {
        attackUpStack++;
    }

    #endregion

    #region MonsterBehaviour
    protected virtual void MonsterBehaviour()
    {
        NormalAttack();
    }

    protected void NormalAttack()
    {
        ThisAnimation.CrossFade("Attack");
        StartCoroutineRunWaitTimeToEndTurn(ThisAnimation["Attack"].length);
    }

    protected void ResetLocalPositionToZero()
    {
        ThisTransform.localPosition = Vector3.zero;
    }

    protected void PlayAnimation(string animationNameToPlay)
    {
        ThisAnimation.Play(animationNameToPlay);
    }

    protected void CrossFadeAnimation(string animationNameToCrossFade)
    {
        ThisAnimation.CrossFade(animationNameToCrossFade);
    }

    protected void StartCoroutineRunWaitTimeToEndTurn(float timeToWait)
    {
        StartCoroutine(RunWaitTimeToEndTurn(timeToWait));
    }

    IEnumerator RunWaitTimeToEndTurn(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        EndTurn();
    }

    IEnumerator WaitingDieAnimationToDestroy()
    {
        var dieAnimationState = ThisAnimation["Die"];
        float waitTime = 0.5f;
        if (dieAnimationState != null)
        {
            ThisAnimation.CrossFade("Die");
            waitTime += dieAnimationState.length;
        }
        yield return new WaitForSeconds(waitTime);

        listGameObjectTransformInParent.ForEach(gameObjectTransformInParent =>
        {
            gameObjectTransformInParent.parent = null;
            gameObjectTransformInParent.gameObject.SetActive(false);
        });
            
        Destroy(gameObject);
        Destroy(this);
    }
    #endregion

    #region ReceiveAttackQueue
    public void ReceiveAttackQueue(ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire:
                queueElementReceive.Enqueue(FireReceiveBehaviour());
                break;
            case ElementType.Water:
                queueElementReceive.Enqueue(WaterReceiveBehaviour());
                break;
            case ElementType.Earth:
                queueElementReceive.Enqueue(EarthReceiveBehaviour());
                break;
            case ElementType.Wood:
                queueElementReceive.Enqueue(WoodReceiveBehaviour());
                break;
        }
        if (!queueElementIsRunning)
            StartCoroutine(RunElementReceive());
    }

    IEnumerator RunElementReceive()
    {
        queueElementIsRunning = true;
        while(queueElementReceive.Count > 0)
            yield return StartCoroutine(queueElementReceive.Dequeue());
        queueElementIsRunning = false;
        if (isBurn)
            StartCoroutine(BurnReceiveBehaviour());
    }

    IEnumerator FireReceiveBehaviour()
    {
        int damage = elementDamageBase * (weaknessElement == ElementType.Fire ? 2 : 1);
        ReuseGameObject(fireParticle, Vector3.zero, true);
        yield return new WaitForSeconds(6f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(damage, 1f, 1.2f, 3));
        BurnTurn = 5;
    }

    IEnumerator WaterReceiveBehaviour()
    {
        int damagePerReceive = (elementDamageBase / 4) * 
            (weaknessElement == ElementType.Water ? 2 : 1);
        ReuseGameObject(waterParticle, Vector3.zero, true);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            ReceiveDamage(OftenMethod.ProbabilityDistribution(damagePerReceive, 0.75f, 1.1f, 3));
        }
        LowAttackDamageTurn = 2;
    }

    IEnumerator EarthReceiveBehaviour()
    {
        int damagePerReceive = (elementDamageBase / 14) * 
            (weaknessElement == ElementType.Earth ? 2 : 1);
        ReuseGameObject(earthParticle, Vector3.zero, true);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 14; i++)
        {
            ReceiveDamage(OftenMethod.ProbabilityDistribution(damagePerReceive, 0.9f, 1.1f, 3));
            yield return new WaitForSeconds(0.25f);
        }
        StunTurn = 1;
    }

    IEnumerator WoodReceiveBehaviour()
    {
        int damage = elementDamageBase * (weaknessElement == ElementType.Wood ? 2 : 1);
        ReuseGameObject(woodParticle, Vector3.zero, true);
        yield return new WaitForSeconds(6.5f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(damage, 0.95f, 1.05f, 3));
        MoreReceiveDamageTurn = 3;
    }

    IEnumerator BurnReceiveBehaviour()
    {
        nowBurning = true;
        yield return new WaitForSeconds(1f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(MaxHp * 0.01f, 0.5f, 1.5f, 3));
        BurnTurn--;
        nowBurning = false;
    }
    #endregion

    #region Debuff Character
    public void SetPercentDebuffToCharacter(float percentDebuffToCharacter)
    {
        this.percentDebuffToCharacter = percentDebuffToCharacter;
    }

    public void RotateMagicCircle(int indexMagicCircleChange)
    {
        MagicFieldController.RotateMagicCircle(indexMagicCircleChange, -indexMagicCircleChange);
    }

    public void RandomRotateMagicCircle()
    {
        MagicFieldController.RotateMagicCircle(RandomNumberSpin(5, 10), RandomNumberSpin(5, 10));
    }

    public void RandomRotateMagicCircleOut()
    {
        MagicFieldController.RotateMagicCircle(RandomNumberSpin(5, 10), 0);
    }

    public void RandomRotateMagicCircleIn()
    {
        MagicFieldController.RotateMagicCircle(0, RandomNumberSpin(5, 10));
    }

    int RandomNumberSpin(int minNum, int maxNum)
    {
        return Random.Range(0, 2) == 0 ?
            Random.Range(-maxNum, -minNum) : Random.Range(minNum, maxNum);
    }

    public void RandomCharacterFall(int turnFall)
    {
        if (OftenMethod.RandomPercent(percentDebuffToCharacter))
        {
            if (ListCharacterControllerIsFall.Count < 5)
            {
                turnFall++;
                CharacterController selectedCharacterController;
                List<CharacterController> listUnFallCharacterController =
                    new List<CharacterController>(5);

                ListCharacterController.ForEach(characterController =>
                    listUnFallCharacterController.Add(characterController));

                ListCharacterControllerIsFall.ForEach(characterController =>
                    listUnFallCharacterController.Remove(characterController));
                selectedCharacterController = listUnFallCharacterController
                    [Random.Range(0, listUnFallCharacterController.Count)];

                selectedCharacterController.SetToFall(turnFall);
            }
        }
    }

    public void MagicFieldRandomAction(int randomCount)
    {
        if (OftenMethod.RandomPercent(percentDebuffToCharacter))
            MagicFieldController.RandomChaActionStateCount = randomCount;
    }

    public void CharacterBurn(int burnTurn)
    {
        if (OftenMethod.RandomPercent(percentDebuffToCharacter))
            CharacterController.BurnTurn = burnTurn;
    }

    public void CharacterPoison(int poisonTurn)
    {
        if (OftenMethod.RandomPercent(percentDebuffToCharacter))
            CharacterController.PoisonTurn = poisonTurn;
    }

    public void CharacterStun(int stunTurn)
    {
        if (OftenMethod.RandomPercent(percentDebuffToCharacter))
            CharacterController.StunTurn = stunTurn;
    }

    public void CharacterFreeze(int freezeTurn)
    {
        if (OftenMethod.RandomPercent(percentDebuffToCharacter))
            CharacterController.FreezeTurn = freezeTurn;
    }

    public void OrbBurn(int numberOrbBurn)
    {
        MagicFieldController.OrbBurn(numberOrbBurn);
    }

    public void OrbSkull(int numberOrbSkull)
    {
        MagicFieldController.OrbSkull(numberOrbSkull);
    }
    #endregion
}
