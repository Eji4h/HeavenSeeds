using UnityEngine;
using System.Collections;

public class ChangeSceneBtn : MonoBehaviour
{
    public string sceneGoToStr;

    // Use this for initialization
    void Start()
    {
        EventDelegate.Add(GetComponent<UIButton>().onClick, ChangeSceneBtnOnClick);
    }

    void ChangeSceneBtnOnClick()
    {
        Application.LoadLevel(sceneGoToStr);
    }
}
