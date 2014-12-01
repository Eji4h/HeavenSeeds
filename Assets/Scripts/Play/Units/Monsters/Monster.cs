using UnityEngine;
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
        woodParticle, 
        burnParticle, 
        vortexParticle,
        stunParticle,
        rootParticle;

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
    #endregion

    #region Variable
    protected Rigidbody thisRigidbody;
    protected int hp;
    protected float damageMinimumMultiply = 0.9f, damageMaximumMultiply = 1.1f;

    ElementType weaknessElement;

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
                StartCoroutine(WaitingDieAnimationToDestroy());
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
            burnParticle.SetActive(isBurn);
        }
    }

    int LowAttackDamageTurn
    {
        get { return lowAttackDamageTurn; }
        set
        {
            lowAttackDamageTurn = value;
            isLowAttackDamage = lowAttackDamageTurn > 0;
            vortexParticle.SetActive(isLowAttackDamage);
        }
    }

    int StunTurn
    {
        get { return stunTurn; }
        set
        {
            stunTurn = value;
            isStun = stunTurn > 0;
            stunParticle.SetActive(isStun);
        }
    }

    int MoreReceiveDamageTurn
    {
        get { return moreReceiveDamageTurn; }
        set
        {
            moreReceiveDamageTurn = value;
            isMoreReceiveDamage = moreReceiveDamageTurn > 0;
            rootParticle.SetActive(isMoreReceiveDamage);
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

        Collider thisCollider = collider;
        slashParticleLocalPosition =
            new Vector3(0f, thisCollider.bounds.center.y, thisCollider.bounds.extents.z + 0.5f);
        arrowHitParticleLocalPosition =
            new Vector3(0f, thisCollider.bounds.center.y - 0.25f, thisCollider.bounds.extents.z + 0.5f);

        burnParticle.transform.parent = thisTransform;
        burnParticle.transform.localPosition = Vector3.zero;
        listGameObjectTransformInParent.Add(burnParticle.transform);

        vortexParticle.transform.parent = thisTransform;
        vortexParticle.transform.localPosition = Vector3.zero;
        listGameObjectTransformInParent.Add(vortexParticle.transform);

        stunParticle.transform.parent = thisTransform;
        stunParticle.transform.localPosition = new Vector3(0f, thisCollider.bounds.max.y, thisCollider.bounds.extents.z + 0.1f);
        listGameObjectTransformInParent.Add(stunParticle.transform);

        rootParticle.transform.parent = thisTransform;
        rootParticle.transform.localPosition = Vector3.zero;
        listGameObjectTransformInParent.Add(rootParticle.transform);

        base.Start();
    }

    public virtual void ReceiveDamage(int dmg)
    {
        if (!isImmortal)
        {
            dmg = Mathf.RoundToInt(dmg * 1.2f);
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
                StartCoroutine(RunWaitTimeToEndTurn(3f));
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
        StunTurn = 1;
    }

    IEnumerator WoodReceiveBehaviour()
    {
        int damge = elementDamageBase * (weaknessElement == ElementType.Wood ? 2 : 1);
        ReuseGameObject(woodParticle, Vector3.zero, true);
        yield return new WaitForSeconds(2f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(elementDamageBase, 0.95f, 1.05f, 3));
        MoreReceiveDamageTurn = 1;
    }

    IEnumerator BurnReceiveBehaviour()
    {
        nowBurning = true;
        yield return new WaitForSeconds(1f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(MaxHp * 0.01f, 0.5f, 1.5f, 3));
        BurnTurn--;
        nowBurning = false;
    }

    IEnumerator WaitingDieAnimationToDestroy()
    {
        yield return new WaitForSeconds(1f);

        listGameObjectTransformInParent.ForEach(gameObjectTransformInParent =>
            {
                gameObjectTransformInParent.parent = null;
                gameObjectTransformInParent.gameObject.SetActive(false);
            });
        Destroy(gameObject);
        SceneController.NextMonsterQueue();
        SceneController.TurnController.CharacterActionEnd();
    }
}
    #endregion
