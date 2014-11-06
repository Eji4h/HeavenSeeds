using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnController : MonoBehaviour
{
    #region Variable
    bool playerTurn = true;
    #endregion

    // Use this for initialization
    void Start()
    {
        TurnChange();
    }

    public void TurnChange()
    {
        if (playerTurn)
            SceneController.MagicFieldController.ChangeStateToWaitingCommand();
        else
            SceneController.ListMonsters.ForEach(monster =>
                {
                    monster.StartState();
                });
        //playerTurn = !playerTurn;
    }
}
