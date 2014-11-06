using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public abstract class Unit : MonoBehaviour
{
    #region Static Variable
    protected static int randomNumSecurity;

    protected static List<Monster> listMonsters;
    protected static List<CharacterController> listCharacterController = new List<CharacterController>(5);
    protected static CharacterController swordCharacterController,
        bowCharacterController,
        wandCharacterController,
        shieldCharacterController,
        scrollCharacterController;

    protected static List<CharacterController> listCharacterControllerIsFall = new List<CharacterController>(5);

    protected static float nearPoint,
        middlePoint, farPoint;

    protected static MagicFieldController magicFieldController;
    protected static TurnController turnController;
    #endregion

    #region Static Method
    public static void SetInit()
    {
        swordCharacterController = SceneController.SwordCharacterController;
        bowCharacterController = SceneController.BowCharacterController;
        wandCharacterController = SceneController.WandCharacterController;
        shieldCharacterController = SceneController.ShieldCharacterController;
        scrollCharacterController = SceneController.ScrollCharacterController;

        randomNumSecurity = Random.Range(0, 1000);
        listMonsters = SceneController.ListMonsters;

        nearPoint = SceneController.NearPoint;
        middlePoint = SceneController.MiddlePoint;
        farPoint = SceneController.FarPoint;

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

    int maxHp;
    #endregion

    #region Properties
    public int MaxHp
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
    }

    protected int DamageProbabilityDistribution(float dmgBase, 
        float minimumDmgMultiply, float maximumDmgMultiply)
    {
        if (dmgBase > 0f)
        {
            int divide = 3;
            float dmgBaseMod = dmgBase / divide,
                dmgMinimumMod = dmgBaseMod * minimumDmgMultiply,
                dmgMaximumMod = dmgBaseMod * maximumDmgMultiply,
                resultDmg = 0f;

            for (int i = 0; i < divide; i++)
                resultDmg += Random.Range(dmgMinimumMod, dmgMaximumMod);

            return Mathf.RoundToInt(resultDmg);
        }
        return 0;
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
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
