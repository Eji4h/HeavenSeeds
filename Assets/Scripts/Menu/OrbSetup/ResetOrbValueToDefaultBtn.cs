using UnityEngine;
using System.Collections;

public class ResetOrbValueToDefaultBtn : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        EventDelegate.Add(GetComponent<UIButton>().onClick, ResetOrbValueToDefaultBtnOnClick);
    }

    void ResetOrbValueToDefaultBtnOnClick()
    {
        OrbValueController.ResetOrbValue();
    }
}
