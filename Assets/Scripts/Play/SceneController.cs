using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SceneController : MonoBehaviour
{
    static Camera mainCamera,
        uiCamera,
        overlayFxCamera;

    static CharacterController swordCharacterController,
        bowCharacterController,
        wandCharacterController,
        shieldCharacterController,
        scrollCharacterController;

    static Transform monstersParentTransform;
    static Monster currentMonster;
    static Queue<Monster> queueMonster = new Queue<Monster>(3);

    static MagicFieldController magicFieldController;

    static TurnController turnController;

    public static Camera MainCamera
    {
        get { return SceneController.mainCamera; }
    }

    public static Camera UICamera
    {
        get { return SceneController.uiCamera; }
    }

    public static Camera OverlayFxCamera
    {
        get { return SceneController.overlayFxCamera; }
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

    public static void NextMonsterQueue()
    {
        if (queueMonster.Count > 0)
        {
            currentMonster = queueMonster.Dequeue();
            Unit.Monster = currentMonster;
            currentMonster.gameObject.SetActive(true);
        }
    }

    void Awake()
    {
        Unit.GenRandomNumSecurity();
        queueMonster.Clear();
        SetCameraObjects();
        SetMagicFieldController();
        SetCharacters();
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

        monstersParentTransform = GameObject.Find("Monsters").transform;
        int sceneSelected = PlayerPrefs.GetInt("SceneSelected", 1);
        string monsterPath = "Prefabs/Monsters/Scene" + sceneSelected,
            sceneSetPath = "Prefabs/Scenes/SceneSet" + sceneSelected;
        List<Monster> listMonster = new List<Monster>(5);

        Instantiate(Resources.Load(sceneSetPath));

        //For monsters test
        sceneSelected = 2;
        monsterPath = monsterPath.Substring(0, monsterPath.Length - 1) + sceneSelected;

        switch (sceneSelected)
        {
            case 1:
                Monster
                    //duRongKraiSorn = InstantiateMonster(monsterPath + "/DuRongKraiSorn").GetComponent<Monster>(),
                    //payakKraiSorn = InstantiateMonster(monsterPath + "/PayakKraiSorn").GetComponent<Monster>(),
                    bossKhchsingh = InstantiateMonster(monsterPath + "/BossKhchsingh").GetComponent<Monster>();

                //listMonster.Add(duRongKraiSorn);
                //listMonster.Add(payakKraiSorn);
                listMonster.Add(bossKhchsingh);
                break;
            case 2:
                Monster
                    bossMachanu = InstantiateMonster(monsterPath + "/BossMuchanu").GetComponent<Monster>();

                listMonster.Add(bossMachanu);
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
                monster.transform.parent = monstersParentTransform;
                monster.transform.localPosition = Vector3.up * 2f;
                monster.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.up);
                monster.gameObject.SetActive(false);
                queueMonster.Enqueue(monster);
            });

        NextMonsterQueue();
    }

    Monster InstantiateMonster(string path)
    {
        return Instantiate(Resources.Load<Monster>(path)) as Monster;
    }

    void SetCameraObjects()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        overlayFxCamera = GameObject.Find("OverlayFxCamera").GetComponent<Camera>();
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

        string swordWeaponName = PlayerPrefs.GetString("SwordWeapon", "Sword_Stone"),
            bowWeaponName = PlayerPrefs.GetString("BowWeapon", "Bow_Stone"),
            wandWeaponName = PlayerPrefs.GetString("WandWeapon", "Wand_Stone"),
            shieldWeaponName = PlayerPrefs.GetString("ShieldWeapon", "Shield_Stone"),
            scrollWeaponName = PlayerPrefs.GetString("ScrollWeapon", "Scroll_Stone");

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

        string prefabsWeaponOverlayFxPath = "Prefabs/Particle/Player/SpellCircle/";

        GameObject swordFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + swordWeaponName) as GameObject,
            bowFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + bowWeaponName) as GameObject,
            wandFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + wandWeaponName) as GameObject,
            shieldFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + shieldWeaponName) as GameObject,
            scrollFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + scrollWeaponName) as GameObject;

        MagicFieldController.swordFxAnimation = Instantiate(swordFxAnimationPrefab, 
            swordFxAnimationPrefab.transform.position, swordFxAnimationPrefab.transform.rotation) as GameObject;
        MagicFieldController.bowFxAnimation = Instantiate(bowFxAnimationPrefab,
            bowFxAnimationPrefab.transform.position, bowFxAnimationPrefab.transform.rotation) as GameObject;
        //MagicFieldController.wandFxAnimation = Instantiate(wandFxAnimationPrefab,
        //    wandFxAnimationPrefab.transform.position, wandFxAnimationPrefab.transform.rotation) as GameObject;
        MagicFieldController.shieldFxAnimation = Instantiate(shieldFxAnimationPrefab,
            shieldFxAnimationPrefab.transform.position, shieldFxAnimationPrefab.transform.rotation) as GameObject;
        //MagicFieldController.scrollFxAnimation = Instantiate(scrollFxAnimationPrefab,
        //    scrollFxAnimationPrefab.transform.position, scrollFxAnimationPrefab.transform.rotation) as GameObject;

        MagicFieldController.swordFxAnimation.SetActive(false);
        MagicFieldController.bowFxAnimation.SetActive(false);
        //MagicFieldController.wandFxAnimation.SetActive(false);
        MagicFieldController.shieldFxAnimation.SetActive(false);
        //MagicFieldController.scrollFxAnimation.SetActive(false);
    }

    void SetTurnController()
    {
        turnController = GameObject.FindObjectOfType<TurnController>();
    }
}
