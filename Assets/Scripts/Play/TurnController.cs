using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TurnController : MonoBehaviour
{
    bool playerTurn = false;
    int turnCount = 0,
        incomeCost;

    public bool PlayerTurn
    {
        get { return playerTurn; }
    }

    public int TurnCount
    {
        get { return turnCount; }
    }

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
    }

    public void TurnChange()
    {
        playerTurn = !playerTurn;
        if (playerTurn)
        {
            CharacterController.Cost += incomeCost;
            CharacterController.TurnEffectDecrease();
            if(CharacterController.IsStun)
            {
                CharacterController.StunTurn--;
                playerTurn = false;
            }
            if(CharacterController.IsFreeze)
            {
                CharacterController.FreezeTurn--;
                playerTurn = false;
            }
        }
        else
            SceneController.CurrentMonster.StartState();

        SceneController.MagicFieldController.ChangeMgFieldState(playerTurn);
        UIController.EndTurnButton.TurnChange(playerTurn);
        turnCount++;
    }
}
