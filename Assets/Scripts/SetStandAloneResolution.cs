using UnityEngine;
using System.Collections;

public class SetStandAloneResolution : MonoBehaviour
{
    void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(480, 800, false);
#endif
    }
}
