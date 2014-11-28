﻿using UnityEngine;
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

        fireElementBarController = GameObject.Find("FireElementBar").GetComponent<ElementBarController>();
        waterElementBarController = GameObject.Find("WaterElementBar").GetComponent<ElementBarController>();
        earthElementBarController = GameObject.Find("EarthElementBar").GetComponent<ElementBarController>();
        woodElementBarController = GameObject.Find("WoodElementBar").GetComponent<ElementBarController>();
    }

    public static void ShowHpPopUp(int value, Vector3 targetPos, bool isDmg)
    {
        ShowHpPopUp(value, targetPos, isDmg ? new Color32(235, 72, 7, 255) : new Color32(228, 213, 116, 255));
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
