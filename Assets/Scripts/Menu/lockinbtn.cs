using UnityEngine;
using System.Collections;

public class lockinbtn : MonoBehaviour {
    public static bool islockin = false;
    // Use this for initialization
    void Start()
    {
        EventDelegate.Add(GetComponent<UIButton>().onClick, LockinBtnOnClick);
    }
   
    void LockinBtnOnClick()
    {
        islockin = true;
    }
}
