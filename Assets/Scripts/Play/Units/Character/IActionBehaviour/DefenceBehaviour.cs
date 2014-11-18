using UnityEngine;
using System.Collections;

public class DefenceBehaviour : IActionBehaviour
{
    int barrierHp;
    float blockPercentPerTimeDefence;

    public DefenceBehaviour(int barrierHp, float blockPercentPerTimeDefence)
    {
        this.barrierHp = barrierHp;
        this.blockPercentPerTimeDefence = blockPercentPerTimeDefence;
    }

    public void Action()
    {
        CharacterController.BarrierDefence(barrierHp, blockPercentPerTimeDefence);
    }
}
