using UnityEngine;
using System.Collections;

public class GameManager
{
    #region Singleton
    static volatile GameManager instance;
    static object syncRoot = new object();

    GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new GameManager();
                }
            return instance;
        }
    }
    #endregion

    #region Pause
    bool pause = false;

    public bool Pause
    {
        get { return pause; }
        set
        {
            pause = value;
            Time.timeScale = (pause) ? 0f : 1f;
        }
    }

    public IEnumerator PauseRoutine()
    {
        while (pause)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion
}
