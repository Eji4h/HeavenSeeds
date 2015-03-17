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

    static List<CharacterController> listCharacterController = new List<CharacterController>(5),
        listCharacterControllerIsFall = new List<CharacterController>(5);

    static Transform monstersParentTransform;
    static Monster currentMonster;
    static Queue<Monster> queueMonster = new Queue<Monster>(3);

    static MagicFieldController magicFieldController;

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

    public static List<CharacterController> ListCharacterController
    {
        get { return SceneController.listCharacterController; }
    }

    public static List<CharacterController> ListCharacterControllerIsFall
    {
        get { return SceneController.listCharacterControllerIsFall; }
    }

    public static Monster CurrentMonster
    {
        get { return SceneController.currentMonster; }
    }

    public static MagicFieldController MagicFieldController
    {
        get { return SceneController.magicFieldController; }
    }

    void Awake()
    {
        queueMonster.Clear();
        SetCameraObjects();
        SetMagicFieldController();
        SetCharacters();
    }

    // Use this for initialization
    void Start()
    {
        UIController.SetInit();
        MagicPoint.SetInit();
        Monster.SetInit();
        CharacterController.SetInit();

        monstersParentTransform = GameObject.Find("Monsters").transform;
        int sceneSelected = PlayerPrefs.GetInt("SceneSelected", 1);
        //For monsters test
        //sceneSelected = 2;
        string monsterPath = "Prefabs/Monsters/Scene" + sceneSelected,
            sceneSetPath = "Prefabs/Scenes/SceneSet" + sceneSelected;

        Instantiate(Resources.Load(sceneSetPath));

        var monstersPrefab = Resources.LoadAll<Monster>(monsterPath);
        var listMonster = new List<Monster>(monstersPrefab.Length);

        foreach(var monsterPrefab in monstersPrefab)
            listMonster.Add(Instantiate(monsterPrefab) as Monster);

        UIController.SetInitBar(listMonster.Count);

        for (int i = 0; i < listMonster.Count; i++)
            listMonster[i].HpBar = UIController.ListMonsterHpBar[i];

        int indexGateBar = 0;

        listCharacterController.ForEach(characterController =>
            {
                characterController.SetGateBarController(UIController.ListGateBarController[indexGateBar]);
                indexGateBar++;
            });
        listMonster.ForEach(monster =>
            {
                monster.SetGateBarController(UIController.ListGateBarController[indexGateBar]);
                indexGateBar++;
            });

        listMonster.ForEach(monster =>
            {
                monster.transform.parent = monstersParentTransform;
                monster.transform.localPosition = Vector3.up * 2f;
                monster.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.up);
                monster.gameObject.SetActive(false);
                queueMonster.Enqueue(monster);
            });

        listMonster[0].gameObject.SetActive(true);

        //for (int i = listMonster.Count - 1; i >= 0; i--)
        //    listMonster[i].transform.localPosition = new Vector3(3f * (i - 1), 2f);
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

    enum WeaponType
    {
        Stone,
        Iron,
        Diamond
    }

    [SerializeField]
    WeaponType weaponType = WeaponType.Stone;

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

        string swordWeaponType = PlayerPrefs.GetString("SwordWeaponType", "Stone"),
            bowWeaponType = PlayerPrefs.GetString("BowWeaponType", "Stone"),
            wandWeaponType = PlayerPrefs.GetString("WandWeaponType", "Stone"),
            shieldWeaponType = PlayerPrefs.GetString("ShieldWeaponType", "Stone"),
            scrollWeaponType = PlayerPrefs.GetString("ScrollWeaponType", "Stone");

        string prefabsWeaponPath = "Prefabs/Weapons/";

        GameObject swordWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + "Sword_" + swordWeaponType)) as GameObject,
            bowWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + "Bow_" + bowWeaponType)) as GameObject,
            wandWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + "Wand_" + wandWeaponType)) as GameObject,
            shieldWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + "Shield_" + shieldWeaponType)) as GameObject,
            scrollWeaponGameObject = Instantiate(Resources.Load(prefabsWeaponPath + "Scroll_" + scrollWeaponType)) as GameObject;

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

        swordWeaponType = weaponType.ToString();
        bowWeaponType = weaponType.ToString();
        wandWeaponType = weaponType.ToString();
        shieldWeaponType = weaponType.ToString();
        scrollWeaponType = weaponType.ToString();

        GameObject swordFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + "Sword_" + swordWeaponType) as GameObject,
            bowFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + "Bow_" + bowWeaponType) as GameObject,
            wandFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + "Wand_" + wandWeaponType) as GameObject,
            shieldFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + "Shield_" + shieldWeaponType) as GameObject,
            scrollFxAnimationPrefab = Resources.Load(prefabsWeaponOverlayFxPath + "Scroll_" + scrollWeaponType) as GameObject;

        MagicFieldController.swordFxAnimation = Instantiate(swordFxAnimationPrefab,
            swordFxAnimationPrefab.transform.position, swordFxAnimationPrefab.transform.rotation) as GameObject;
        MagicFieldController.bowFxAnimation = Instantiate(bowFxAnimationPrefab,
            bowFxAnimationPrefab.transform.position, bowFxAnimationPrefab.transform.rotation) as GameObject;
        MagicFieldController.wandFxAnimation = Instantiate(wandFxAnimationPrefab,
            wandFxAnimationPrefab.transform.position, wandFxAnimationPrefab.transform.rotation) as GameObject;
        MagicFieldController.shieldFxAnimation = Instantiate(shieldFxAnimationPrefab,
            shieldFxAnimationPrefab.transform.position, shieldFxAnimationPrefab.transform.rotation) as GameObject;
        MagicFieldController.scrollFxAnimation = Instantiate(scrollFxAnimationPrefab,
            scrollFxAnimationPrefab.transform.position, scrollFxAnimationPrefab.transform.rotation) as GameObject;

        MagicFieldController.swordFxAnimation.SetActive(false);
        MagicFieldController.bowFxAnimation.SetActive(false);
        MagicFieldController.wandFxAnimation.SetActive(false);
        MagicFieldController.shieldFxAnimation.SetActive(false);
        MagicFieldController.scrollFxAnimation.SetActive(false);
    }
}
