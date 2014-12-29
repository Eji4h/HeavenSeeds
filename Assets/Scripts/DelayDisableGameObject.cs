using UnityEngine;
using System.Collections;

public class DelayDisableGameObject : MonoBehaviour
{
    public float timeToDisable;

    void OnEnable()
    {
        StartCoroutine(DelayDisable());
    }

    IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(timeToDisable);
        gameObject.SetActive(false);
    }
}
