using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    static HpPopUp[] hpPopUpArray;
    static int hpPopUpCurrentIndex = 0,
        hpPopUpCount = 20;

    static PayUIProgressBar playerHpBar;
    static List<PayUIProgressBar> listMonsterHpBar;

    static List<GateBarController> listGateBarController;

    static SpinButton spinButton;
    static UIButton pauseButton;

    static ElementBarController fireElementBarController,
        waterElementBarController,
        earthElementBarController,
        woodElementBarController;

    public static PayUIProgressBar PlayerHpBar
    {
        get { return UIController.playerHpBar; }
    }

    public static List<PayUIProgressBar> ListMonsterHpBar
    {
        get { return UIController.listMonsterHpBar; }
    }

    public static List<GateBarController> ListGateBarController
    {
        get { return UIController.listGateBarController; }
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

    public static void SetInit()
    {
        playerHpBar = GameObject.Find("PlayerHpBar").GetComponent<PayUIProgressBar>();

        SetButtons();
        SetElementBars();
        SetInitHpPopUp();
    }

    static void SetButtons()
    {
        spinButton = GameObject.Find("SpinButton").GetComponent<SpinButton>();
        pauseButton = GameObject.Find("PauseButton").GetComponent<UIButton>();
    }

    static void SetElementBars()
    {
        fireElementBarController = GameObject.Find("FireElementBar").GetComponent<ElementBarController>();
        waterElementBarController = GameObject.Find("WaterElementBar").GetComponent<ElementBarController>();
        earthElementBarController = GameObject.Find("EarthElementBar").GetComponent<ElementBarController>();
        woodElementBarController = GameObject.Find("WoodElementBar").GetComponent<ElementBarController>();
    }

    public static void SetInitBar(int monsterAmount)
    {
        SetInitMonsterHpBar(monsterAmount);
        SetInitGateBar(monsterAmount);
    }

    public static void ShowHpPopUp(int value, Vector3 targetPos, Color32 color)
    {
        hpPopUpArray[hpPopUpCurrentIndex++].PopUp(value, targetPos, color);
        hpPopUpCurrentIndex %= hpPopUpCount;
    }

    static void SetInitMonsterHpBar(int monsterAmount)
    {
        PayUIProgressBar monsterHpBarPrefab = Resources.Load<PayUIProgressBar>("Prefabs/UI/monsterHpBarPrefab");

        listMonsterHpBar = new List<PayUIProgressBar>(monsterAmount);

        for (int i = 0; i < listMonsterHpBar.Count; i++)
            listMonsterHpBar.Add(Instantiate(monsterHpBarPrefab) as PayUIProgressBar);
    }

    static void SetInitGateBar(int monsterAmount)
    {
        GateBarController gateBarPrefab = Resources.Load<GateBarController>("Prefabs/UI/GateBarPrefab");

        listGateBarController = new List<GateBarController>(monsterAmount + 5);

        for (int i = 0; i < listGateBarController.Count; i++)
            listGateBarController.Add(Instantiate(gateBarPrefab) as GateBarController);
    }

    static void SetInitHpPopUp()
    {
        HpPopUp.SetCamera();
        HpPopUp hpPopUpPrefab = Resources.Load<HpPopUp>("Prefabs/UI/HpPopUp");

        hpPopUpArray = new HpPopUp[hpPopUpCount];

        for (int i = 0; i < hpPopUpCount; i++)
            hpPopUpArray[i] = Instantiate(hpPopUpPrefab) as HpPopUp;
    }
}
