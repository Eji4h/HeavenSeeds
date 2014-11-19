using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

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
    }
    #endregion

    #region Variable
    protected Rigidbody thisRigidbody;
    protected int hp, attackDamageBase;

    ElementType weaknessElement;

    protected bool isImmortal = false;

    Vector3 slashParticleLocalPosition,
        arrowHitParticleLocalPosition,
        spellParticleLocalPosition;

    Queue<IEnumerator> queueElementReceive = new Queue<IEnumerator>(4);
    bool queueElementIsRunning = false;
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
    #endregion

    protected override void Awake()
    {
        base.Awake();
        thisRigidbody = rigidbody;
        attackDamageBase = PlayerPrefs.GetInt(name + "AttackDamageBase", 100);
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
            UIController.ShowHpPopUp(dmg, thisTransform.position, true);
            Hp -= dmg;
        }
    }

    public virtual void ReceiveHeal(int heal)
    {
        UIController.ShowHpPopUp(heal, thisTransform.position, false);
        Hp += heal;
    }

    public abstract void StartState();

    protected virtual void EndTurn()
    {
        thisAnimation.CrossFade("Idle");
        turnController.TurnChange();
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
            List<CharacterController> listUnFallCharacterController = new List<CharacterController>(5);

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
        {
            yield return StartCoroutine(queueElementReceive.Dequeue());
        }
        queueElementIsRunning = false;
    }

    IEnumerator FireReceiveBehaviour()
    {
        ReuseGameObject(fireParticle, Vector3.zero, true);
        yield return new WaitForSeconds(1f);
        ReceiveDamage(500);
    }

    IEnumerator WaterReceiveBehaviour()
    {
        ReuseGameObject(waterParticle, Vector3.zero, true);
        for (int i = 0; i < 4; i++)
        {
            ReceiveDamage(100);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator EarthReceiveBehaviour()
    {
        ReuseGameObject(earthParticle, Vector3.zero, true);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 14; i++)
        {
            ReceiveDamage(25);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator WoodReceiveBehaviour()
    {
        ReuseGameObject(woodParticle, Vector3.zero, true);
        yield return new WaitForSeconds(1.25f);
        ReceiveDamage(300);
    }

    IEnumerator AlphaToDestroy()
    {
        //Material material = renderer.material;
        //while(material.color.a > 0f)
        //{
        //    Color tempColor = material.color;
        //    material.color = new Color(tempColor.r, tempColor.g,
        //        tempColor.b, tempColor.a - 0.03f);
        //    yield return null;
        //}
        yield return new WaitForSeconds(3f);
        listGameObjectTransformInParent.ForEach(gameObjectTransformInParent =>
            gameObjectTransformInParent.parent = null);
        Destroy(gameObject);
        SceneController.NextMonsterQueue();
    }
}
