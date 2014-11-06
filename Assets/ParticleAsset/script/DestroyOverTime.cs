using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {
    public float setTime ;
    private float cTime;
	// Use this for initialization
    IEnumerator Start()
    {
        cTime = setTime;
        while (true)
        {
            cTime -= Time.deltaTime;
            if (cTime <= 0)
                Destroy(gameObject);
            yield return 0;
        }
    }
    
}
