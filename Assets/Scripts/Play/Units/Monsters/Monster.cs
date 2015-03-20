using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Monster : Unit
{
    static GameObject 
        slashParticlePrefab = Resources.Load("Prefabs/Particle/Player/Attack/Slash 2") as GameObject,
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

    static int elementDamageBase;

    static Color32 receiveDamageHpPopColor = new Color32(235, 72, 7, 255),
        healHpPopUpColor = new Color32(0, 220, 0, 255);

    public static void SetInit()
    {
        elementDamageBase = (CharacterController.SwordValue +
            CharacterController.BowValue +
            CharacterController.WandValue +
            CharacterController.ShieldValue +
            CharacterController.ScrollValue) / 5;
    }

    int hp;
    PayUIProgressBar hpBar;

    [SerializeField]
    [Range(1, 3)]
    int numberOfLine;

    [SerializeField]
    [Range(0, 100000)]
    protected int damageBase;

    [SerializeField]
    [Range(0.1f, 5f)]
    float damageMinimumMultiply,
        damageMaximumMultiply;

    [SerializeField]
    Vector3 offsetBarPos;

    [SerializeField]
    protected ElementType weaknessElement;

    protected bool isImmortal = false;

    Vector3 slashParticleLocalPosition,
        arrowHitParticleLocalPosition,
        spellParticleLocalPosition = new Vector3(0f, 0.05f, 0f);

    Queue<IEnumerator> queueElementReceive = new Queue<IEnumerator>(4);
    bool queueElementIsRunning = false;

    GameObject slashParticle,
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

    protected bool isBurn = false,
        isLowAttackDamage = false,
        isStun = false,
        isMoreReceiveDamage = false;

    int lowAttackDamageCount = 0,
        moreReceiveDamageCount = 0;

    float burnRemainTime = 0f,
        stunRemainTime = 0f;

    float percentDebuffToCharacter;

    protected bool isAttackUp = false;
    protected int attackUpStack = 0;

    public PayUIProgressBar HpBar
    {
        get { return hpBar; }
        set 
        {
            hpBar = value;
            hpBar.transform.parent = UIController.BarsTransform;
            hpBar.transform.localScale = Vector3.one;

            hpBar.MaxValue = MaxHp;
            Hp = MaxHp;

            hpBar.GetComponent<UIOffset>().SetInit(offsetBarPos + new Vector3(0f, .025f), ThisTransform,
                SceneController.MainCamera, SceneController.UICamera);
        }
    }

    public int NumberOfLine
    {
        get { return numberOfLine; }
    }

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

            hpBar.Value = Hp;

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

    public float DamageMinimumMultiply
    {
        get { return damageMinimumMultiply; }
    }

    public float DamageMaximumMultiply
    {
        get { return damageMaximumMultiply; }
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

    float BurnRemainTime
    {
        get { return burnRemainTime / NumberSecurity.RandomNumSecurity; }
        set
        {
            burnRemainTime = Mathf.Clamp(value, 0f, 10f) * NumberSecurity.RandomNumSecurity;
            bool oldIsBurn = isBurn;
            isBurn = BurnRemainTime > 0;
            burnParticle.SetActive(isBurn);
            if(!oldIsBurn && isBurn)
                StartCoroutine(UpdateBurnBehaviour());
        }
    }

    int LowAttackDamageCount
    {
        get { return lowAttackDamageCount / NumberSecurity.RandomNumSecurity; }
        set
        {
            lowAttackDamageCount = Mathf.Clamp(value, 0, 10) * NumberSecurity.RandomNumSecurity;
            isLowAttackDamage = LowAttackDamageCount > 0;
            vortexParticle.SetActive(isLowAttackDamage);
        }
    }

    float StunRemainTime
    {
        get { return stunRemainTime / NumberSecurity.RandomNumSecurity; }
        set
        {
            stunRemainTime = Mathf.Clamp(value, 0f, 10f) * NumberSecurity.RandomNumSecurity;
            bool oldIsStun = isStun;
            isStun = StunRemainTime > 0;
            stunParticle.SetActive(isStun);
            if (!oldIsStun && isStun)
                StartCoroutine(UpdateStunBehaviour());
        }
    }

    int MoreReceiveDamageCount
    {
        get { return moreReceiveDamageCount / NumberSecurity.RandomNumSecurity; }
        set
        {
            moreReceiveDamageCount = Mathf.Clamp(value, 0, 10) * NumberSecurity.RandomNumSecurity;
            isMoreReceiveDamage = MoreReceiveDamageCount > 0;
            rootParticle.SetActive(isMoreReceiveDamage);
        }
    }

    protected override void Start()
    {
        if (damageMinimumMultiply == 0f)
            damageMinimumMultiply = 0.9f;
        if (damageMaximumMultiply == 0f)
            damageMaximumMultiply = 1.1f;

        Collider thisCollider = collider;
        slashParticleLocalPosition =
            new Vector3(0f, thisCollider.bounds.center.y, thisCollider.bounds.extents.z + 0.5f);
        arrowHitParticleLocalPosition =
            new Vector3(0f, thisCollider.bounds.center.y - 0.25f, thisCollider.bounds.extents.z + 0.5f);

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

        burnParticle.transform.parent = ThisTransform;
        burnParticle.transform.localPosition = Vector3.zero;

        vortexParticle.transform.parent = ThisTransform;
        vortexParticle.transform.localPosition = Vector3.zero;

        stunParticle.transform.parent = ThisTransform;
        stunParticle.transform.localPosition = new Vector3(0f, 
            thisCollider.bounds.max.y / ThisTransform.localScale.y, 
            (thisCollider.bounds.extents.z + 0.1f) / ThisTransform.localScale.z);

        rootParticle.transform.parent = ThisTransform;
        rootParticle.transform.localPosition = Vector3.zero;
    }

    public override void SetGateBarController(GateBarController gateBarController)
    {
        gateBarController.GetComponent<UIOffset>().SetInit(offsetBarPos, ThisTransform,
            SceneController.MainCamera, SceneController.UICamera);
        gateBarController.GateCountTarget = 1;
        gateBarController.SetCheckGateCountIsTarget(true);
        gateBarController.GateCountTargetAction = NormalAttack;
        base.SetGateBarController(gateBarController);
    }

    public virtual void RunBehaviour()
    {

    }

    protected virtual void EndTurn()
    {
        EndTurnBehaviour(true);
    }

    protected virtual void EndTurnNoCrossFadeToIdle()
    {
        EndTurnBehaviour(false);
    }

    protected virtual void EndTurnBehaviour(bool isCrossFadeToIdle)
    {
        if (Hp > 0)
        {
            if (isCrossFadeToIdle)
                ThisAnimation.CrossFade("Idle");
            MonsterBehaviour();
        }
    }

    public virtual void ReceiveDamage(int dmg)
    {
        if (!isImmortal)
        {
            dmg = Mathf.RoundToInt(dmg * (isMoreReceiveDamage ? 1.2f : 1f));
            UIController.ShowHpPopUp(dmg, ThisTransform.position, receiveDamageHpPopColor);
            Hp -= dmg;
            MoreReceiveDamageCount--;
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
        if (isLowAttackDamage)
            LowAttackDamageCount--;
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
    }

    protected void NormalAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        ThisAnimation.CrossFade("Attack");
        GateBarController.GateCount--;
        yield return new WaitForSeconds(ThisAnimation["Attack"].length);
        ThisAnimation.CrossFade("Idle");
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
        Destroy(HpBar.gameObject);
        Destroy(GateBarController.gameObject);
        SceneController.UpdateQueueLineMonster(this);

        yield return new WaitForSeconds(waitTime);
            
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
    }

    IEnumerator FireReceiveBehaviour()
    {
        int damage = elementDamageBase * (weaknessElement == ElementType.Fire ? 2 : 1);
        ReuseGameObject(fireParticle, Vector3.zero, true);
        yield return new WaitForSeconds(2.5f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(damage, 1f, 1.2f, 3));
        yield return new WaitForSeconds(1f);
        BurnRemainTime = 10f;
    }

    IEnumerator WaterReceiveBehaviour()
    {
        int damagePerReceive = (elementDamageBase / 4) * 
            (weaknessElement == ElementType.Water ? 2 : 1);
        ReuseGameObject(waterParticle, Vector3.zero, true);
        yield return new WaitForSeconds(0.75f);
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.25f);
            ReceiveDamage(OftenMethod.ProbabilityDistribution(damagePerReceive, 0.75f, 1.1f, 3));
        }
        LowAttackDamageCount = 2;
    }

    IEnumerator EarthReceiveBehaviour()
    {
        int damagePerReceive = (elementDamageBase / 6) * 
            (weaknessElement == ElementType.Earth ? 2 : 1);
        ReuseGameObject(earthParticle, Vector3.zero, true);
        yield return new WaitForSeconds(1.25f);
        for (int i = 0; i < 6; i++)
        {
            ReceiveDamage(OftenMethod.ProbabilityDistribution(damagePerReceive, 0.9f, 1.1f, 3));
            yield return new WaitForSeconds(0.2f);
        }
        StunRemainTime = 10f;
    }

    IEnumerator WoodReceiveBehaviour()
    {
        int damage = elementDamageBase * (weaknessElement == ElementType.Wood ? 2 : 1);
        ReuseGameObject(woodParticle, Vector3.zero, true);
        yield return new WaitForSeconds(3.5f);
        ReceiveDamage(OftenMethod.ProbabilityDistribution(damage, 0.95f, 1.05f, 3));
        MoreReceiveDamageCount = 3;
        yield return new WaitForSeconds(1f);
    }

    IEnumerator UpdateBurnBehaviour()
    {
        while (isBurn)
        {
            ReceiveDamage(OftenMethod.ProbabilityDistribution(MaxHp * 0.01f, 0.5f, 1.5f, 3));
            yield return new WaitForSeconds(1f);
            BurnRemainTime--;
        }
    }

    IEnumerator UpdateStunBehaviour()
    {
        while(isStun)
        {
            StunRemainTime -= Time.deltaTime;
            yield return null;
        }
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
