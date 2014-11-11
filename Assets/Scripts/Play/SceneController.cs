using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SceneController : MonoBehaviour
{
    #region Variable
    static Camera mainCamera,
        uiCamera;

    static string monsterPath;

    static CharacterController swordCharacterController,
        bowCharacterController,
        wandCharacterController,
        shieldCharacterController,
        scrollCharacterController;

    static List<Monster> listMonsters = new List<Monster>(5);

    static MagicFieldController magicFieldController;

    static TurnController turnController;
    #endregion

    #region Properteis
    public static Camera MainCamera
    {
        get { return SceneController.mainCamera; }
    }

    public static Camera UICamera
    {
        get { return SceneController.uiCamera; }
    }
    public static string MonsterPath
    {
        get { return SceneController.monsterPath; }
    }

    public static CharacterController SwordCharacterController
    {
        get { return swordCharacterController; }
    }

    public static CharacterController BowCharacterController
    {
        get { return bowCharacterController; }
    }

    public static CharacterController WandCharacterController
    {
        get { return wandCharacterController; }
    }

    public static CharacterController ShieldCharacterController
    {
        get { return shieldCharacterController; }
    }

    public static CharacterController ScrollCharacterController
    {
        get { return scrollCharacterController; }
    }

    public static List<Monster> ListMonsters
    {
        get { return listMonsters; }
    }

    public static MagicFieldController MagicFieldController
    {
        get { return SceneController.magicFieldController; }
    }

    public static TurnController TurnController
    {
        get { return SceneController.turnController; }
    }
    #endregion

    void Awake()
    {
        listMonsters.Clear();
        SetCameraObjects();
        SetMagicFieldController();
        SetCharacters();
        SetTurnController();
        MagicPoint.SetInit();
    }

    // Use this for initialization
    void Start()
    {
        Unit.SetInit();
        Monster.SetInit();
        CharacterController.SetInit();
        int sceneSelected = PlayerPrefs.GetInt("SceneSelected", 1);
        monsterPath = "Prefabs/Monsters/Scene" + sceneSelected;
        string bossPath = monsterPath,
            sceneSetPath = "Prefabs/Scenes/SceneSet" + sceneSelected;

        switch (sceneSelected)
        {
            case 1:
                bossPath += "/BossKhchsingh";
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
            case 9:

                break;
            case 10:

                break;
            case 11:

                break;
            case 12:

                break;
            case 13:

                break;
            default:
                bossPath += "/BossKhchsingh";
                break;
        }

        Monster boss = Instantiate(Resources.Load<Monster>(bossPath), Vector3.forward * 20f + Vector3.up,
            Quaternion.AngleAxis(180, Vector3.up)) as Monster;

        Instantiate(Resources.Load(sceneSetPath));

    }

    void SetCameraObjects()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    void SetMagicFieldController()
    {
        magicFieldController = GameObject.FindObjectOfType<MagicFieldController>();
    }

    void SetCharacters()
    {
        GameObject allCharactersGameObject = GameObject.Find("Characters");

        GameObject swordPositionObj = allCharactersGameObject.transform.Find("SwordPosition").gameObject,
            bowPositionObj = allCharactersGameObject.transform.Find("BowPosition").gameObject,
            wandPositionObj = allCharactersGameObject.transform.Find("WandPosition").gameObject,
            shieldPositionObj = allCharactersGameObject.transform.Find("ShieldPosition").gameObject,
            scrollPositionObj = allCharactersGameObject.transform.Find("ScrollPosition").gameObject;

        string swordCharacterName = PlayerPrefs.GetString("SwordCharacter", "Shun"),
            bowCharacterName = PlayerPrefs.GetString("BowCharacter", "Sansa"),
            wandCharacterName = PlayerPrefs.GetString("WandCharacter", "Yana"),
            shieldCharacterName = PlayerPrefs.GetString("ShieldCharacter", "Goma"),
            scrollCharacterName = PlayerPrefs.GetString("ScrollCharacter", "Hansa");

        string prefabsCharacterPath = "Prefabs/Characters/";

        swordCharacterController = (Instantiate(Resources.Load(prefabsCharacterPath + swordCharacterName))
            as GameObject).GetComponent<CharacterController>();
        bowCharacterController = (Instantiate(Resources.Load(prefabsCharacterPath + bowCharacterName))
            as GameObject).GetComponent<CharacterController>();
        wandCharacterController = (Instantiate(Resources.Load(prefabsCharacterPath + wandCharacterName))
            as GameObject).GetComponent<CharacterController>();
        shieldCharacterController = (Instantiate(Resources.Load(prefabsCharacterPath + shieldCharacterName))
            as GameObject).GetComponent<CharacterController>();
        scrollCharacterController = (Instantiate(Resources.Load(prefabsCharacterPath + scrollCharacterName))
            as GameObject).GetComponent<CharacterController>();

        Transform swordCharacterControllerTransform = swordCharacterController.transform,
            bowCharacterControllerTransform = bowCharacterController.transform,
            wandCharacterControllerTransform = wandCharacterController.transform,
            shieldCharacterControllerTransform = shieldCharacterController.transform,
            scrollCharacterControllerTransform = scrollCharacterController.transform;

        swordCharacterControllerTransform.parent = swordPositionObj.transform;
        bowCharacterControllerTransform.parent = bowPositionObj.transform;
        wandCharacterControllerTransform.parent = wandPositionObj.transform;
        shieldCharacterControllerTransform.parent = shieldPositionObj.transform;
        scrollCharacterControllerTransform.parent = scrollPositionObj.transform;

        swordCharacterControllerTransform.localPosition = Vector3.zero;
        bowCharacterControllerTransform.localPosition = Vector3.zero;
        wandCharacterControllerTransform.localPosition = Vector3.zero;
        shieldCharacterControllerTransform.localPosition = Vector3.zero;
        scrollCharacterControllerTransform.localPosition = Vector3.zero;

        string swordWeaponName = PlayerPrefs.GetString("SwordWeapon", "Sword1"),
            bowWeaponName = PlayerPrefs.GetString("BowWeapon", "Bow1"),
            wandWeaponName = PlayerPrefs.GetString("WandWeapon", "Wand1"),
            shieldWeaponName = PlayerPrefs.GetString("ShieldWeapon", "Shield1"),
            scrollWeaponName = PlayerPrefs.GetString("ScrollWeapon", "Scroll1");

        string prefabsWeaponPath = "Prefabs/Weapons/";

        GameObject swordWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + swordWeaponName)) as GameObject,
            bowWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + bowWeaponName)) as GameObject,
            wandWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + wandWeaponName)) as GameObject,
            shieldWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + shieldWeaponName)) as GameObject,
            scrollWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + scrollWeaponName)) as GameObject;

        swordCharacterController.SetWeapon(CharacterActionState.SwordAction, swordWeaponGameObject);
        bowCharacterController.SetWeapon(CharacterActionState.BowAction, bowWeaponGameObject);
        wandCharacterController.SetWeapon(CharacterActionState.WandAction, wandWeaponGameObject);
        shieldCharacterController.SetWeapon(CharacterActionState.ShieldAction, shieldWeaponGameObject);
        scrollCharacterController.SetWeapon(CharacterActionState.ScrollAction, scrollWeaponGameObject);
    }

    void SetTurnController()
    {
        turnController = GameObject.FindObjectOfType<TurnController>();
    }
}
