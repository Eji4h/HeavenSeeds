using UnityEngine;
using System.Collections;

public class PauseButton : UIButtonMonoBehaviour
{
    bool isPause = false;

    protected override void OnClickBehaviour()
    {
        isPause = !isPause;
        GameManager.Instance.Pause = isPause;
        UIController.SpinButton.Enabled = !isPause;
        //UIController.PauseButton.isEnabled = !isPause;

        SceneController.OverlayFxCamera.gameObject.SetActive(!isPause);
    }
}
