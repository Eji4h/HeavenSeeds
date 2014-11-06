using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    static HpPopUp[] hpPopUpArray;
    static int hpPopUpCurrentIndex = 0,
        hpPopUpCount = 20;

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
