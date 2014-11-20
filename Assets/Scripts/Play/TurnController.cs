using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TurnController : MonoBehaviour
{
    #region Variable
    bool playerTurn = false;
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
        playerTurn = !playerTurn;
        if (playerTurn)
        {
            CharacterController.Cost += incomeCost;
            CharacterController.TurnEffectDecrease();
        }
        else
            SceneController.CurrentMonster.StartState();

        SceneController.MagicFieldController.ChangeMgFieldState(playerTurn);
        UIController.EndTurnButton.Enabled = playerTurn;
    }
}
