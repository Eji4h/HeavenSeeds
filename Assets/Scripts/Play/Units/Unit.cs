using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public abstract class Unit : MonoBehaviour
{
    static List<CharacterController> listCharacterController = new List<CharacterController>(5);

    static CharacterController swordCharacterController,
        bowCharacterController,
        wandCharacterController,
        shieldCharacterController,
        scrollCharacterController;

    static List<CharacterController> listCharacterControllerIsFall = new List<CharacterController>(5);

    static MagicFieldController magicFieldController;

    protected static List<CharacterController> ListCharacterController
    {
        get { return Unit.listCharacterController; }
    }

    protected static CharacterController SwordCharacterController
    {
        get { return Unit.swordCharacterController; }
    }

    protected static CharacterController BowCharacterController
    {
        get { return Unit.bowCharacterController; }
    }

    protected static CharacterController WandCharacterController
    {
        get { return Unit.wandCharacterController; }
    }

    protected static CharacterController ShieldCharacterController
    {
        get { return Unit.shieldCharacterController; }
    }

    protected static CharacterController ScrollCharacterController
    {
        get { return Unit.scrollCharacterController; }
    }

    protected static List<CharacterController> ListCharacterControllerIsFall
    {
        get { return Unit.listCharacterControllerIsFall; }
    }

    protected static MagicFieldController MagicFieldController
    {
        get { return Unit.magicFieldController; }
    }

    public static void SetInit()
    {
        swordCharacterController = SceneController.SwordCharacterController;
        bowCharacterController = SceneController.BowCharacterController;
        wandCharacterController = SceneController.WandCharacterController;
        shieldCharacterController = SceneController.ShieldCharacterController;
        scrollCharacterController = SceneController.ScrollCharacterController;

        magicFieldController = SceneController.MagicFieldController;
    }

    public static void SetCharactersController(CharacterController swordCharacterController,
        CharacterController bowCharacterController, CharacterController wandCharacterController,
        CharacterController shieldCharacterController, CharacterController scrollCharacterController)
    {
        Monster.swordCharacterController = swordCharacterController;
        Monster.bowCharacterController = bowCharacterController;
        Monster.wandCharacterController = wandCharacterController;
        Monster.shieldCharacterController = shieldCharacterController;
        Monster.scrollCharacterController = scrollCharacterController;
    }

    Transform thisTransform;
    Animation thisAnimation;

    [SerializeField]
    [Range(0, 10000)]
    int maxHp;

    GateBarController gateBarController;

    [SerializeField]
    [Range(0, 10)]
    protected int maxGate;

    [SerializeField]
    [Range(0f, 1f)]
    float gateBarRegenPerSecond;

    Predicate<int> checkGateCountIsTarget;
    Action gateCountTargetAction;

    protected List<Transform> listGameObjectTransformInParent = new List<Transform>(8);

    protected Transform ThisTransform
    {
        get { return thisTransform; }
    }

    protected Animation ThisAnimation
    {
        get { return thisAnimation; }
    }

    protected virtual int MaxHp
    {
        get { return maxHp / NumberSecurity.RandomNumSecurity; }
        set { maxHp = value * NumberSecurity.RandomNumSecurity; }
    }

    protected virtual void Awake()
    {
        thisTransform = transform;
        thisAnimation = animation;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        gateBarController.SetInit(maxGate, gateBarRegenPerSecond,
            checkGateCountIsTarget, gateCountTargetAction);
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
