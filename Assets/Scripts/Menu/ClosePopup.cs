using UnityEngine;
using System.Collections;

public class ClosePopup : MonoBehaviour {


    // Use this for initialization
    void Start()
    {
        EventDelegate.Add(GetComponent<UIButton>().onClick, CloseOnClick);
    }

    void CloseOnClick()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
