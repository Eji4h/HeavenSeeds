using UnityEngine;
using System.Collections;
using PayUnity;

public class PauseButton : UIButtonMonoBehaviour
{
    bool isPause = false;

    protected override void OnClickBehaviour()
    {
        isPause = !isPause;
        GameManager.Instance.Pause = isPause;
        UIController.EndTurnButton.Enabled = !isPause;
        UIController.SpinButton.Enabled = !isPause;
        //UIController.PauseButton.isEnabled = !isPause;

        SceneController.OverlayFxCamera.gameObject.SetActive(!isPause);
    }
}
