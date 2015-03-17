﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public abstract class Unit : MonoBehaviour
{
    protected static List<CharacterController> ListCharacterController
    {
        get { return SceneController.ListCharacterController; }
    }

    protected static CharacterController SwordCharacterController
    {
        get { return SceneController.SwordCharacterController; }
    }

    protected static CharacterController BowCharacterController
    {
        get { return SceneController.BowCharacterController; }
    }

    protected static CharacterController WandCharacterController
    {
        get { return SceneController.WandCharacterController; }
    }

    protected static CharacterController ShieldCharacterController
    {
        get { return SceneController.ShieldCharacterController; }
    }

    protected static CharacterController ScrollCharacterController
    {
        get { return SceneController.ScrollCharacterController; }
    }

    protected static List<CharacterController> ListCharacterControllerIsFall
    {
        get { return SceneController.ListCharacterControllerIsFall; }
    }

    protected static MagicFieldController MagicFieldController
    {
        get { return SceneController.MagicFieldController; }
    }

    Transform thisTransform;
    Animation thisAnimation;

    [SerializeField]
    [Range(0, 1000000)]
    protected int maxHp;

    GateBarController gateBarController;

    [SerializeField]
    [Range(1, 10)]
    int maxGate = 1;

    [SerializeField]
    [Range(0f, 1f)]
    float gateBarRegenFull1GatePerSecond;

    Predicate<int> checkGateCountIsTarget;
    Action gateCountTargetAction;

    protected List<Transform> listGameObjectTransformInParent = new List<Transform>(8);

    public Transform ThisTransform
    {
        get { return thisTransform; }
    }

    protected Animation ThisAnimation
    {
        get { return thisAnimation; }
    }

    public virtual int MaxHp
    {
        get { return maxHp; }
    }

    protected int MaxGate
    {
        get { return maxGate; }
        set { maxGate = value; }
    }

    protected float GateBarRegenFull1GatePerSecond
    {
        get { return gateBarRegenFull1GatePerSecond; }
        set { gateBarRegenFull1GatePerSecond = value; }
    }

    protected virtual void Awake()
    {
        thisTransform = transform;
        thisAnimation = animation;
    }

    // Use this for initialization
    protected abstract void Start();

    public virtual void SetGateBarController(GateBarController gateBarController)
    {
        this.gateBarController = gateBarController;
        gateBarController.SetInit(maxGate, gateBarRegenFull1GatePerSecond);
    }

    protected void ReuseGameObject(GameObject gameObject, Vector3 localPosition, bool parent)
    {
        ReuseGameObject(gameObject, localPosition, parent, thisTransform);
    }

    protected void ReuseGameObject(GameObject gameObject, Vector3 localPosition,
        bool parent, Transform parentTransform)
    {
        Transform gameObjectTransform = gameObject.transform;

        gameObjectTransform.parent = parentTransform;
        gameObjectTransform.localPosition = localPosition;
        if (!parent)
            gameObjectTransform.parent = null;
        else if (!listGameObjectTransformInParent.Contains(gameObjectTransform))
            listGameObjectTransformInParent.Add(gameObjectTransform);
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
