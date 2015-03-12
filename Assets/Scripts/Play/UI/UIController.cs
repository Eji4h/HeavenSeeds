using UnityEngine;
using System;
using System.Collections;

public class UIController : MonoBehaviour
{
    static HpPopUp[] hpPopUpArray;
    static int hpPopUpCurrentIndex = 0,
        hpPopUpCount = 20;

    static PayUIProgressBar playerHpBar;

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
