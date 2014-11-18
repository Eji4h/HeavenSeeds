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
    protected int hp, attackDamageBase;

    ElementType weaknessElement;

    protected bool isImmortal = false;

    GameObject slashParticle,
        arrowHitParticle,
        spellParticle;

    Vector3 slashParticleLocalPosition,
        arrowHitParticleLocalPosition,
        spellParticleLocalPosition;
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
                SceneController.NextMonsterQueue();
                Destroy(gameObject);
            }
        }
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        thisRigidbody = rigidbody;
        attackDamageBase = PlayerPrefs.GetInt(name + "AttackDamageBase", 100);

        slashParticle = Instantiate(slashParticlePrefab, Vector3.zero, Quaternion.AngleAxis(180f, Vector3.up)) as GameObject;
        arrowHitParticle = Instantiate(arrowHitParticlePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        spellParticle = Instantiate(spellParticlePrefab, Vector3.zero, spellParticlePrefab.transform.rotation) as GameObject;

        slashParticle.SetActive(false);
        arrowHitParticle.SetActive(false);
        spellParticle.SetActive(false);
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
}
