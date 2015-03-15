﻿using UnityEngine;
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

    static Transform allHpPopUpParentTransform,
        barsTransform;

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

    public static Transform AllHpPopUpParentTransform
    {
        get { return UIController.allHpPopUpParentTransform; }
    }

    public static Transform BarsTransform
    {
        get { return UIController.barsTransform; }
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
        barsTransform = GameObject.Find("Bars").transform;
        SetInitMonsterHpBar(monsterAmount);
        SetInitGateBar(monsterAmount + 5);
    }

    public static void ShowHpPopUp(int value, Vector3 targetPos, Color32 color)
    {
        hpPopUpArray[hpPopUpCurrentIndex++].PopUp(value, targetPos, color);
        hpPopUpCurrentIndex %= hpPopUpCount;
    }

    static void SetInitMonsterHpBar(int monsterHpBarAmount)
    {
        var monsterHpBarPrefab = Resources.Load<PayUIProgressBar>("Prefabs/UI/MonsterHpBar");

        listMonsterHpBar = new List<PayUIProgressBar>(monsterHpBarAmount);

        for (int i = 0; i < listMonsterHpBar.Capacity; i++)
            listMonsterHpBar.Add(Instantiate(monsterHpBarPrefab) as PayUIProgressBar);
    }

    static void SetInitGateBar(int gateBarAmount)
    {
        var gateBarPrefab = Resources.Load<GateBarController>("Prefabs/UI/GateBar");

        listGateBarController = new List<GateBarController>(gateBarAmount);

        for (int i = 0; i < listGateBarController.Capacity; i++)
            listGateBarController.Add(Instantiate(gateBarPrefab) as GateBarController);
    }

    static void SetInitHpPopUp()
    {
        allHpPopUpParentTransform = GameObject.Find("AllHpPopUp").transform;
        HpPopUp hpPopUpPrefab = Resources.Load<HpPopUp>("Prefabs/UI/HpPopUp");

        hpPopUpArray = new HpPopUp[hpPopUpCount];

        for (int i = 0; i < hpPopUpCount; i++)
            hpPopUpArray[i] = Instantiate(hpPopUpPrefab) as HpPopUp;
    }
}
