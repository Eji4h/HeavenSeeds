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


    IEnumerator Start()
    {
        while (true)
        {
            if (isOvertime)
            {
                if (setRotX != 0)
                {
                    if(setRotX <= valClamp)
                        setRotX += overTime*Time.deltaTime;
                }
                if (setRotY != 0)
                {
                    if (setRotY <= valClamp)
                        setRotY += overTime*Time.deltaTime;
                }
                if (setRotZ != 0)
                {
                    if (setRotZ <= valClamp)
                        setRotZ += overTime*Time.deltaTime;
                }
            }
            if (canStop)
            {
              
                stopIn -= Time.deltaTime;
                if (stopIn >= 0)
                    transform.Rotate(setRotX * Time.deltaTime, setRotY * Time.deltaTime, setRotZ * Time.deltaTime);
            }
            else
                transform.Rotate(setRotX * Time.deltaTime, setRotY * Time.deltaTime, setRotZ * Time.deltaTime);
            yield return 0;
        }
    }
}
