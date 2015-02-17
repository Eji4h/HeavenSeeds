using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public abstract class Unit : MonoBehaviour
{
    #region Static Variable
    protected static int randomNumSecurity;

    protected static Monster monster;

    protected static List<CharacterController> listCharacterController = new List<CharacterController>(5);
    protected static CharacterController swordCharacterController,
        bowCharacterController,
        wandCharacterController,
        shieldCharacterController,
        scrollCharacterController;

    protected static List<CharacterController> listCharacterControllerIsFall = new List<CharacterController>(5);

    protected static MagicFieldController magicFieldController;
    protected static TurnController turnController;
    #endregion

    #region Static Properties
    public static Monster Monster
    {
        get { return Unit.monster; }
        set { Unit.monster = value; }
    }
    #endregion

    #region Static Method
    public static void GenRandomNumSecurity()
    {
        randomNumSecurity = UnityEngine.Random.Range(0, 1000);
    }

    public static void SetInit()
    {
        swordCharacterController = SceneController.SwordCharacterController;
        bowCharacterController = SceneController.BowCharacterController;
        wandCharacterController = SceneController.WandCharacterController;
        shieldCharacterController = SceneController.ShieldCharacterController;
        scrollCharacterController = SceneController.ScrollCharacterController;

        monster = SceneController.CurrentMonster;

        magicFieldController = SceneController.MagicFieldController;
        turnController = SceneController.TurnController;
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
    #endregion

    #region Variable
    protected Transform thisTransform;
    protected Animation thisAnimation;

    [SerializeField]
    [Range(0, 10000)]
    protected int maxHp;

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
    #endregion

    #region Properties
    protected virtual int MaxHp
    {
        get { return maxHp / randomNumSecurity; }
        set { maxHp = value * randomNumSecurity; }
    }
    #endregion

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
