using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TurnController : MonoBehaviour
{
    #region Variable
    bool playerTurn = true;
    int incomeCost;
    #endregion

    #region Properties
    public bool PlayerTurn
    {
        get { return playerTurn; }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        incomeCost = PlayerPrefs.GetInt("incomeCost", 13);
        TurnChange();
    }

    public void CharacterActionEnd()
    {
        if (CharacterController.Cost < CharacterController.LowestCost)
            TurnChange();
        else
            SceneController.MagicFieldController.ChangeMgFieldState(true);
    }

    public void TurnChange()
    {
        if (playerTurn)
        {
            CharacterController.Cost += incomeCost;
            CharacterController.TurnEffectDecrease();
            SceneController.MagicFieldController.ChangeMgFieldState(true);
        }
        else
        {
            SceneController.CurrentMonster.StartState();
            SceneController.MagicFieldController.ChangeMgFieldState(false);
        }
        UIController.EndTurnButton.Enabled = playerTurn;
        playerTurn = !playerTurn;
    }
}
