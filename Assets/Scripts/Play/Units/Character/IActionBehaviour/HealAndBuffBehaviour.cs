using UnityEngine;
using System.Collections;

public class HealAndBuffBehaviour : IActionBehaviour
{
    int healValue;
    float atkPercentIncrease, 
        barrierHpPercentIncrease, 
        healPercentIncrease;

    public HealAndBuffBehaviour(int healValue,
        float atkPercentIncrease, 
        float barrierHpPercentIncrease, 
        float healPercentIncrease)
    {
        this.healValue = healValue;
        this.atkPercentIncrease = atkPercentIncrease;
        this.barrierHpPercentIncrease = barrierHpPercentIncrease;
        this.healPercentIncrease = healPercentIncrease;
    }

    public void Action()
    {
        CharacterController.ReceiveHeal(healValue);
        CharacterController.GetBuff(atkPercentIncrease, barrierHpPercentIncrease, healPercentIncrease);
    }
}
