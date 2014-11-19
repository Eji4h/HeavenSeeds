﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TurnController : MonoBehaviour
{
    #region Variable
    bool playerTurn = true;
    int incomeCost;
    #endregion

    // Use this for initialization
    void Start()
    {
        incomeCost = PlayerPrefs.GetInt("incomeCost", 10);
        TurnChange();
    }

    public void CharacterActionEnd()
    {
        if (CharacterController.Cost < CharacterController.LowestCost)
            TurnChange();
        else
            SceneController.MagicFieldController.ChangeStateToWaitingCommand();
    }

    public void TurnChange()
    {
        if (playerTurn)
        {
            CharacterController.Cost += incomeCost;
            SceneController.MagicFieldController.ChangeStateToWaitingCommand();
        }
        else
            SceneController.CurrentMonster.StartState();
        UIController.EndTurnButton.Enabled = playerTurn;
        playerTurn = !playerTurn;
    }
}
