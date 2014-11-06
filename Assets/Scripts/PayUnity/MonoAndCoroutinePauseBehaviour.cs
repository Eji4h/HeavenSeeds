using UnityEngine;
using System.Collections;
using PayUnity;

public class MonoAndCoroutinePauseBehaviour : MonoBehaviour
{
    protected Coroutine _sync()
    {
        return StartCoroutine(GameManager.Instance.PauseRoutine());
    }
}
