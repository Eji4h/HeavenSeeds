using UnityEngine;
using System.Collections;

public class EndTurnButton : UIButtonMonoBehaviour
{

    protected override void OnClickBehaviour()
    {
        SceneController.TurnController.TurnChange();
    }
}
