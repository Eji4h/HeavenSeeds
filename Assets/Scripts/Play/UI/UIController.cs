using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region Static Variable
    static HpPopUp[] hpPopUpArray;
    static int hpPopUpCurrentIndex = 0,
        hpPopUpCount = 20;

    static PayUIProgressBar playerHpBar,
        monsterHpBar;

    static UILabel manaLabel;

    static EndTurnButton endTurnButton;
    #endregion

    #region Static Properties
    public static void SetInit()
    {
        playerHpBar = GameObject.Find("PlayerHpBar").GetComponent<PayUIProgressBar>();
        monsterHpBar = GameObject.Find("MonsterHpBar").GetComponent<PayUIProgressBar>();
        manaLabel = GameObject.Find("ManaLabel").GetComponent<UILabel>();
        endTurnButton = GameObject.FindObjectOfType<EndTurnButton>();
    }
    public static PayUIProgressBar PlayerHpBar
    {
        get { return UIController.playerHpBar; }
    }

    public static PayUIProgressBar MonsterHpBar
    {
        get { return UIController.monsterHpBar; }
    }
    public static UILabel ManaLabel
    {
        get { return UIController.manaLabel; }
    }

    public static EndTurnButton EndTurnButton
    {
        get { return UIController.endTurnButton; }
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

    public static void ShowHpPopUp(int value, Vector3 targetPos, bool isDmg)
    {
        hpPopUpArray[hpPopUpCurrentIndex++].PopUp(value, targetPos, isDmg);
        hpPopUpCurrentIndex %= hpPopUpCount;
    }
}
