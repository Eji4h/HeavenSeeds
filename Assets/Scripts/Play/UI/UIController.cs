using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region Static Variable
    static HpPopUp[] hpPopUpArray;
    static int hpPopUpCurrentIndex = 0,
        hpPopUpCount = 20;

    static CalculateUIProgressBar playerHpBar,
        monsterHpBar;
    #endregion

    #region Static Properties
    public static CalculateUIProgressBar PlayerHpBar
    {
        get { return UIController.playerHpBar; }
        set { UIController.playerHpBar = value; }
    }

    public static CalculateUIProgressBar MonsterHpBar
    {
        get { return UIController.monsterHpBar; }
        set { UIController.monsterHpBar = value; }
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

        playerHpBar = GameObject.Find("PlayerHpBar").GetComponent<CalculateUIProgressBar>();
        monsterHpBar = GameObject.Find("MonsterHpBar").GetComponent<CalculateUIProgressBar>();
    }

    public static void ShowHpPopUp(int value, Vector3 targetPos, bool isDmg)
    {
        hpPopUpArray[hpPopUpCurrentIndex++].PopUp(value, targetPos, isDmg);
        hpPopUpCurrentIndex %= hpPopUpCount;
    }
}
