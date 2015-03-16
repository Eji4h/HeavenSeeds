using UnityEngine;
using System.Collections;

public class DisableParticle : MonoBehaviour {
    public float settime;
    private float ftime;
   
	// Use this for initialization
    void OnEnable()
    {
        ftime = settime;
    }
	// Update is called once per frame
	void Update () {
        if (gameObject.activeInHierarchy)
        {
            ftime -= Time.deltaTime;
            if (ftime <= 0)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
