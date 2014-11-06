using UnityEngine;
using System.Collections;

public abstract class Boss : Monster
{
    #region Variable

    #endregion

    #region Properties

    #endregion
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected abstract IEnumerator BossFSM();
}
