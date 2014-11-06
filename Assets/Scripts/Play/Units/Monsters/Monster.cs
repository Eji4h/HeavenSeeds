using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public abstract class Monster : Unit
{
    #region Static Variable
    static GameObject slashParticlePrefab,
        arrowHitParticlePrefab,
        spellParticlePrefab;

    static int monsterEndCount = 0;
    #endregion

    #region Static Properties
    public static int MonsterEndCount
    {
        get { return Monster.monsterEndCount; }
        set
        {
            Monster.monsterEndCount = value;
            if (monsterEndCount >= listMonsters.Count)
            {
                turnController.TurnChange();
                monsterEndCount = 0;
            }
        }
    }
    #endregion

    #region Static Method
    public static new void SetInit()
    {
        slashParticlePrefab = Resources.Load("Prefabs/Particle/Player/Attack/Slash 2") as GameObject;
        arrowHitParticlePrefab = Resources.Load("Prefabs/Particle/ArrowHit") as GameObject;
        spellParticlePrefab = Resources.Load("Prefabs/Particle/Player/Attack/Spell2") as GameObject;
    }
    #endregion

    #region Variable
    protected Rigidbody thisRigidbody;
    protected RangeType thisRangeType;

    int hp;
    protected int atkValue;
    protected float speed;

    GameObject slashParticle,
        arrowHitParticle,
        spellParticle;

    Vector3 slashParticleLocalPosition,
        arrowHitParticleLocalPosition,
        spellParticleLocalPosition;

    #endregion

    #region Properties

    public virtual int Hp
    {
        get { return hp / randomNumSecurity; }
        set
        {
            if (value > MaxHp)
                hp = MaxHp * randomNumSecurity;
            else
                hp = value * randomNumSecurity;

            if (Hp <= 0)
            {
                listMonsters.Remove(this);
                thisTransform.position = new Vector3(-1000f, -1000f);
                thisRigidbody.useGravity = false;
                thisRigidbody.isKinematic = true;
            }
        }
    }
    #endregion

    #region Enum Type
    public enum RangeType
    {
        Near,
        Middle,
        Far
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        listMonsters.Add(this);
        thisRigidbody = rigidbody;
        atkValue = PlayerPrefs.GetInt(name + "AtkValue", 100);

        slashParticle = Instantiate(slashParticlePrefab, Vector3.zero, Quaternion.AngleAxis(180f, Vector3.up)) as GameObject;
        arrowHitParticle = Instantiate(arrowHitParticlePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        spellParticle = Instantiate(spellParticlePrefab, Vector3.zero, spellParticlePrefab.transform.rotation) as GameObject;

        slashParticle.SetActive(false);
        arrowHitParticle.SetActive(false);
        spellParticle.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        SetGen(thisTransform.position);
    }

    public void SetGen(Vector3 genPos)
    {
        Hp = MaxHp;
        thisTransform.position = genPos;
        thisRigidbody.useGravity = true;
        thisRigidbody.isKinematic = false;

        float thisPosZ = thisTransform.position.z;

        if (thisPosZ == nearPoint)
            thisRangeType = RangeType.Near;
        else if (thisPosZ == middlePoint)
            thisRangeType = RangeType.Middle;
        else if (thisPosZ == farPoint)
            thisRangeType = RangeType.Far;
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
        UIController.ShowHpPopUp(dmg, thisTransform.position, true);
        Hp -= dmg;
    }

    public virtual void ReceiveHeal(int heal)
    {
        UIController.ShowHpPopUp(heal, thisTransform.position, false);
        Hp += heal;
    }

    public abstract void StartState();

    protected virtual void EndTurn()
    {
        MonsterEndCount++;
        thisAnimation.CrossFade("Idle");
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
}
