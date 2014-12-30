using UnityEngine;
using System;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region Static Variable
    static HpPopUp[] hpPopUpArray;
    static int hpPopUpCurrentIndex = 0,
        hpPopUpCount = 20;

    static PayUIProgressBar playerHpBar,
        monsterHpBar, 
        manaBar;

    static UILabel manaLabel;

    static EndTurnButton endTurnButton;
    static SpinButton spinButton;
    static UIButton pauseButton;

    static ElementBarController fireElementBarController,
        waterElementBarController,
        earthElementBarController,
        woodElementBarController;
    #endregion

    #region Static Properties
    public static int ManaCost
    {
        get { return Convert.ToInt32(manaLabel.text); }
        set 
        {
            manaBar.Value = value;
            manaLabel.text = value.ToString();
        }
    }

    public static PayUIProgressBar PlayerHpBar
    {
        get { return UIController.playerHpBar; }
    }

    public static PayUIProgressBar MonsterHpBar
    {
        get { return UIController.monsterHpBar; }
    }

    public static EndTurnButton EndTurnButton
    {
        get { return UIController.endTurnButton; }
    }

    public static SpinButton SpinButton
    {
        get { return UIController.spinButton; }
    }

    public static UIButton PauseButton
    {
        get { return UIController.pauseButton; }
    }

    public static ElementBarController FireElementBarController
    {
        get { return UIController.fireElementBarController; }
    }

    public static ElementBarController WaterElementBarController
    {
        get { return UIController.waterElementBarController; }
    }

    public static ElementBarController EarthElementBarController
    {
        get { return UIController.earthElementBarController; }
    }

    public static ElementBarController WoodElementBarController
    {
        get { return UIController.woodElementBarController; }
    }
    #endregion

    #region Static Method
    public static void SetInit()
    {
        playerHpBar = GameObject.Find("PlayerHpBar").GetComponent<PayUIProgressBar>();
        monsterHpBar = GameObject.Find("MonsterHpBar").GetComponent<PayUIProgressBar>();
        manaBar = GameObject.Find("ManaBar").GetComponent<PayUIProgressBar>();
        manaBar.MaxValue = 99;
        manaLabel = GameObject.Find("ManaLabel").GetComponent<UILabel>();
        endTurnButton = GameObject.FindObjectOfType<EndTurnButton>();
        spinButton = GameObject.Find("SpinButton").GetComponent<SpinButton>();
        pauseButton = GameObject.Find("PauseButton").GetComponent<UIButton>();

        fireElementBarController = GameObject.Find("FireElementBar").GetComponent<ElementBarController>();
        waterElementBarController = GameObject.Find("WaterElementBar").GetComponent<ElementBarController>();
        earthElementBarController = GameObject.Find("EarthElementBar").GetComponent<ElementBarController>();
        woodElementBarController = GameObject.Find("WoodElementBar").GetComponent<ElementBarController>();
    }

    public static void ShowHpPopUp(int value, Vector3 targetPos, Color32 color)
    {
        hpPopUpArray[hpPopUpCurrentIndex++].PopUp(value, targetPos, color);
        hpPopUpCurrentIndex %= hpPopUpCount;
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        HpPopUp.SetCamera();
        HpPopUp hpPopUpPrefab = Resources.Load<HpPopUp>("Prefabs/UI/HpPopUp");

        hpPopUpArray = new HpPopUp[hpPopUpCount];

        for (int i = 0; i < hpPopUpCount; i++)
            hpPopUpArray[i] = Instantiate(hpPopUpPrefab) as HpPopUp;
    }
}
