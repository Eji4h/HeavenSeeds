using UnityEngine;
using System.Collections;

public class ChangeSceneBtn : MonoBehaviour
{
    [SerializeField]
    public string sceneGoToString;

    // Use this for initialization
    void Start()
    {
        EventDelegate.Add(GetComponent<UIButton>().onClick, ChangeSceneBtnOnClick);
    }

    void ChangeSceneBtnOnClick()
    {
        Application.LoadLevel(sceneGoToString);
    }
}
