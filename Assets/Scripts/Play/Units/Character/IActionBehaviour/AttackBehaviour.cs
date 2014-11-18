using UnityEngine;
using System.Collections;
using PayUnity;

public class AttackBehaviour : IActionBehaviour
{
    int atkValue;
    float minimumDmgMultiply, maximumDmgMultiply;

    public AttackBehaviour(int atkValue, float minimumDmgMultiply, float maximumDmgMultiply)
    {
        this.atkValue = atkValue;
        this.minimumDmgMultiply = minimumDmgMultiply;
        this.maximumDmgMultiply = maximumDmgMultiply;
    }

    public void Action()
    {
        int dmg = Mathf.RoundToInt(OftenMethod.ProbabilityDistribution(
            atkValue, minimumDmgMultiply, maximumDmgMultiply, 3) *
            CharacterController.GetAtkPercentIncrease);
        SceneController.CurrentMonster.ReceiveDamage(dmg);
    }
}
