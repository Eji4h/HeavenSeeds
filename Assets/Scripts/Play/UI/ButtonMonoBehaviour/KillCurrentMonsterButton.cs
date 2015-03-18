using UnityEngine;
using System.Collections;

public class KillCurrentMonsterButton : UIButtonMonoBehaviour
{

    protected override void OnClickBehaviour()
    {
        Monster currentMonster = SceneController.ChooseMonster;
        if (currentMonster != null)
            currentMonster.Hp = 0;
    }
}
