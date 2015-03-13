using UnityEngine;
using System.Collections;

public class MonoAndCoroutinePauseBehaviour : MonoBehaviour
{
    protected Coroutine _sync()
    {
        return StartCoroutine(GameManager.Instance.PauseRoutine());
    }
}
