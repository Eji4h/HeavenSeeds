using UnityEngine;
using System.Collections;

public class ChangeSceneBtn : MonoBehaviour
{
    public string sceneGoToStr;

    void OnClick()
    {
        Application.LoadLevel(sceneGoToStr);
    }
}
