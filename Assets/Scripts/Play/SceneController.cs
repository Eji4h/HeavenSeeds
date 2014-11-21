using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SceneController : MonoBehaviour
{
    #region Variable
    static Camera mainCamera,
        uiCamera;

    static CharacterController swordCharacterController,
        bowCharacterController,
        wandCharacterController,
        shieldCharacterController,
        scrollCharacterController;

    static Monster currentMonster;
    static Queue<Monster> queueMonster = new Queue<Monster>(3);

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

    public static Monster CurrentMonster
    {
        get { return SceneController.currentMonster; }
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

    #region Static Method
    public static void NextMonsterQueue()
    {
        if (queueMonster.Count > 0)
        {
            currentMonster = queueMonster.Dequeue();
            Unit.Monster = currentMonster;
            currentMonster.gameObject.SetActive(true);
        }
    }
    #endregion

    void Awake()
    {
        Unit.GenRandomNumSecurity();
        queueMonster.Clear();
        SetCameraObjects();
        SetCharacters();
        SetMagicFieldController();
        SetTurnController();
    }

    // Use this for initialization
    void Start()
    {
        UIController.SetInit();
        MagicPoint.SetInit();
        Unit.SetInit();
        Monster.SetInit();
        CharacterController.SetInit();

        int sceneSelected = PlayerPrefs.GetInt("SceneSelected", 1);
        string monsterPath = "Prefabs/Monsters/Scene" + sceneSelected,
            sceneSetPath = "Prefabs/Scenes/SceneSet" + sceneSelected;
        List<Monster> listMonster = new List<Monster>(5);

        Instantiate(Resources.Load(sceneSetPath));

        switch (sceneSelected)
        {
            case 1:
                listMonster.Add(InstantiateMonster(monsterPath + "/DuRongKraiSorn"));
                listMonster.Add(InstantiateMonster(monsterPath + "/PayakKraiSorn"));
                listMonster.Add(InstantiateMonster(monsterPath + "/BossKhchsingh"));
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
        }

        listMonster.ForEach(monster =>
            {
                monster.gameObject.SetActive(false);
                queueMonster.Enqueue(monster);
            });

        NextMonsterQueue();
    }

    Monster InstantiateMonster(string path)
    {
        return Instantiate(Resources.Load<Monster>(path), Vector3.forward * 20f + Vector3.up * 2f,
            Quaternion.AngleAxis(180, Vector3.up)) as Monster;
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

        swordCharacterController.SetStatus();
        bowCharacterController.SetStatus();
        wandCharacterController.SetStatus();
        shieldCharacterController.SetStatus();
        scrollCharacterController.SetStatus();
    }

    void SetTurnController()
    {
        turnController = GameObject.FindObjectOfType<TurnController>();
    }
}
