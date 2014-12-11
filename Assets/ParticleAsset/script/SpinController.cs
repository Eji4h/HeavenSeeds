using UnityEngine;
using System.Collections;

public class SpinController : MonoBehaviour {
    public float setRotX;
    public float setRotY;
    public float setRotZ;
    public float stopIn;
    public bool canStop = false;
    public bool isOvertime = false;
    public float overTime,valClamp;
    private float stopinSet;

    void OnEnable()
    {
        stopinSet = stopIn;
        StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        for (; ; )
        {
            if (isOvertime)
            {
                if (setRotX != 0 && setRotX <= valClamp)
                    setRotX += overTime * Time.deltaTime;
                if (setRotY != 0 && setRotY <= valClamp)
                    setRotY += overTime * Time.deltaTime;
                if (setRotZ != 0 && setRotZ <= valClamp)
                    setRotZ += overTime * Time.deltaTime;
            }
            if (canStop)
            {
                stopinSet -= Time.deltaTime;
                if (stopinSet >= 0)
                    transform.Rotate(setRotX * Time.deltaTime, setRotY * Time.deltaTime, setRotZ * Time.deltaTime);
            }
            else
                transform.Rotate(setRotX * Time.deltaTime, setRotY * Time.deltaTime, setRotZ * Time.deltaTime);
            yield return null;
        }
    }
}
