﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using PayUnity;

public abstract class Monster : Unit
{
    #region Static Variable
    static GameObject slashParticle,
        arrowHitParticle,
        spellParticle, 
        fireParticle,
        waterParticle,
        earthParticle,
        woodParticle;

    static int elementDamageBase;
    #endregion

    #region Static Method
    public static new void SetInit()
    {
        GameObject slashParticlePrefab = Resources.Load("Prefabs/Particle/Player/Attack/Slash 2") as GameObject, 
            arrowHitParticlePrefab = Resources.Load("Prefabs/Particle/ArrowHit") as GameObject, 
            spellParticlePrefab = Resources.Load("Prefabs/Particle/Player/Attack/Spell2") as GameObject,
            fireParticlePrefab = Resources.Load("Prefabs/Particle/Player/Ultimate/newFire") as GameObject,
            waterParticlePrefab = Resources.Load("Prefabs/Particle/Player/Ultimate/Water") as GameObject,
            earthParticlePrefab = Resources.Load("Prefabs/Particle/Player/Ultimate/Ground") as GameObject,
            woodParticlePrefab = Resources.Load("Prefabs/Particle/Player/Ultimate/LeafStrom") as GameObject;

        slashParticle = Instantiate(slashParticlePrefab, Vector3.zero, Quaternion.AngleAxis(180f, Vector3.up)) as GameObject;
        arrowHitParticle = Instantiate(arrowHitParticlePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        spellParticle = Instantiate(spellParticlePrefab, Vector3.zero, spellParticlePrefab.transform.rotation) as GameObject;
        fireParticle = Instantiate(fireParticlePrefab) as GameObject;
        waterParticle = Instantiate(waterParticlePrefab) as GameObject;
        earthParticle = Instantiate(earthParticlePrefab) as GameObject;
        woodParticle = Instantiate(woodParticlePrefab) as GameObject;

        slashParticle.SetActive(false);
        arrowHitParticle.SetActive(false);
        spellParticle.SetActive(false);
        fireParticle.SetActive(false);
        waterParticle.SetActive(false);
        earthParticle.SetActive(false);
        woodParticle.SetActive(false);

        elementDamageBase = (CharacterController.SwordValue +
            CharacterController.BowValue +
            CharacterController.WandValue +
            CharacterController.ShieldValue +
            CharacterController.ScrollValue) / 5;
    }
    #endregion

    #region Variable
    protected Rigidbody thisRigidbody;
    protected int hp;
    protected float damageMinimumMultiply = 0.9f, damageMaximumMultiply = 1.1f;

    ElementType weaknessElement;

    protected bool isImmortal = false;

    Vector3 slashParticleLocalPosition,
        arrowHitParticleLocalPosition,
        spellParticleLocalPosition;

    Queue<IEnumerator> queueElementReceive = new Queue<IEnumerator>(4);
    bool queueElementIsRunning = false,
        nowBurning = false;

    protected bool isBurn = false,
        isLowAttackDamage = false,
        isMoreReceiveDamage = false,
        isStun = false;

    int burnTurn = 0,
        lowAttackDamageTurn = 0,
        moreReceiveDamageTurn = 0,
        stunTurn = 0;

    protected Material material;
    #endregion

    #region Properties
    protected override int MaxHp
    {
        get { return base.MaxHp; }
        set
        {
            base.MaxHp = value;
            UIController.MonsterHpBar.MaxValue = MaxHp;
        }
    }
    public virtual int Hp
    {
        get { return hp / randomNumSecurity; }
        set
        {
            if (value > MaxHp)
                hp = MaxHp * randomNumSecurity;
            else
                hp = value * randomNumSecurity;

            UIController.MonsterHpBar.Value = Hp;

            if (Hp <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(AlphaToDestroy());
            }
        }
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
        get { return burnTurn; }
        set
        {
            burnTurn = value;
            isBurn = burnTurn > 0;
        }
    }

    int LowAttackDamageTurn
    {
        get { return lowAttackDamageTurn; }
        set
        {
            lowAttackDamageTurn = value;
            isLowAttackDamage = lowAttackDamageTurn > 0;
        }
    }

    int MoreReceiveDamageTurn
    {
        get { return moreReceiveDamageTurn; }
        set
        {
            moreReceiveDamageTurn = value;
            isMoreReceiveDamage = moreReceiveDamageTurn > 0;
        }
    }

    int StunTurn
    {
        get { return stunTurn; }
        set
        {
            stunTurn = value;
            isStun = stunTurn > 0;
        }
    }
    #endregion

    #region Method
    protected override void Awake()
    {
        base.Awake();
        thisRigidbody = rigidbody;
    }

    protected override void Start()
    {
        UIController.MonsterHpBar.MaxValue = MaxHp;
        Hp = MaxHp;
        SetGen(thisTransform.position);
        base.Start();
    }

    public void SetGen(Vector3 genPos)
    {
        thisTransform.position = genPos;
        thisRigidbody.useGravity = true;
        thisRigidbody.isKinematic = false;
    }

    protected void SetLocalPositionReceiveDamageParticle(Vector3 slashParticleLocalPosition,
        Vector3 arrowHitParticleLocalPosition,
        Vector3 spellParticleLocalPosition)
    {
        this.slashParticleLocalPosition = slashParticleLocalPosition;
        this.arrowHitParticleLocalPosition = arrowHitParticleLocalPosition;
        this.spellParticleLocalPosition = spellParticleLocalPosition;
    }

    public virtual void ReceiveDamage(int dmg)
    {
        if (!isImmortal)
        {
            if (isMoreReceiveDamage)
            {
                print("Damage Before : " + dmg);
                dmg = Mathf.RoundToInt(dmg * 1.2f);
                print("Damage After : " + dmg);
            }
            else
                print("Damage : " + dmg);
            UIController.ShowHpPopUp(dmg, thisTransform.position, true);
            Hp -= dmg;
        }
    }

    public virtual void ReceiveHeal(int heal)
    {
        UIController.ShowHpPopUp(heal, thisTransform.position, false);
        Hp += heal;
    }

    public void StartState()
    {
        if (Hp > 0)
        {
            MoreReceiveDamageTurn--;
            if (!isStun)
                MonsterBehaviour();
            else
                EndTurn();
        }
    }

    protected virtual void EndTurn()
    {
        if (Hp > 0)
        {
            thisAnimation.CrossFade("Idle");

            if (isBurn)
                StartCoroutine(BurnReceiveBehaviour());
            LowAttackDamageTurn--;
            StunTurn--;

            turnController.TurnChange();
        }
    }

    public void SendDamageToCharacter(int dmg)
    {
        SendDamageToCharacter(dmg, damageMinimumMultiply, damageMaximumMultiply);
    }

    public void SendDamageToCharacter(int dmg, float minimumMultiply, float maximumMultiply)
    {
        if(isLowAttackDamage)
        {
            print("Damage Send Before Calculate LowAttackDamage : " + dmg);
            print("Damage Send After Calculate LowAttackDamage : " + (dmg * (isLowAttackDamage ? 0.8f : 1f)));
        }
        else
            print("Normal Send Damage : " + dmg);
        CharacterController.ReceiveDamage(
            OftenMethod.ProbabilityDistribution(dmg * (isLowAttackDamage ? 0.8f : 1f), 
            minimumMultiply, maximumMultiply, 3));
    }

    protected virtual void MonsterBehaviour()
    {
        NormalAttack();
    }

    protected void NormalAttack()
    {
        thisAnimation.CrossFade("Attack");
        StartCoroutine(RunWaitTimeToEndTurn(thisAnimation["Attack"].length));
    }

    IEnumerator RunWaitTimeToEndTurn(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        EndTurn();
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

    public void RandomCharacterFall(int turnFall)
    {
        if (listCharacterControllerIsFall.Count < 5)
        {
            turnFall++;
            CharacterController selectedCharacterController;
            List<CharacterController> listUnFallCharacterController = 
                new List<CharacterController>(5);

            listCharacterController.ForEach(characterController =>
                listUnFallCharacterController.Add(characterController));

            listCharacterControllerIsFall.ForEach(characterController =>
                listUnFallCharacterController.Remove(characterController));
            selectedCharacterController = listUnFallCharacterController
                [Random.Range(0, listUnFallCharacterController.Count)];

            selectedCharacterController.SetToFall(turnFall);
        }
    }

    public void ReceiveQueue(ElementType element)
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
        int damage = elementDamageBase * 5 * (weaknessElement == ElementType.Fire ? 2 : 1);
        ReuseGameObject(fireParticle, Vector3.zero, true);
        yield return new WaitForSeconds(1f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(damage, 1f, 1.2f, 3));
        BurnTurn = 5;
    }

    IEnumerator WaterReceiveBehaviour()
    {
        int damagePerReceive = (elementDamageBase / 4) * 
            (weaknessElement == ElementType.Water ? 2 : 1);
        ReuseGameObject(waterParticle, Vector3.zero, true);
        for (int i = 0; i < 4; i++)
        {
            ReceiveDamage(OftenMethod.ProbabilityDistribution(damagePerReceive, 0.75f, 1.1f, 3));
            yield return new WaitForSeconds(1f);
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
        MoreReceiveDamageTurn = 1;
    }

    IEnumerator WoodReceiveBehaviour()
    {
        int damge = elementDamageBase * (weaknessElement == ElementType.Wood ? 2 : 1);
        ReuseGameObject(woodParticle, Vector3.zero, true);
        yield return new WaitForSeconds(2f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(elementDamageBase, 0.95f, 1.05f, 3));
        StunTurn = 1;
    }

    IEnumerator BurnReceiveBehaviour()
    {
        nowBurning = true;
        //Burn Effect Show
        print("Burn Effect");
        yield return new WaitForSeconds(1f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(MaxHp * 0.01f, 0.5f, 1.5f, 3));
        BurnTurn--;
        nowBurning = false;
    }

    IEnumerator AlphaToDestroy()
    {
        //Color targetColor = new Color(material.color.r, material.color.g, material.color.b, 0f);

        //while (material.color.a > 0.1f)
        //{
        //    material.color = Color.Lerp(material.color, targetColor, Time.deltaTime);
        //    print(material.color.a);
        //    yield return null;
        //}

        yield return new WaitForSeconds(1f);

        listGameObjectTransformInParent.ForEach(gameObjectTransformInParent =>
            gameObjectTransformInParent.parent = null);
        Destroy(gameObject);
        SceneController.NextMonsterQueue();
        SceneController.TurnController.CharacterActionEnd();
    }
}
    #endregion
